using CoursesManager.MVVM.Data;

namespace CoursesManager.UI.Models
{
    public class CourseStudentPayment : IsObservable
    {
        private bool _isPaid;
        private bool _isAchieved;
        public Student? Student { get; set; }
        public Course? Course { get; set; }
        public bool IsPaid
        {
            get => _isPaid;
            set
            {
                if (value == false && IsAchieved)
                {
                    return;
                }
                SetProperty(ref _isPaid, value);
            }
        }
        public bool IsAchieved
        {
            get => _isAchieved;
            set => SetProperty(ref _isAchieved, value);
        }
        public string? FullName { get; set; }

        public CourseStudentPayment(Student student, Registration registration)
        {
            Student = student;
            IsPaid = registration.PaymentStatus;
            FullName = $"{student.FirstName} {student.Insertion} {student.LastName}";
            IsAchieved = registration.IsAchieved;
        }

        public CourseStudentPayment(Course course, Registration registration)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course), "Course cannot be null");
            }
            FullName = course.Name;
            Course = course;
            IsPaid = registration.PaymentStatus;
            IsAchieved = registration.IsAchieved;
        }
    }
}
