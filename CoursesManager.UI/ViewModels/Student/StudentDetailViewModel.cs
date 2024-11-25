using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.MVVM.Navigation;
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
        private Student _student;
        private ObservableCollection<CourseStudentPayment> _courseDetails = new ObservableCollection<CourseStudentPayment>();

        public ObservableCollection<CourseStudentPayment> CourseDetails
        {
            get => _courseDetails;
            private set
            {
                _courseDetails = value;
                OnPropertyChanged(nameof(CourseDetails));
            }
        }
        public StudentDetailViewModel(IRegistrationRepository registrationRepository,INavigationService navigationService, Student student) : base(navigationService)
        {
            _registrationRepository = registrationRepository;
            Student = student;
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

    }

}