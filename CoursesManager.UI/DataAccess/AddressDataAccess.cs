using CoursesManager.UI.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace CoursesManager.UI.DataAccess
{
    public class AddressDataAccess : BaseDataAccess<Address>
    {
        
        public List<Address> GetAll()
        {
            string query = "SELECT * FROM addresses";
            return FetchAll(query);
        }

        public Address? GetById(int id)
        {
            try
            {
                string query = "SELECT * FROM addresses WHERE id = @Id";
                var parameters = new MySqlParameter[] { new MySqlParameter("@Id", id) };
                var addresses = FetchAll(query, parameters);
                return addresses.FirstOrDefault();
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        public void Add(Address address)
        {
            try
            {
                string query = "INSERT INTO addresses (country, zipcode, city, street, house_number, house_number_extension, created_at, updated_at) " +
                               "VALUES (@Country, @ZipCode, @City, @Street, @HouseNumber, @HouseNumberExtension, @CreatedAt, @UpdatedAt)";

                var parameters = new Dictionary<string, object>
                {
                    { "@Country", address.Country },
                    { "@ZipCode", address.ZipCode },
                    { "@City", address.City },
                    { "@Street", address.Street },
                    { "@HouseNumber", address.HouseNumber },
                    { "@HouseNumberExtension", address.HouseNumberExtension ?? (object)DBNull.Value },
                    { "@CreatedAt", DateTime.Now },
                    { "@UpdatedAt", DateTime.Now }
                };

                ExecuteNonQuery(query, parameters);
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        public int GetLastInsertedId()
        {
            try
            {
                string query = "SELECT LAST_INSERT_ID()";
                using var connection = GetConnection();
                using var command = new MySqlCommand(query, connection);
                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        public void Update(Address address)
        {
            try
            {
                string query = "UPDATE addresses SET country = @Country, zipcode = @ZipCode, city = @City, street = @Street, " +
                               "house_number = @HouseNumber, house_number_extension = @HouseNumberExtension, updated_at = @UpdatedAt WHERE id = @Id";

                var parameters = new Dictionary<string, object>
                {
                    { "@Country", address.Country },
                    { "@ZipCode", address.ZipCode },
                    { "@City", address.City },
                    { "@Street", address.Street },
                    { "@HouseNumber", address.HouseNumber },
                    { "@HouseNumberExtension", address.HouseNumberExtension ?? (object)DBNull.Value },
                    { "@UpdatedAt", DateTime.Now },
                    { "@Id", address.Id }
                };

                ExecuteNonQuery(query, parameters);
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                string query = "DELETE FROM addresses WHERE id = @Id";
                var parameters = new MySqlParameter[] { new MySqlParameter("@Id", id) };
                ExecuteNonQuery(query, parameters);
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        protected Address FillModel(MySqlDataReader reader)
        {
            return new Address
            {
                Id = reader.GetInt32("id"),
                Country = reader.GetString("country"),
                ZipCode = reader.GetString("zipcode"),
                City = reader.GetString("city"),
                Street = reader.GetString("street"),
                HouseNumber = reader.GetString("house_number"),
                HouseNumberExtension = reader.IsDBNull(reader.GetOrdinal("house_number_extension")) ? null : reader.GetString("house_number_extension"),
                CreatedAt = reader.GetDateTime("created_at"),
                UpdatedAt = reader.GetDateTime("updated_at")
            };
        }
    }
}