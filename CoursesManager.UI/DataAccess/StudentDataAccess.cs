using CoursesManager.UI.Models;

namespace CoursesManager.UI.DataAccess;

public class StudentDataAccess(string connectionString) : BaseDataAccess<Student>(connectionString)
{
}