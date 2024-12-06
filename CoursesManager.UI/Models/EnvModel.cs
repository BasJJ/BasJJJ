using System.Dynamic;

namespace CoursesManager.UI.Models
{
    public class EnvModel
    {
        public string ConnectionString { get; set; }
        public string MailConnectionString { get; set; }

        public string GetConnectionString() => ConnectionString;

        public void SetConnectionString(string connectionString) => ConnectionString = connectionString;

        public string GetMailConnectionString() => MailConnectionString;

        public void SetCMailConnectionString(string mailConnectionString) => MailConnectionString = mailConnectionString;

    }
}