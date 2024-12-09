using CoursesManager.UI.Models;

namespace CoursesManager.UI.Repositories.StudentRepository;

public interface IStudentRepository : IRepository<Student>
{
    List<Student> GetNotDeletedStudents();

    List<Student> GetDeletedStudents();
}