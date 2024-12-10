using System.Configuration;
using CoursesManager.MVVM.Env;
using CoursesManager.UI.Models;

namespace CoursesManager.UI.Service
{
    public class ConfigurationService : IConfigurationService
    {

        private readonly EncryptionService _encryptionService;


        public ConfigurationService( EncryptionService encryptionService)
        {
            _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
        }

        public void SaveEnvSettings(Dictionary<string, string> dbParams, Dictionary<string, string> mailParams)
        {
            if (dbParams == null || mailParams == null)
            {
                throw new ArgumentNullException("Parameters voor opslaan mogen niet null zijn.");
            }

            var dbConnectionString = BuildConnectionString(dbParams);
            var mailConnectionString = BuildConnectionString(mailParams);

            EnvManager<EnvModel>.Values.ConnectionString = _encryptionService.Encrypt(dbConnectionString);
            EnvManager<EnvModel>.Values.MailConnectionString = _encryptionService.Encrypt(mailConnectionString);

            EnvManager<EnvModel>.Save();
        }



        public EnvModel GetDecryptedEnvSettings()
        {
            EnvManager<EnvModel>.Values.ConnectionString =
                _encryptionService.Decrypt(EnvManager<EnvModel>.Values.ConnectionString);

            EnvManager<EnvModel>.Values.MailConnectionString =
                _encryptionService.Decrypt(EnvManager<EnvModel>.Values.MailConnectionString);

            return EnvManager<EnvModel>.Values;
        }




        public Dictionary<string, string> GetDatabaseParameters()
        {
            var decryptedConfig = GetDecryptedEnvSettings();
            if (string.IsNullOrWhiteSpace(decryptedConfig.ConnectionString))
            {
                Console.WriteLine("Geen ConnectionString gevonden in de gedecrypteerde instellingen.");
                return new Dictionary<string, string>();
            }

            return ParseConnectionString(decryptedConfig.ConnectionString);
        }

        public Dictionary<string, string> GetMailParameters()
        {
            var decryptedConfig = GetDecryptedEnvSettings();
            if (string.IsNullOrWhiteSpace(decryptedConfig.MailConnectionString))
            {
                Console.WriteLine("Geen MailConnectionString gevonden in de gedecrypteerde instellingen.");
                return new Dictionary<string, string>();
            }

            return ParseConnectionString(decryptedConfig.MailConnectionString);
        }



        public bool ValidateSettings()
        {
            var config = GetDecryptedEnvSettings();

            return !string.IsNullOrWhiteSpace(config.ConnectionString) &&
                   !string.IsNullOrWhiteSpace(config.MailConnectionString);
        }

        private Dictionary<string, string> ParseConnectionString(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Console.WriteLine("ConnectionString is leeg of null.");
                return new Dictionary<string, string>();
            }

            try
            {
                var result = connectionString
                    .Split(';', StringSplitOptions.RemoveEmptyEntries)
                    .Where(part => part.Contains("="))
                    .Select(part => part.Split('='))
                    .ToDictionary(split => split[0].Trim(), split => split[1].Trim());

                foreach (var kvp in result)
                {
                    Console.WriteLine($"Parsed: {kvp.Key} = {kvp.Value}");
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij het parsen van de connectionstring: {ex.Message}");
                return new Dictionary<string, string>();
            }
        }


        private string BuildConnectionString(Dictionary<string, string> parameters)
        {
            return string.Join(";", parameters.Select(kv => $"{kv.Key}={kv.Value}"));
        }


    }
}
