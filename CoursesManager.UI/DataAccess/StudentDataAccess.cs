using CoursesManager.UI.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CoursesManager.UI.DataAccess
{
    public class StudentDataAccess : BaseDataAccess<Student>
    {
        private readonly AddressDataAccess _addressDataAccess;
        private readonly RegistrationDataAccess _registrationDataAccess;

        public StudentDataAccess()
        {
            _addressDataAccess = new AddressDataAccess();
        }

        public List<Student> GetAll()
        {
            try
            {
                string query = "SELECT * FROM students";
                return FetchAll(query);
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        public List<Student> GetNotDeletedStudents()
        {
            try
            {
                string query = "SELECT * FROM students WHERE is_deleted = 0";
                return FetchAll(query);
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        public List<Student> GetDeletedStudents()
        {
            try
            {
                string query = "SELECT * FROM students WHERE is_deleted = 1";
                return FetchAll(query);
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        public void Add(Student student)
        {
            try
            {
                // Add the address first
                _addressDataAccess.Add(student.Address);

                // Get the newly created address ID
                int addressId = _addressDataAccess.GetLastInsertedId();

                string query = "INSERT INTO students (first_name, last_name, email, phone, address_id, is_deleted, deleted_at, created_at, updated_at, insertion, date_of_birth) " +
                               "VALUES (@FirstName, @LastName, @Email, @Phone, @AddressId, @IsDeleted, @DeletedAt, @CreatedAt, @UpdatedAt, @Insertion, @DateOfBirth)";

                var parameters = new Dictionary<string, object>
                {
                    { "@FirstName", student.FirstName },
                    { "@LastName", student.LastName },
                    { "@Email", student.Email },
                    { "@Phone", student.Phone },
                    { "@AddressId", addressId },
                    { "@IsDeleted", student.IsDeleted },
                    { "@DeletedAt", student.DeletedAt ?? (object)DBNull.Value },
                    { "@CreatedAt", DateTime.Now },
                    { "@UpdatedAt", DateTime.Now },
                    { "@Insertion", student.Insertion ?? (object)DBNull.Value },
                    { "@DateOfBirth", student.DateOfBirth }
                };

                ExecuteNonQuery(query, parameters);

                foreach (var registration in student.Registrations)
                {
                    registration.StudentId = GetLastInsertedId();
                    _registrationDataAccess.Add(registration);
                }
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        public Student? GetById(int id)
        {
            try
            {
                string query = "SELECT * FROM students WHERE id = @Id";
                var parameters = new MySqlParameter[] { new MySqlParameter("@Id", id) };
                var students = FetchAll(query, parameters);
                return students.FirstOrDefault();
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }



        public void Delete(Student student)
        {
            ArgumentNullException.ThrowIfNull(student);

            throw new NotImplementedException();
        }

        public void DeleteById(int id)
        {
            ArgumentNullException.ThrowIfNull(id);

            throw new NotImplementedException();
        }
        protected  Student FillModel(MySqlDataReader reader)
        {
            var student = new Student
            {
                Id = reader.GetInt32("id"),
                FirstName = reader.GetString("first_name"),
                LastName = reader.GetString("last_name"),
                Email = reader.GetString("email"),
                Phone = reader.GetString("phone"),
                IsDeleted = reader.GetBoolean("is_deleted"),
                DeletedAt = reader.IsDBNull(reader.GetOrdinal("deleted_at")) ? (DateTime?)null : reader.GetDateTime("deleted_at"),
                CreatedAt = reader.GetDateTime("created_at"),
                UpdatedAt = reader.GetDateTime("updated_at"),
                AddressId = reader.IsDBNull(reader.GetOrdinal("address_id")) ? (int?)null : reader.GetInt32("address_id"),
                DateOfBirth = reader.GetDateTime("date_of_birth"),
                Insertion = reader.IsDBNull(reader.GetOrdinal("insertion")) ? null : reader.GetString("insertion")
            };

            if (student.AddressId.HasValue)
            {
                student.Address = _addressDataAccess.FetchOneById(student.AddressId.Value);
            }

            student.Registrations = new ObservableCollection<Registration>(_registrationDataAccess.GetByStudentId(student.Id));

            return student;
        }
    }
}


