using CoursesManager.UI.Models;
using MySql.Data.MySqlClient;
using System.Reflection;
using CoursesManager.UI.Database;

namespace CoursesManager.UI.DataAccess;

public class RegistrationDataAccess : BaseDataAccess<Registration>
{
    public List<Registration> GetAllRegistrationsByCourse(int courseId)
    {
        try
        {
            return ExecuteProcedure(StoredProcedures.RegistrationsGetByCourseId, new MySqlParameter("@p_courseId", courseId)).Select(row => new Registration
            {
                Id = Convert.ToInt32(row["id"]),
                CourseId = Convert.ToInt32(row["course_id"]),
                StudentId = Convert.ToInt32(row["student_id"]),
                RegistrationDate = Convert.ToDateTime(row["registration_date"]),
                PaymentStatus = Convert.ToBoolean(row["payment_status"]),
                IsAchieved = Convert.ToBoolean(row["is_achieved"]),
                IsActive = Convert.ToBoolean(row["is_active"])
            }).ToList();
        }
        catch (MySqlException ex)
        {
            throw new InvalidOperationException(ex.Message, ex);
        }
    }

    public List<Registration> GetAll()
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public void Add(Registration data)
    {
        throw new NotImplementedException();
    }

    public void Update(Registration data)
    {
        throw new NotImplementedException();
    }

    public List<Registration> GetByStudentId(int studentId)
    {
        try
        {
            string query = "SELECT * FROM registrations WHERE student_id = @StudentId";
            var parameters = new MySqlParameter[] { new MySqlParameter("@StudentId", studentId) };
            return FetchAll(query, parameters);
        }
        catch (MySqlException ex)
        {
            throw new InvalidOperationException(ex.Message, ex);
        }
    }
}