using System.Collections.ObjectModel;
using System.Windows.Input;
using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.StudentRepository;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.Models.Repositories.RegistrationRepository;
using System.Net.Mail;
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
        public Student Student { get; }
        public Student StudentCopy { get; }
        public ObservableCollection<SelectableCourse> SelectableCourses { get; }

        public EditStudentViewModel(IStudentRepository studentRepository, ICourseRepository courseRepository, IRegistrationRepository registrationRepository, IDialogService dialogService, Student? student)
            : base(student)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
            _registrationRepository = registrationRepository ?? throw new ArgumentNullException(nameof(registrationRepository));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            Student = student;
            StudentCopy = student.Copy();

            SelectableCourses = InitializeSelectableCourses();

            SaveCommand = new RelayCommand(async () => await OnSaveAsync());
            CancelCommand = new RelayCommand(OnCancel);
        }

        private ObservableCollection<SelectableCourse> InitializeSelectableCourses()
        {
            var registeredCourseIds = _registrationRepository.GetAll()
                .Where(r => r.StudentID == Student.Id)
                .Select(r => r.CourseID)
                .ToHashSet();

            var selectableCourses = _courseRepository.GetAll()
                .Select(course => new SelectableCourse
                {
                    ID = course.ID,
                    Name = course.Name,
                    IsSelected = registeredCourseIds.Contains(course.ID)
                })
                .ToList();

            return new ObservableCollection<SelectableCourse>(selectableCourses);
        }

        protected override void InvokeResponseCallback(DialogResult<Student> dialogResult)
        {
            ResponseCallback?.Invoke(dialogResult);
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

            foreach (var registration in existingRegistrations)
            {
                if (!SelectableCourses.Any(c => c.ID == registration.CourseID && c.IsSelected))
                {
                    _registrationRepository.Delete(registration.ID);
                }
            }

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

        private async Task OnSaveAsync()
        {
            if (await ValidateFieldsAsync())
            {
                bool confirmSave = await ShowYesNoDialogAsync("Wilt u de wijzigingen opslaan?");

                if (confirmSave)
                {
                    UpdateStudentDetails();
                    UpdateRegistrations();

                    await ShowConfirmationDialogAsync("Cursist succesvol opgeslagen.");

                    InvokeResponseCallback(DialogResult<Student>.Builder().SetSuccess(Student, "Success").Build());
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
            if (string.IsNullOrWhiteSpace(StudentCopy.FirstName))
            {
                await ShowConfirmationDialogAsync("Voornaam is verplicht.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(StudentCopy.LastName))
            {
                await ShowConfirmationDialogAsync("Achternaam is verplicht.");
                return false;
            }

            if (!IsValidEmail(StudentCopy.Email))
            {
                await ShowConfirmationDialogAsync("Het opgegeven e-mailadres is ongeldig.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(StudentCopy.PhoneNumber))
            {
                await ShowConfirmationDialogAsync("Telefoonnummer is verplicht");
                return false;
            }

            if (string.IsNullOrWhiteSpace(StudentCopy.PostCode))
            {
                await ShowConfirmationDialogAsync("Postcode is verplicht.");
                return false;
            }

            if (StudentCopy.HouseNumber <= 0)
            {
                await ShowConfirmationDialogAsync("Huisnummer is ongeldig.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(StudentCopy.City))
            {
                await ShowConfirmationDialogAsync("Stad is verplicht.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(StudentCopy.StreetName))
            {
                await ShowConfirmationDialogAsync("Straatnaam is verplicht.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(StudentCopy.Country))
            {
                await ShowConfirmationDialogAsync("Land is verplicht.");
                return false;
            }

            if (!IsUniqueEmail(StudentCopy.Email))
            {
                await ShowConfirmationDialogAsync("Dit e-mailadres wordt al gebruikt.");
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
            return !_studentRepository.EmailExists(email) || email == StudentCopy.Email;
        }

        private async Task ShowConfirmationDialogAsync(string message)
        {
            IsDialogOpen = true;

            await _dialogService.ShowDialogAsync<ConfirmationDialogViewModel, ConfirmationDialogResultType>(new ConfirmationDialogResultType
            {
                DialogTitle = "Informatie",
                DialogText = message
            });

            IsDialogOpen = false;
        }

        private async Task<bool> ShowYesNoDialogAsync(string message)
        {
            IsDialogOpen = true;

            var result = await _dialogService.ShowDialogAsync<YesNoDialogViewModel, YesNoDialogResultType>(
                new YesNoDialogResultType
                {
                    DialogTitle = "Bevestiging",
                    DialogText = message
                });

            IsDialogOpen = false;

            return result?.Data?.Result ?? false;
        }
    }
}