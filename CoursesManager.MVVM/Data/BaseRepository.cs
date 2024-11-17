using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection;
using System.Windows.Input;

namespace CoursesManager.MVVM.Data
{
    public abstract class BaseRepository
    {
        private readonly string _connectionString;
        private readonly string _modelTabel;

        public BaseRepository(string table)
        {
            _modelTabel = table;
            _connectionString = "Server=85.10.149.233;Port=3306;User=courses_manager;Password=C0urs3sManager;Database=courses_manager;";
        }

        protected MySqlConnection GetConnection () => new(_connectionString);

        protected List<T> FetchAll<T>() where T : new()
        {
            return FetchAll<T>($"SELECT * FROM `{_modelTabel}`;");
        }
        protected List<T> FetchAll<T>(string query, params MySqlParameter[] parameters) where T : new()
        {
            List<T> result = new List<T>();
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            using (var mySqlConnection = GetConnection())
            using (MySqlCommand mySqlCommand = new($"{query};", mySqlConnection))
            {
                if (parameters != null)
                    mySqlCommand.Parameters.AddRange(parameters);

                try
                {
                    mySqlConnection.Open();

                    using MySqlDataReader mySqlReader = mySqlCommand.ExecuteReader();
                    while (mySqlReader.Read())
                    {
                        result.Add(FillModel<T>(mySqlReader, properties));
                    }
                }
                catch (MySqlException ex)
                {
                    throw new DataException("An error occurred while querying the database.", ex);
                }
            }

            return result;
        }

        protected T FetchOneByID<T>(int id) where T : new()
        {
            return FetchAll<T>(
                $"SELECT * FROM `{_modelTabel}` WHERE ID = @ID;",
                [new MySqlParameter("@ID", id)]
            ).FirstOrDefault();
        }

        protected void InsertRow(Dictionary<string, object> data)
        {
            if (data == null || data.Count == 0)
                throw new ArgumentException("Data cannot be null or empty.", nameof(data));

            string columns = string.Join(", ", data.Keys.Select(k => $"`{k}`"));
            string parameters = string.Join(", ", data.Keys.Select(k => $"@{k}"));
            string query = $"INSERT INTO `{_modelTabel}` ({columns}) VALUES ({parameters});";

            ExecuteNonQuery(query, data);
        }

        protected void UpdateRow(Dictionary<string, object> data, string whereClause, params MySqlParameter[] parameters)
        {
            if (data == null || data.Count == 0)
                throw new ArgumentException("Data cannot be null or empty.", nameof(data));

            if (string.IsNullOrWhiteSpace(whereClause))
                throw new ArgumentException("Where clause cannot be null or empty.", nameof(whereClause));

            string setClause = string.Join(", ", data.Keys.Select(k => $"`{k}` = @{k}"));
            string query = $"UPDATE `{_modelTabel}` SET {setClause} WHERE {whereClause};";

            List<MySqlParameter> allParameters = data.Select(kvp => new MySqlParameter($"@{kvp.Key}", kvp.Value ?? DBNull.Value)).ToList();
            if (parameters != null)
                allParameters.AddRange(parameters);

            ExecuteNonQuery(query, allParameters.ToArray());
        }

        protected void DeleteRow(string whereClause, params MySqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(whereClause))
                throw new ArgumentException("Where clause cannot be null or empty.", nameof(whereClause));

            string query = $"DELETE FROM `{_modelTabel}` WHERE {whereClause};";
            ExecuteNonQuery(query, parameters);
        }

        protected T FillModel<T>(MySqlDataReader mySqlReader, PropertyInfo[] properties) where T : new()
        {
            T model = new T();

            foreach (var property in properties)
            {
                if (!HasColumn(mySqlReader, property.Name) || mySqlReader[property.Name] is DBNull)
                    continue;

                try
                {
                    property.SetValue(model, Convert.ChangeType(mySqlReader[property.Name], property.PropertyType));
                }
                catch (Exception ex)
                {
                    throw new InvalidCastException(
                        $"Error converting column '{property.Name}' to property '{property.Name}' of type '{property.PropertyType}'.", ex);
                }
            }

            return model;
        }

        private void ExecuteNonQuery(string query, params MySqlParameter[] parameters)
        {
            using var mySqlConnection = GetConnection();
            using var mySqlCommand = new MySqlCommand(query, mySqlConnection);
            if (parameters != null)
                mySqlCommand.Parameters.AddRange(parameters);

            try
            {
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                throw new DataException("An error occurred while executing the database command.", ex);
            }
        }
        private void ExecuteNonQuery(string query, Dictionary<string, object> data)
        {
            MySqlParameter[] parameters = data.Select(kvp => new MySqlParameter($"@{kvp.Key}", kvp.Value ?? DBNull.Value)).ToArray();
            ExecuteNonQuery(query, parameters);
        }

        private bool HasColumn(MySqlDataReader mySqlReader, string columnName)
        {
            for (var i = 0; i < mySqlReader.FieldCount; i++)
            {
                if (mySqlReader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
