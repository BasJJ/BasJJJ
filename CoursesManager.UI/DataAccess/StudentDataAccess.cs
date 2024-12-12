using CoursesManager.UI.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CoursesManager.UI.Database;

namespace CoursesManager.UI.DataAccess
{
    public class StudentDataAccess : BaseDataAccess<Student>
    {
        private readonly AddressDataAccess _addressDataAccess;
        private readonly RegistrationDataAccess _registrationDataAccess;

        public StudentDataAccess()
        {
            _addressDataAccess = new AddressDataAccess();
            _registrationDataAccess = new RegistrationDataAccess();
        }

        public List<Student> GetAll()
        {
            try
            {
                string procedureName = StoredProcedures.GetAllStudents;
                return FetchAll(procedureName);
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
                string procedureName = StoredProcedures.GetNotDeletedStudents;
                return FetchAll(procedureName);
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
                string procedureName = StoredProcedures.GetDeletedStudents;
                return FetchAll(procedureName);
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

                string procedureName = StoredProcedures.AddStudent;
                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@p_first_name", student.FirstName),
                    new MySqlParameter("@p_last_name", student.LastName),
                    new MySqlParameter("@p_email", student.Email),
                    new MySqlParameter("@p_phone", student.Phone),
                    new MySqlParameter("@p_address_id", addressId),
                    new MySqlParameter("@p_is_deleted", student.IsDeleted),
                    new MySqlParameter("@p_deleted_at", student.DeletedAt ?? (object)DBNull.Value),
                    new MySqlParameter("@p_created_at", DateTime.Now),
                    new MySqlParameter("@p_updated_at", DateTime.Now),
                    new MySqlParameter("@p_insertion", student.Insertion ?? (object)DBNull.Value),
                    new MySqlParameter("@p_date_of_birth", student.DateOfBirth)
                };

                ExecuteNonProcedure(procedureName, parameters);

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
                string procedureName = StoredProcedures.GetStudentById;
                var parameters = new MySqlParameter[] { new MySqlParameter("@p_id", id) };
                var students = FetchAll(procedureName, parameters);
                return students.FirstOrDefault();
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        public void Update(Student student)
        {
            try
            {
                // Update the address first
                _addressDataAccess.Update(student.Address);
                
                int? addressId = student.AddressId;

                ExecuteNonProcedure(
                    StoredProcedures.EditStudent,
                    new MySqlParameter("@p_id", student.Id),
                    new MySqlParameter("@p_first_name", student.FirstName),
                    new MySqlParameter("@p_last_name", student.LastName),
                    new MySqlParameter("@p_email", student.Email),
                    new MySqlParameter("@p_phone", student.Phone),
                    new MySqlParameter("@p_address_id", addressId),
                    new MySqlParameter("@p_is_deleted", student.IsDeleted),
                    new MySqlParameter("@p_deleted_at", student.DeletedAt ?? (object)DBNull.Value),
                    new MySqlParameter("@p_created_at", DateTime.Now),
                    new MySqlParameter("@p_updated_at", DateTime.Now),
                    new MySqlParameter("@p_insertion", student.Insertion ?? (object)DBNull.Value),
                    new MySqlParameter("@p_date_of_birth", student.DateOfBirth)
                );

                LogUtil.Log($"Student: {student.Id} updated successfully");
            }
            catch (MySqlException ex)
            {
                LogUtil.Error($"MySQL error in Update: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                LogUtil.Error($"General error in Update: {ex.Message}");
                throw;
            }
        }

        public void DeleteById(int id)
        {
            try
            {
                ExecuteNonProcedure(StoredProcedures.DeleteStudent, new MySqlParameter("@p_id", id));
                LogUtil.Log($"Student ID: {id} deleted successfully.");
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                LogUtil.Error($"General error in Update: {ex.Message}");
                throw;
            }
        }

        protected Student FillModel(MySqlDataReader reader)
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
                student.Address = _addressDataAccess.GetById(student.AddressId.Value);
            }

            student.Registrations = new ObservableCollection<Registration>(_registrationDataAccess.GetByStudentId(student.Id));

            return student;
        }
    }
}