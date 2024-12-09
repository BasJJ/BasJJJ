using CoursesManager.UI.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using CoursesManager.UI.Database;

namespace CoursesManager.UI.DataAccess
{
    public class AddressDataAccess : BaseDataAccess<Address>
    {
        public List<Address> GetAll()
        {
            string procedureName = StoredProcedures.GetAllAddresses;
            return FetchAll(procedureName);
        }

        public Address? GetById(int id)
        {
            try
            {
                string procedureName = StoredProcedures.GetAddressById;
                var parameters = new MySqlParameter[] { new MySqlParameter("@p_id", id) };
                var addresses = FetchAll(procedureName, parameters);
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
                string procedureName = StoredProcedures.AddAddress;
                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@p_country", address.Country),
                    new MySqlParameter("@p_zipcode", address.ZipCode),
                    new MySqlParameter("@p_city", address.City),
                    new MySqlParameter("@p_street", address.Street),
                    new MySqlParameter("@p_house_number", address.HouseNumber),
                    new MySqlParameter("@p_house_number_extension", address.HouseNumberExtension ?? (object)DBNull.Value),
                    new MySqlParameter("@p_created_at", DateTime.Now),
                    new MySqlParameter("@p_updated_at", DateTime.Now)
                };

                ExecuteNonProcedure(procedureName, parameters);
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
                string procedureName = StoredProcedures.UpdateAddress;
                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@p_id", address.Id),
                    new MySqlParameter("@p_country", address.Country),
                    new MySqlParameter("@p_zipcode", address.ZipCode),
                    new MySqlParameter("@p_city", address.City),
                    new MySqlParameter("@p_street", address.Street),
                    new MySqlParameter("@p_house_number", address.HouseNumber),
                    new MySqlParameter("@p_house_number_extension", address.HouseNumberExtension ?? (object)DBNull.Value),
                    new MySqlParameter("@p_updated_at", DateTime.Now)
                };

                ExecuteNonProcedure(procedureName, parameters);
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
                string procedureName = StoredProcedures.DeleteAddress;
                var parameters = new MySqlParameter[] { new MySqlParameter("@p_id", id) };
                ExecuteNonProcedure(procedureName, parameters);
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