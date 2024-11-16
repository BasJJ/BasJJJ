using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Input;
using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.StudentRepository;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.Models.Repositories.RegistrationRepository;
using CoursesManager.UI.Dialogs.ResultTypes;
using CoursesManager.UI.Dialogs.ViewModels;

namespace CoursesManager.UI.ViewModels
{
    public class EditStudentViewModel : DialogViewModel<Student>
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IRegistrationRepository _registrationRepository;
        private readonly IDialogService _dialogService;
        private bool _isDialogOpen;

        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set => SetProperty(ref _isDialogOpen, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
  

        public EditStudentViewModel(
            IStudentRepository studentRepository,
            ICourseRepository courseRepository,
            IRegistrationRepository registrationRepository,
            IDialogService dialogService,
            Student? student)
            : base(student)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
            _registrationRepository = registrationRepository ?? throw new ArgumentNullException(nameof(registrationRepository));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            Student = student ?? throw new ArgumentNullException(nameof(student));
            StudentCopy = student.Copy();

            SelectableCourses = InitializeSelectableCourses();

            SaveCommand = new RelayCommand(async () => await OnSaveAsync());
            CancelCommand = new RelayCommand(OnCancel);
        }
        public Student Student { get; }
        public Student StudentCopy { get; private set; }
        public ObservableCollection<SelectableCourse> SelectableCourses { get; private set; }

        private ObservableCollection<SelectableCourse> InitializeSelectableCourses()
        {
            var registeredCourseIds = _registrationRepository.GetAll()
                .Where(r => r.StudentID == Student.Id)
                .Select(r => r.CourseID)
                .ToHashSet();

            var courses = _courseRepository.GetAll()
                .Select(course => new SelectableCourse
                {
                    ID = course.ID,
                    Name = course.Name,
                    IsSelected = registeredCourseIds.Contains(course.ID)
                })
                .ToList();

            return new ObservableCollection<SelectableCourse>(courses);
        }

        private async Task OnSaveAsync()
        {
            if (await ValidateFieldsAsync())
            {
                if (await ShowYesNoDialogAsync("Wilt u de wijzigingen opslaan?"))
                {
                    UpdateStudentDetails();
                    UpdateRegistrations();

                    await ShowConfirmationDialogAsync("Cursist succesvol opgeslagen.");
                    InvokeResponseCallback(DialogResult<Student>.Builder().SetSuccess(Student, "Success").Build());
                }
            }
        }

        private void UpdateStudentDetails()
        {
            Student.FirstName = StudentCopy.FirstName;
            Student.Insertion = StudentCopy.Insertion;
            Student.LastName = StudentCopy.LastName;
            Student.Email = StudentCopy.Email;
            Student.PhoneNumber = StudentCopy.PhoneNumber;
            Student.PostCode = StudentCopy.PostCode;
            Student.Country = StudentCopy.Country;
            Student.City = StudentCopy.City;
            Student.StreetName = StudentCopy.StreetName;
            Student.HouseNumber = StudentCopy.HouseNumber;

            _studentRepository.Update(Student);
        }

        private void UpdateRegistrations()
        {
            var existingRegistrations = _registrationRepository.GetAll()
                .Where(r => r.StudentID == Student.Id)
                .ToList();

            // Delete unselected registrations
            foreach (var registration in existingRegistrations)
            {
                if (!SelectableCourses.Any(c => c.ID == registration.CourseID && c.IsSelected))
                {
                    _registrationRepository.Delete(registration.ID);
                }
            }

            // Add new registrations
            foreach (var course in SelectableCourses.Where(c => c.IsSelected))
            {
                if (!existingRegistrations.Any(r => r.CourseID == course.ID))
                {
                    _registrationRepository.Add(new Registration
                    {
                        StudentID = Student.Id,
                        CourseID = course.ID,
                        RegistrationDate = DateTime.Now,
                        IsActive = true
                    });
                }
            }
        }

        private void OnCancel()
        {
            var dialogResult = DialogResult<Student>.Builder()
                .SetCanceled("Changes were canceled by the user.")
                .Build();

            InvokeResponseCallback(dialogResult);
        }

        private async Task<bool> ValidateFieldsAsync()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(StudentCopy.FirstName))
                errors.Add("Voornaam is verplicht.");
            if (string.IsNullOrWhiteSpace(StudentCopy.LastName))
                errors.Add("Achternaam is verplicht.");
            if (!IsValidEmail(StudentCopy.Email))
                errors.Add("Het opgegeven e-mailadres is ongeldig.");
            if (string.IsNullOrWhiteSpace(StudentCopy.PhoneNumber))
                errors.Add("Telefoonnummer is verplicht.");
            if (string.IsNullOrWhiteSpace(StudentCopy.PostCode))
                errors.Add("Postcode is verplicht.");
            if (StudentCopy.HouseNumber <= 0)
                errors.Add("Huisnummer is ongeldig.");
            if (string.IsNullOrWhiteSpace(StudentCopy.City))
                errors.Add("Stad is verplicht.");
            if (string.IsNullOrWhiteSpace(StudentCopy.StreetName))
                errors.Add("Straatnaam is verplicht.");
            if (string.IsNullOrWhiteSpace(StudentCopy.Country))
                errors.Add("Land is verplicht.");
            if (!IsUniqueEmail(StudentCopy.Email))
                errors.Add("Dit e-mailadres wordt al gebruikt.");

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    await ShowConfirmationDialogAsync(error);
                }
                return false;
            }

            return true;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var emailAddress = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool IsUniqueEmail(string email)
        {
            return !_studentRepository.EmailExists(email) || email == Student.Email;
        }

        private async Task ShowConfirmationDialogAsync(string message)
        {
            await _dialogService.ShowDialogAsync<ConfirmationDialogViewModel, ConfirmationDialogResultType>(
                new ConfirmationDialogResultType
                {
                    DialogTitle = "Informatie",
                    DialogText = message
                });
        }

        private async Task<bool> ShowYesNoDialogAsync(string message)
        {
            var result = await _dialogService.ShowDialogAsync<YesNoDialogViewModel, YesNoDialogResultType>(
                new YesNoDialogResultType
                {
                    DialogTitle = "Bevestiging",
                    DialogText = message
                });

            return result?.Data?.Result ?? false;
        }
        protected override void InvokeResponseCallback(DialogResult<Student> dialogResult)
        {
            ResponseCallback?.Invoke(dialogResult);
        }
    }
}
