using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.UI.Utils;
using CoursesManager.UI.Models;
using CoursesManager.UI.Models.CoursesManager.UI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CoursesManager.MVVM.Messages;
using CoursesManager.MVVM.Navigation;
using CoursesManager.UI.Dialogs.ResultTypes;
using CoursesManager.UI.Dialogs.ViewModels;
using CoursesManager.UI.Messages;
using CoursesManager.UI.Models.Repositories.CourseRepository;
using CoursesManager.UI.ViewModels.Students;

namespace CoursesManager.UI.ViewModels.Courses
{
    internal class CourseOverViewViewModel : NavigatableViewModel
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IDialogService _dialogService;
        private readonly IMessageBroker _messageBroker;

        public ICommand ChangeCourseCommand { get; set; }
        public ICommand DeleteCourseCommand { get; set; }

        public Course CurrentCourse { get; set; }
        public ObservableCollection<Student>? Students { get; set; }

        public ObservableCollection<Course>? Courses { get; set; }
        public ObservableCollection<CourseStudentPayment>? studentPayments { get; set; }

        public CourseOverViewViewModel(ICourseRepository courseRepository, IDialogService dialogService, IMessageBroker messageBroker, INavigationService navigationService) : base(navigationService)
        {
            _courseRepository = courseRepository;
            _dialogService = dialogService;
            _messageBroker = messageBroker;

            ChangeCourseCommand = new RelayCommand(ChangeCourse);
            DeleteCourseCommand = new RelayCommand(DeleteCourse);
            CurrentCourse = (Course)GlobalCache.Instance.Get("Opened Course");
            Students = CurrentCourse.students;
            ObservableCollection<Registration> registration = DummyDataGenerator.GenerateRegistrations(Students.Count, 1);
            studentPayments = new ObservableCollection<CourseStudentPayment>();

            for (int i = 0; i < registration.Count - 1; i++)
            {
                CourseStudentPayment studentPayment = new CourseStudentPayment(Students[i], registration[i]);
                studentPayments.Add(studentPayment);
            }
        }

        public void LoadCourses()
        {
            Courses = new ObservableCollection<Course>(_courseRepository.GetAll());
        }


        private void DeleteCourse()
        {

        }

        private async void ChangeCourse()
        {

            await ExecuteWithOverlayAsync(async () =>
            {
                var dialogResult = await _dialogService.ShowDialogAsync<CourseDialogViewModel, Course>(CurrentCourse);

                if (dialogResult?.Outcome == DialogOutcome.Success)
                {
                    // Herlaad de lijst of voer andere acties uit
                    LoadCourses();
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
    }
}