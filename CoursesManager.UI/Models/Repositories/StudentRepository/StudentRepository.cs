
using System.Collections.ObjectModel;
using CoursesManager.UI.Models.Repositories.StudentRepository;
using CoursesManager.UI.Models;

namespace CoursesManager.UI.Models.Repositories.StudentRepository
{
    public class StudentRepository : IStudentRepository
    {
        //private readonly List<Student> _students = new List<Student>();
        private ObservableCollection<Student> _students;
        
        public StudentRepository()
        {
            _students = App.Students;
        }

        public IEnumerable<Student> GetAll()
        {
            return _students.Where(s => !s.Is_deleted).ToList();
        }

        public Student GetById(int id)
        {
            return _students.FirstOrDefault(s => s.Id == id && !s.Is_deleted);
        }

        public void Add(Student student)
        {
            student.DateCreated = DateTime.Now;
            student.Id = _students.Count + 1;
            _students.Add(student);
        }

        public void Update(Student student)
        {
            var existingStudent = GetById(student.Id);
            if (existingStudent != null)
            {
                existingStudent.FirstName = student.FirstName;
                existingStudent.LastName = student.LastName;
                existingStudent.Email = student.Email;
                existingStudent.PhoneNumber = student.PhoneNumber;
                existingStudent.PostCode = student.PostCode;
                existingStudent.HouseNumber = student.HouseNumber;
                existingStudent.HouseNumberExtension = student.HouseNumberExtension;
            }
        }

        public void Delete(int id)
        {
            var student = GetById(id);
            if (student != null)
            {
                student.Is_deleted = true;
                student.date_deleted = DateTime.Now;
            }
        }

        public bool EmailExists(string email)
        {
            return _students.Any(s => s.Email == email && !s.Is_deleted);
        }
    }
}
