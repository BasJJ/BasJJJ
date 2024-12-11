using CoursesManager.UI.Database;
using CoursesManager.UI.Models;
using MySql.Data.MySqlClient;

namespace CoursesManager.UI.DataAccess
{
    internal class TemplateDataAccess : BaseDataAccess<Template>
    {

        //public List<Location> GetAllWithAddresses()
        //{
        //    try
        //    {
        //        return ExecuteProcedure(StoredProcedures.LocationsWithAddressesGetAll).Select(row => new Location
        //        {
        //            Address = new Address
        //            {
        //                Id = Convert.ToInt32(row["id"]),
        //                City = row["city"]?.ToString() ?? string.Empty,
        //                Country = row["country"]?.ToString() ?? string.Empty,
        //                HouseNumber = row["house_number"]?.ToString() ?? string.Empty,
        //                Street = row["street"]?.ToString() ?? string.Empty,
        //                ZipCode = row["zipcode"]?.ToString() ?? string.Empty
        //            }
        //        }).ToList();
        //    }
        //    catch (MySqlException ex)
        //    {
        //        throw new InvalidOperationException(ex.Message, ex);
        //    }
        //}

        public List<Template> GetAll()
        {
            try
            {
                string procedureName = StoredProcedures.GetAllTemplates;
                return ExecuteProcedure(procedureName).Select(row => new Template
                {
                    Id = Convert.ToInt32(row["id"]),
                    HtmlString = row["html"]?.ToString() ?? string.Empty,
                    Name = row["name"]?.ToString() ?? string.Empty
                }).ToList();

            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        public Template? GetByName(string name)
        {

            try
            {
                string procedureName = StoredProcedures.GetTemplateByName;
                var parameters = new MySqlParameter[] { new MySqlParameter("@p_name", name) };
                var Templates = FetchAll(procedureName, parameters);
                return Templates.FirstOrDefault();
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }

        }

        public void Update(Location data)
        {
            try
            {
                ExecuteNonProcedure(StoredProcedures.LocationUpdate, [
                    new MySqlParameter("@p_location_id", data.Id),
                new MySqlParameter("@p_new_name", data.Name),
                new MySqlParameter("@p_new_address_id", data.Address.Id)
                ]);
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }


        public void UpdateTemplate(Template template)
        {

            try
            {
                ExecuteNonProcedure(StoredProcedures.UpdateTemplate, [
                    new MySqlParameter("@p_id", template.Id),
                    new MySqlParameter("@p_html", template.HtmlString),
                    new MySqlParameter("@p_name", template.Name),
                    new MySqlParameter("@p_updated_at", DateTime.Now)
                ]);
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }
    }

}
