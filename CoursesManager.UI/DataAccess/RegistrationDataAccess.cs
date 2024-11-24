using CoursesManager.UI.Models;

namespace CoursesManager.UI.DataAccess;

public class RegistrationDataAccess(string connectionString) : BaseDataAccess<Registration>(connectionString)
{
}