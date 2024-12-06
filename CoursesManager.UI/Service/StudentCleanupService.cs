using CoursesManager.UI.Repositories.StudentRepository;
using System;

namespace CoursesManager.UI.Services
{
    public class StudentCleanupService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentCleanupService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public void CleanupDeletedStudents()
        {
            var students = _studentRepository.GetDeletedStudents();
            var thresholdDate = DateTime.Now.AddYears(-2);

            foreach (var student in students)
            {
                if (student.DeletedAt.HasValue && student.DeletedAt.Value < thresholdDate)
                {
                    _studentRepository.Delete(student.Id);
                }
            }
        }
    }
}