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
        return new List<Registration>();
    }

    public void Delete(int id)
    {

    }

    public void Add(Registration data)
    {

    }

    public void Update(Registration data)
    {

    }

    public List<Registration> GetByStudentId(int studentId)
    {
        try
        {
            var result = ExecuteProcedure("GetRegistrationsByStudentId", new MySqlParameter[]
            {
                new MySqlParameter("@p_student_id", studentId)
            });

            return result.Select(row => new Registration
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
}