using CoursesManager.UI.Models;

namespace CoursesManager.UI.Repositories.StudentRepository;

public interface IStudentRepository : IRepository<Student>
{
    // I don't wanna ruin other view models in the app so i made this temporary 
    List<Student> GetAllStudents();
}