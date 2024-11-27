using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.MVVM.Messages;
using CoursesManager.MVVM.Navigation;
using CoursesManager.UI.Messages;
using CoursesManager.UI.Models;
using CoursesManager.UI.Models.CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.Utils;
using CoursesManager.UI.ViewModels.Students;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CoursesManager.UI.ViewModels.Courses
{
    internal class CourseOverViewViewModel : NavigatableViewModel
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IDialogService _dialogService;
        private readonly IMessageBroker _messageBroker;

        public ICommand ChangeCourseCommand { get; set; }
        public ICommand DeleteCourseCommand { get; set; }

        private bool _isDialogOpen;
        private Course? _currentCourse;

        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set => SetProperty(ref _isDialogOpen, value);
        }

        public Course CurrentCourse { get => _currentCourse!; set => SetProperty(ref _currentCourse, value); }
        public ObservableCollection<Student>? Students { get; set; }

        public ObservableCollection<Course>? Courses { get; set; }
        public ObservableCollection<CourseStudentPayment>? StudentPayments { get; set; }

        public CourseOverViewViewModel(ICourseRepository courseRepository, IDialogService dialogService, IMessageBroker messageBroker, INavigationService navigationService) : base(navigationService)
        {
            _courseRepository = courseRepository;
            _dialogService = dialogService;
            _messageBroker = messageBroker;

            ChangeCourseCommand = new RelayCommand(ChangeCourse);
            DeleteCourseCommand = new RelayCommand(DeleteCourse);
            CurrentCourse = (Course)GlobalCache.Instance.Get("Opened Course");
            Students = CurrentCourse.Students;

            if (Students is not null)
            {
                ObservableCollection<Registration> registration = DummyDataGenerator.GenerateRegistrations(Students.Count, 1);
                StudentPayments = new ObservableCollection<CourseStudentPayment>();

                for (int i = 0; i < registration.Count - 1; i++)
                {
                    CourseStudentPayment studentPayment = new CourseStudentPayment(Students[i], registration[i]);
                    StudentPayments.Add(studentPayment);
                }
            }
        }


        private void DeleteCourse()
        {

        }

        private async void ChangeCourse()
        {

            await ExecuteWithOverlayAsync(async () =>
            {
                var dialogResult = await _dialogService.ShowDialogAsync<CourseDialogViewModel, Course>(CurrentCourse);

                if (dialogResult.Outcome == DialogOutcome.Success)
                {
                    CurrentCourse = dialogResult.Data;
                    
                    
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

        private System.Windows.Visibility isImageVisible;

        public System.Windows.Visibility IsImageVisible { get => isImageVisible; set => SetProperty(ref isImageVisible, value); }

        private System.Windows.Visibility isPlaceholderVisible;

        public System.Windows.Visibility IsPlaceholderVisible { get => isPlaceholderVisible; set => SetProperty(ref isPlaceholderVisible, value); }
    }
}