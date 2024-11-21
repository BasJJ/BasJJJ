using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CoursesManager.UI.Models;

namespace CoursesManager.UI.Models.Repositories.CourseRepository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ObservableCollection<Course> _courses;

        public CourseRepository(ObservableCollection<Course> courses)
        {
            _courses = courses ?? throw new ArgumentNullException(nameof(courses), "Courses collection cannot be null.");
        }

        public IEnumerable<Course> GetAll()
        {
            return _courses?.ToList() ?? new List<Course>();
        }

        public Course GetById(int id)
        {
            if (id <= 0) throw new ArgumentException("Course ID must be greater than zero.", nameof(id));

            return _courses.FirstOrDefault(c => c.ID == id);
        }

        public void Add(Course course)
        {
            if (course == null) throw new ArgumentNullException(nameof(course), "Course cannot be null.");

            course.DateCreated = DateTime.Now;
            course.ID = _courses.Any() ? _courses.Max(c => c.ID) + 1 : 1;
            _courses.Add(course);
        }

        public void Update(Course course)
        {
            if (course == null) throw new ArgumentNullException(nameof(course), "Course cannot be null.");

            var existingCourse = GetById(course.ID);
            if (existingCourse == null) throw new InvalidOperationException($"Course with ID {course.ID} does not exist.");

            existingCourse.Name = course.Name;
            existingCourse.Description = course.Description;
            existingCourse.IsActive = course.IsActive;
            existingCourse.Category = course.Category;
            existingCourse.StartDate = course.StartDate;
            existingCourse.EndDate = course.EndDate;
            existingCourse.LocationId = course.LocationId;
        }

        public void Delete(int id)
        {
            var course = GetById(id);
            if (course == null) throw new InvalidOperationException($"Course with ID {id} does not exist.");

            _courses.Remove(course);
        }
    }
}
