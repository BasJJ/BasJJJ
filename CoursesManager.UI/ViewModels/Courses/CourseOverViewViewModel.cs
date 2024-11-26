using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.UI.Utils;
using CoursesManager.UI.Models;
using System;
using System.Collections.Generic;
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
        private readonly IStudentRepository _studentRepository;
        private readonly IRegistrationRepository _registrationRepository;

        public Course CurrentCourse { get; private set; }
        public ObservableCollection<Student> Students { get; private set; }
        public ObservableCollection<CourseStudentPayment> StudentPayments { get; private set; }

        public CourseOverViewViewModel(
            IStudentRepository studentRepository,
            IRegistrationRepository registrationRepository)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _registrationRepository = registrationRepository ?? throw new ArgumentNullException(nameof(registrationRepository));
            ChangeCourseCommand = new RelayCommand(ChangeCourse);
            DeleteCourseCommand = new RelayCommand(DeleteCourse);

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

            StudentPayments = new ObservableCollection<CourseStudentPayment>();

            foreach (var registration in registrations)
            {
                var student = _studentRepository.GetById(registration.StudentID);
                if (student != null)
                {
                    var studentPayment = new CourseStudentPayment(student, registration);
                    StudentPayments.Add(studentPayment);
                }
                else
                {
                    Console.WriteLine($"Warning: Student with ID {registration.StudentID} not found for registration ID {registration.ID}.");
                }
            }

            OnPropertyChanged(nameof(Students));
            OnPropertyChanged(nameof(StudentPayments));
        }
        private void ChangeCourse()
        {

        }

        private void DeleteCourse()
        {

        }
    }
}
