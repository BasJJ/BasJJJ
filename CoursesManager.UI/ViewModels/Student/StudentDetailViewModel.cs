using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.MVVM.Messages;
using CoursesManager.MVVM.Navigation;
using CoursesManager.UI.Dialogs.ResultTypes;
using CoursesManager.UI.Dialogs.ViewModels;
using CoursesManager.UI.Messages;
using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.Models.Repositories.RegistrationRepository;
using CoursesManager.UI.Models.Repositories.StudentRepository;
using CoursesManager.UI.Utils;

namespace CoursesManager.UI.ViewModels.Students
{
    public class StudentDetailViewModel : NavigatableViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IMessageBroker _messageBroker;
        private readonly IRegistrationRepository _registrationRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly INavigationService _navigationService;
        private Student _student;
        public Student StudentDetail { get; set; }
        public Course Course { get; set; }
        private bool _isStartAnimationTriggered;
        private bool _isEndAnimationTriggered;

        public UserControl ParentWindow { get; set; }
        public ICommand EditStudentCommand { get; }



        public StudentDetailViewModel(
            IDialogService dialogService,
            IMessageBroker messageBroker,
            INavigationService navigationService,
            IRegistrationRepository registrationRepository,
            ICourseRepository courseRepository,
            IStudentRepository studentRepository,
            Student student): base(navigationService)
        {
            _messageBroker = messageBroker;
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _navigationService = navigationService;
            _registrationRepository = registrationRepository;
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            Student = student;
            ParentWindow = null;
            StudentDetail = student.Copy();
            ViewTitle = "Student Details";

            EditStudentCommand = new RelayCommand<Student>(OpenEditStudentPopup, s => s != null);

        }

        public Student Student
        {
            get => _student;
            set
            {
                if (_student != value)
                {
                    _student = value;
                    OnPropertyChanged(nameof(Student));
                    LoadStudentDetails();
                }
            }
        }

        public ObservableCollection<CourseRegistrationInfo> FilteredCourseRecords { get; private set; } = new ObservableCollection<CourseRegistrationInfo>();

        private void LoadStudentDetails()
        {
            if (Student == null) return;

            var registrations = _registrationRepository.GetAll().Where(r => r.StudentID == Student.Id);

            FilteredCourseRecords.Clear();
            foreach (var registration in registrations)
            {
                var course = _courseRepository.GetById(registration.CourseID);
                if (course != null)
                {
                    FilteredCourseRecords.Add(new CourseRegistrationInfo
                    {
                        CourseName = course.Name,
                        PaymentStatus = registration.PaymentStatus,
                        IsAchieved = registration.IsAchieved
                    });
                }
            }

            StudentDetail = Student.Copy();
            OnPropertyChanged(nameof(FilteredCourseRecords));
        }
        private async void OpenEditStudentPopup(Student student)
        {
            if (student == null)
                await ExecuteWithOverlayAsync(async () =>
                {
                    {
                        await _dialogService.ShowDialogAsync<NotifyDialogViewModel, DialogResultType>(
                            new DialogResultType
                            {
                                DialogTitle = "Error",
                                DialogText = "Geen student geselecteerd om te bewerken."
                            });
                        return;
                    }
                });

            if (student == null) return;
            await ExecuteWithOverlayAsync(async () =>
            {
                var dialogResult = await _dialogService.ShowDialogAsync<EditStudentViewModel, Student>(student);

                if (dialogResult?.Outcome == DialogOutcome.Success)
                {
                    // Refresh the list or perform other actions
                    LoadStudentDetails();
                }
            });
        }

        private async Task ExecuteWithOverlayAsync(Func<Task> action)
        {
            _messageBroker.Publish(new OverlayActivationMessage(true));
            try
            {
                await action();
            }
            finally
            {
                _messageBroker.Publish(new OverlayActivationMessage(false));
            }
        }    }

}

public class CourseRegistrationInfo
{
    public string CourseName { get; set; }
    public bool PaymentStatus { get; set; }
    public bool IsAchieved { get; set; }
}