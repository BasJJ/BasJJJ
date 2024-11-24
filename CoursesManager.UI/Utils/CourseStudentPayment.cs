using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoursesManager.UI.Models;

namespace CoursesManager.UI.Utils
{
    public class CourseStudentPayment
    {
        public Student? Student { get; set; }
        public Course? Course { get; set; }
        public bool IsPaid { get; set; }
        public bool IsAchieved { get; set; }
        public string? FullName { get; set; }

        public CourseStudentPayment(Student student, Registration registration)
        {
            Student = student;
            IsPaid = registration.PaymentStatus;
            FullName = $"{student.FirstName} {student.Insertion} {student.LastName}";
            IsAchieved = registration.Is_Achieved;
        }

        public CourseStudentPayment(Course course, Registration registration)
        {
            FullName = course.Name;
            Course = course;
            IsPaid = registration.PaymentStatus;
            IsAchieved = registration.Is_Achieved;
        }
    }
}
