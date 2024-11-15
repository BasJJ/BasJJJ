using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesManager.UI.Models.Repositories.CourseRepository
{
    public interface ICourseRepository
    {
        IEnumerable<Course> GetAll();

        Course GetById(int id);

        void Add(Course course);

        void Update(Course course);

        void Delete(int id);
    }
}