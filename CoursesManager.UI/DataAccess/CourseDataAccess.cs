using CoursesManager.UI.Models;
using MySql.Data.MySqlClient;
using CoursesManager.UI.Database;

namespace CoursesManager.UI.DataAccess;

public class CourseDataAccess : BaseDataAccess<Course>
{
    public List<Course> GetAll()
    {
        throw new NotImplementedException();
    }

    public void Add(Course course)
    {
        ArgumentNullException.ThrowIfNull(course);

        throw new NotImplementedException();
    }

    public void Update(Course course)
    {
        ArgumentNullException.ThrowIfNull(course);

        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        try
        {
            ExecuteNonProcedure(StoredProcedures.CoursesDeleteById, new MySqlParameter("@p_id", id));
            LogUtil.Log("Course deleted successfully.");
        }
        catch (MySqlException ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }
}