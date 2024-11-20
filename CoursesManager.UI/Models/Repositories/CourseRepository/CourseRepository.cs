using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Env;
using MySql.Data.MySqlClient;
using CoursesManager.UI.Models;

namespace CoursesManager.UI.Models.Repositories.CourseRepository
{
    public class CourseRepository : BaseRepository, ICourseRepository
    {
        public CourseRepository() : base("courses", EnvManager<EnvModel>.Values.ConnectionString) {}

        public IEnumerable<Course> GetAll()
        {
            return FetchAll<Course>(
                "SELECT c.*, COUNT(r.id) AS participants, " +
                "CASE WHEN COUNT(r.id) = COUNT(CASE WHEN r.payment_status = 1 THEN 1 END) " +
                "THEN 'TRUE' ELSE 'FALSE' END AS isPayed " +
                "FROM courses c " +
                "LEFT JOIN registrations r ON r.course_id = c.id;"
            );
        }

        public Course GetById(int id) 
        {
            if (id <= 0) throw new ArgumentException("Course ID must be greater than zero.", nameof(id));
            FetchOneByID<Course>(id);
        }
        
        public void Add(Course course)
        {
            InsertRow(
                new Dictionary<string, object> {
                    { "Name", course.Name },
                    { "Description", course.Description },
                    { "IsActive", course.IsActive },
                    { "Category", course.Category },
                    { "StartDate", course.StartDate },
                    { "EndDate", course.EndDate },
                    { "LocationId", course.LocationId },
                    { "DateCreated", DateTime.Now },
                }
            );
        }

        public void Update(Course course)
        {
            if (course == null) throw new ArgumentNullException(nameof(course), "Course cannot be null.");
            
            UpdateRow(
                new Dictionary<string, object> {
                    { "Name", course.Name },
                    { "Description", course.Description },
                    { "IsActive", course.IsActive },
                    { "Category", course.Category },
                    { "StartDate", course.StartDate },
                    { "EndDate", course.EndDate },
                    { "LocationId", course.LocationId },
                    { "DateUpdated", DateTime.Now },
                },
                "ID = @ID",
                [new MySqlParameter("@ID", course.ID)]
            );
        }

        public void Delete(int id)
        {
            if (course == null) throw new InvalidOperationException($"Course with ID {id} does not exist.");
            DeleteRow("ID = @ID", [new MySqlParameter("@ID", id)]);
        }
    }
}