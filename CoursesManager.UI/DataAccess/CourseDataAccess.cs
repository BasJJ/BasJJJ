using CoursesManager.UI.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace CoursesManager.UI.DataAccess;

public class CourseDataAccess : BaseDataAccess<Course>
{
    private const string ProcedureCourseDeleteById = "spCourse_DeleteById";

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
            ExecuteNonProcedure(ProcedureCourseDeleteById, new MySqlParameter("@p_id", id));
            LogUtil.Log("Course deleted successfully.");
        }
        catch (MySqlException ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }
}