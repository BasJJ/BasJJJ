using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.UI.Utils;
using CoursesManager.UI.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CoursesManager.UI.Repositories.RegistrationRepository;
using CoursesManager.UI.Repositories.StudentRepository;

namespace CoursesManager.UI.ViewModels.Courses
{
    class CourseOverViewViewModel : ViewModel
    {
        public ICommand ChangeCourseCommand { get; set; }
        public ICommand DeleteCourseCommand { get; set; }
        public ICommand CheckboxChangedCommand { get; }


        private readonly IStudentRepository _studentRepository;
        private readonly IRegistrationRepository _registrationRepository;

        private Course _currentCourse;
        public Course CurrentCourse
        {
            get => _currentCourse;
            private set => SetProperty(ref _currentCourse, value);
        }

        private ObservableCollection<Student> _students;
        public ObservableCollection<Student> Students
        {
            get => _students;
            private set => SetProperty(ref _students, value);
        }

        private ObservableCollection<CourseStudentPayment> _studentPayments;
        public ObservableCollection<CourseStudentPayment> StudentPayments
        {
            get => _studentPayments;
            private set => SetProperty(ref _studentPayments, value);
        }

        public CourseOverViewViewModel(
            IStudentRepository studentRepository,
            IRegistrationRepository registrationRepository)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _registrationRepository = registrationRepository ?? throw new ArgumentNullException(nameof(registrationRepository));

            ChangeCourseCommand = new RelayCommand(ChangeCourse);
            DeleteCourseCommand = new RelayCommand(DeleteCourse);
            CheckboxChangedCommand = new RelayCommand<CourseStudentPayment>(OnCheckboxChanged);


            LoadCourseData();
        }

        private void LoadCourseData()
        {
            CurrentCourse = (Course)GlobalCache.Instance.Get("Opened Course");

            if (CurrentCourse == null)
            {
                throw new InvalidOperationException("No course is currently opened. Ensure the course is loaded in the GlobalCache.");
            }

            Students = new ObservableCollection<Student>(
                _studentRepository.GetAll()
                    .Where(s => s.Courses != null && s.Courses.Any(c => c != null && c.ID == CurrentCourse.ID))
            );

            var registrations = _registrationRepository.GetAll()
                .Where(r => r.CourseID == CurrentCourse.ID)
                .ToList();

            var payments = registrations.Select(registration =>
            {
                var student = _studentRepository.GetById(registration.StudentID);
                if (student == null)
                {
                    Console.WriteLine($"Warning: Student with ID {registration.StudentID} not found for registration ID {registration.ID}.");
                    return null;
                }
                return new CourseStudentPayment(student, registration);
            }).Where(payment => payment != null);

            StudentPayments = new ObservableCollection<CourseStudentPayment>(payments);
        }

        private void OnCheckboxChanged(CourseStudentPayment payment)
        {
            if (payment == null || CurrentCourse == null) return;

            var existingRegistration = _registrationRepository.GetAll()
                .FirstOrDefault(r => r.CourseID == CurrentCourse.ID && r.StudentID == payment.Student?.Id);

            if (existingRegistration != null)
            {
                existingRegistration.PaymentStatus = payment.IsPaid;
                existingRegistration.IsAchieved = payment.IsAchieved;
                _registrationRepository.Update(existingRegistration);
            }
            else if (payment.IsPaid || payment.IsAchieved)
            {
                _registrationRepository.Add(new Registration
                {
                    StudentID = payment.Student?.Id ?? 0,
                    CourseID = CurrentCourse.ID,
                    PaymentStatus = payment.IsPaid,
                    IsAchieved = payment.IsAchieved,
                    RegistrationDate = DateTime.Now,
                    IsActive = true
                });
            }
            LoadCourseData();
        }

        private void ChangeCourse()
        {
        }

        private void DeleteCourse()
        {
        }
    }
}
