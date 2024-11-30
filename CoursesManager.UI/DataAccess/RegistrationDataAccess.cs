using CoursesManager.UI.Models;
using MySql.Data.MySqlClient;
using System.Reflection;

namespace CoursesManager.UI.DataAccess;

public class RegistrationDataAccess : BaseDataAccess<Registration>
{
    private const string ProcedureRegistrationGetByCourseId = "spRegistrations_GetByCourseId";

    public List<Registration> GetAllRegistrationsByCourse(int courseId)
    {
        var procRes = ExecuteProcedure(ProcedureRegistrationGetByCourseId, new MySqlParameter("@p_courseId", courseId));
        
        return procRes.Select(row => new Registration
        {
            ID = Convert.ToInt32(row["id"]),
            CourseID = Convert.ToInt32(row["course_id"]),
            StudentID = Convert.ToInt32(row["student_id"]),
            RegistrationDate = Convert.ToDateTime(row["registration_date"]),
            PaymentStatus = Convert.ToBoolean(row["payment_status"]),
            IsAchieved = Convert.ToBoolean(row["is_achieved"]),
            IsActive = Convert.ToBoolean(row["is_active"])
        }).ToList();
    }
}