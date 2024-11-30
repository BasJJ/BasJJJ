using CoursesManager.UI.Models;
using MySql.Data.MySqlClient;
using System.Reflection;
using CoursesManager.UI.Database;

namespace CoursesManager.UI.DataAccess;

public class RegistrationDataAccess : BaseDataAccess<Registration>
{
    public List<Registration> GetAllRegistrationsByCourse(int courseId)
    {
        var procRes = ExecuteProcedure(StoredProcedures.RegistrationsGetByCourseId, new MySqlParameter("@p_courseId", courseId));
        
        return procRes.Select(row => new Registration
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
}