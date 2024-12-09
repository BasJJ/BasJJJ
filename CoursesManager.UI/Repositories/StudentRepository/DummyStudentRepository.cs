using CoursesManager.UI.Models;
using System.Collections.ObjectModel;

namespace CoursesManager.UI.Repositories.StudentRepository
{
    public class DummyStudentRepository : IStudentRepository
    {
        private readonly ObservableCollection<Student> _students;

        public DummyStudentRepository(ObservableCollection<Student> students)
        {
            _students = students ?? throw new ArgumentNullException(nameof(students), "Students collection cannot be null.");
        }

        public List<Student> GetAll()
        {
            return _students.ToList();
        }

        public List<Student> RefreshAll()
        {
            throw new NotImplementedException();
        }

        public Student GetById(int id)
        {
            return _students.FirstOrDefault(s => s.Id == id && !s.IsDeleted);
        }

        public void Add(Student student)
        {
            if (student == null) throw new ArgumentNullException(nameof(student), "Student cannot be null.");

            student.CreatedAt = DateTime.Now;
            student.Id = _students.Any() ? _students.Max(s => s.Id) + 1 : 1;
            _students.Add(student);
        }

        public void Update(Student student)
        {
            if (student == null) throw new ArgumentNullException(nameof(student), "Student cannot be null.");

            var existingStudent = GetById(student.Id);
            if (existingStudent == null) throw new InvalidOperationException($"Student with Id {student.Id} does not exist.");

            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.Email = student.Email;
            existingStudent.Phone = student.Phone;
            existingStudent.Address.ZipCode = student.Address.ZipCode;
            existingStudent.Address.HouseNumber = student.Address.HouseNumber;
            existingStudent.Address.HouseNumberExtension = student.Address.HouseNumberExtension;
            existingStudent.IsDeleted = student.IsDeleted;

        }

        public void Delete(Student data)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            var student = GetById(id);
            if (student == null) throw new InvalidOperationException($"Student with Id {id} does not exist.");

            student.IsDeleted = true;
            student.DeletedAt = DateTime.Now;
        }

        public bool EmailExists(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            return _students.Any(s => s.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && !s.IsDeleted);
        }

        public List<Student> GetNotDeletedStudents()
        {
            return _students.Where(s => !s.IsDeleted).ToList();
        }

        public List<Student> GetDeletedStudents()
        {
            return _students.Where(s => s.IsDeleted).ToList();
        }
    }
}