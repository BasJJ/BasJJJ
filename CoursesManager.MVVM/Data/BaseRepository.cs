using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection;

namespace CoursesManager.MVVM.Data
{
    public abstract class BaseRepository
    {
        private readonly string _connectionString;
        private readonly string _modelTabel;

        public BaseRepository(string table, string connectionString)
        {
            _modelTabel = table;
            _connectionString = connectionString;
        }

        protected MySqlConnection GetConnection() => new(_connectionString);

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
                catch (MySqlException exception)
                {
                    LogUtil.Error(exception.Message);
                }
                catch (InvalidCastException exception)
                {
                    LogUtil.Error(exception.Message);
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

        protected bool InsertRow(Dictionary<string, object> data)
        {
            if (data == null || data.Count == 0)
            {
                LogUtil.Error(nameof(data) + " cannot be null or empty.");
                return false;
            }

            string columns = string.Join(", ", data.Keys.Select(k => $"`{k}`"));
            string parameters = string.Join(", ", data.Keys.Select(k => $"@{k}"));
            string query = $"INSERT INTO `{_modelTabel}` ({columns}) VALUES ({parameters});";

            return ExecuteNonQuery(query, data);
        }

        protected bool UpdateRow(Dictionary<string, object> data, string whereClause, params MySqlParameter[] parameters)
        {
            if (data == null || data.Count == 0)
            {
                LogUtil.Error(nameof(data) + " cannot be null or empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(whereClause))
            {
                LogUtil.Error("Where clause cannot be null or empty.");
                return false;
            }

            string setClause = string.Join(", ", data.Keys.Select(k => $"`{k}` = @{k}"));
            string query = $"UPDATE `{_modelTabel}` SET {setClause} WHERE {whereClause};";

            List<MySqlParameter> allParameters = data.Select(kvp => new MySqlParameter($"@{kvp.Key}", kvp.Value ?? DBNull.Value)).ToList();
            if (parameters != null)
                allParameters.AddRange(parameters);

            return ExecuteNonQuery(query, allParameters.ToArray());
        }

        protected bool DeleteRow(string whereClause, params MySqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(whereClause))
            {
                LogUtil.Error("Where clause cannot be null or empty.");
                return false;
            }

            string query = $"DELETE FROM `{_modelTabel}` WHERE {whereClause};";
            return ExecuteNonQuery(query, parameters);
        }

        protected T FillModel<T>(MySqlDataReader mySqlReader, PropertyInfo[] properties) where T : new()
        {
            T model = new();

            foreach (var property in properties)
            {
                if (!HasColumn(mySqlReader, property.Name) || mySqlReader[property.Name] is DBNull)
                    continue;

                try
                {
                    property.SetValue(model, Convert.ChangeType(mySqlReader[property.Name], property.PropertyType));
                }
                catch (Exception exception)
                {
                    throw new InvalidCastException(
                        $"Error converting column '{property.Name}' to property '{property.Name}' of type '{property.PropertyType}'.", exception);
                }
            }

            return model;
        }

        private bool ExecuteNonQuery(string query, params MySqlParameter[] parameters)
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
            catch (MySqlException exception)
            {
                LogUtil.Error(exception.Message);
                return false;
            }

            return true;
        }

        private bool ExecuteNonQuery(string query, Dictionary<string, object> data)
        {
            MySqlParameter[] parameters = data.Select(kvp => new MySqlParameter($"@{kvp.Key}", kvp.Value ?? DBNull.Value)).ToArray();
            return ExecuteNonQuery(query, parameters);
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