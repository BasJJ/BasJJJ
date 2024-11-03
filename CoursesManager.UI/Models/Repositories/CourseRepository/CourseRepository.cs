
using System.Collections.ObjectModel;
using CoursesManager.UI.Models;

namespace CoursesManager.UI.Models.Repositories.CourseRepository
{
    public class CourseRepository : ICourseRepository
    {
        //private readonly List<Course> _courses = new List<Course>();
        private ObservableCollection<Course> _courses;

        public CourseRepository()
        {
            _courses = App.Courses;
        }

        public IEnumerable<Course> GetAll()
        {
            return _courses.ToList();
        }

        public Course GetById(int id)
        {
            // Method Syntax
            return _courses.FirstOrDefault(c => c.ID == id);
        }

        public void Add(Course course)
        {
            course.DateCreated = DateTime.Now;
            _courses.Add(course);
        }

        public void Update(Course course)
        {
            var existingCourse = GetById(course.ID);
            if (existingCourse != null)
            {
                existingCourse.CourseName = course.CourseName;
                existingCourse.Description = course.Description;
                existingCourse.IsActive = course.IsActive;
                existingCourse.Category = course.Category;
                existingCourse.StartDate = course.StartDate;
                existingCourse.EndDate = course.EndDate;
                existingCourse.LocationId = course.LocationId;
            }
        }

        public void Delete(int id)
        {
            var course = GetById(id);
            if (course != null)
            {
                _courses.Remove(course);
            }
        }
    }
}
