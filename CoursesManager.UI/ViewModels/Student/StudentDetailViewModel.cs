using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
using CoursesManager.UI.Utils;

namespace CoursesManager.UI.ViewModels.Students
{
    public class StudentDetailViewModel : NavigatableViewModel
    {
        private readonly IRegistrationRepository _registrationRepository;
        private readonly INavigationService _navigation;
        private readonly ICourseRepository _courseRepository;
        private readonly IDialogService _dialogService;
        private readonly IMessageBroker _messageBroker;
        public event PropertyChangedEventHandler PropertyChanged;
        private Student _student;
        private ObservableCollection<CourseStudentPayment> _courseDetails = new ObservableCollection<CourseStudentPayment>();
        public ICommand EditStudent { get; set; }

        public ObservableCollection<CourseStudentPayment> CourseDetails
        {
            get => _courseDetails;
            private set
            {
                _courseDetails = value;
                OnPropertyChanged(nameof(CourseDetails));
            }
        }
        public StudentDetailViewModel(
            IDialogService dialogService,
            IMessageBroker messageBroker,
            IRegistrationRepository registrationRepository,
            INavigationService navigationService,
            Student student) : base(navigationService)
        {
            _dialogService = dialogService;
            _messageBroker = messageBroker;
            _registrationRepository = registrationRepository;
            Student = student;
            EditStudent = new RelayCommand<Student>(OpenEditStudentPopup, s => s != null);
            LoadStudentDetails();
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
                }
            }
        }

        private void LoadStudentDetails()
        {
            if (Student == null)
            {
                Debug.WriteLine("Student is null in LoadStudentDetails.");
                return;
            }

            var registrations = _registrationRepository.GetAll().Where(r => r.StudentID == Student.Id);
            CourseDetails.Clear();

            foreach (var registration in registrations)
            {
                if (registration.Course == null)
                {
                    Debug.WriteLine($"Registration with ID {registration.ID} has no associated course.");
                    continue;
                }

                CourseDetails.Add(new CourseStudentPayment(registration.Course, registration));
            }

            Debug.WriteLine($"CourseDetails populated with {CourseDetails.Count} items.");
            OnPropertyChanged(nameof(CourseDetails));
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
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}