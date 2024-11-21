using System.Collections.ObjectModel;
using System.Linq;
using CoursesManager.MVVM.Data;
using CoursesManager.UI.Models;
using CoursesManager.UI.Models.Repositories.RegistrationRepository;
using CoursesManager.UI.Utils;

namespace CoursesManager.UI.ViewModels.Students
{
    public class StudentDetailViewModel : ViewModel
    {
        private readonly IRegistrationRepository _registrationRepository;
        private Student _student;

        public StudentDetailViewModel(IRegistrationRepository registrationRepository)
        {
            _registrationRepository = registrationRepository;
        }

        public Student Student
        {
            get => _student;
            set
            {
                _student = value;
                OnPropertyChanged();
                LoadStudentDetails();
            }
        }

        public ObservableCollection<CourseStudentPayment> CourseDetails { get; private set; } = new ObservableCollection<CourseStudentPayment>();

        private void LoadStudentDetails()
        {
            if (Student == null) return;

            var registrations = _registrationRepository.GetAll().Where(r => r.StudentID == Student.Id);
            CourseDetails.Clear();

            foreach (var registration in registrations)
            {
                CourseDetails.Add(new CourseStudentPayment(registration.Course, registration));
            }
        }
    }
}