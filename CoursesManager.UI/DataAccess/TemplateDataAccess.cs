using CoursesManager.UI.Database;
using CoursesManager.UI.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace CoursesManager.UI.DataAccess
{
    internal class TemplateDataAccess : BaseDataAccess<Template>
    {

        public List<Template> GetAll()
        {
            try
            {
                string procedureName = StoredProcedures.GetAllTemplates;
                return ExecuteProcedure(procedureName).Select(row => ToTemplate(row)).ToList();

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
                return ExecuteProcedure(procedureName).Select(row => ToTemplate(row)).FirstOrDefault();

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
                    new MySqlParameter("@p_html", template.SubjectString),
                    new MySqlParameter("@p_name", template.Name),
                    new MySqlParameter("@p_updated_at", DateTime.Now)
                ]);
            }
            catch (MySqlException ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        private Template ToTemplate(DataRow row)
        {
            return new Template
            {
                Id = Convert.ToInt32(row["id"]),
                HtmlString = row["html"]?.ToString() ?? string.Empty,
                Name = row["name"]?.ToString() ?? string.Empty
            };
        }
    }

}
