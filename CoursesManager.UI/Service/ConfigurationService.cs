using System.Configuration;
using CoursesManager.MVVM.Env;
using CoursesManager.UI.Models;

namespace CoursesManager.UI.Service
{
    public class ConfigurationService : IConfigurationService
    {

        private readonly Lazy<EnvManager<EnvModel>> _envManager = new(() => new EnvManager<EnvModel>());
        private readonly EncryptionService _encryptionService;



        public ConfigurationService( EncryptionService encryptionService)
        {
            _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
        }

        public void SaveEnvSettings(EnvModel appConfig)
        {
            if (appConfig == null)
            {
                throw new ArgumentNullException(nameof(appConfig));
            }

            var encryptedConfig = new EnvModel
            {
                ConnectionString = _encryptionService.Encrypt(appConfig.ConnectionString),
                MailConnectionString = _encryptionService.Encrypt(appConfig.MailConnectionString)
            };

            _envManager.Value.Save(encryptedConfig); // Gebruik Lazy<T>.Value
        }

        public EnvModel GetDecryptedEnvSettings()
        {
            var encryptedConfig = _envManager.Value.Load(); // Gebruik Lazy<T>.Value

            if (encryptedConfig == null)
            {
                return new EnvModel();
            }

            var decryptedConfig = new EnvModel
            {
                ConnectionString = !string.IsNullOrWhiteSpace(encryptedConfig.ConnectionString)
                    ? _encryptionService.Decrypt(encryptedConfig.ConnectionString)
                    : string.Empty, 
                MailConnectionString = !string.IsNullOrWhiteSpace(encryptedConfig.MailConnectionString)
                    ? _encryptionService.Decrypt(encryptedConfig.MailConnectionString)
                    : string.Empty 
            };

            return decryptedConfig;
        }


        public bool ValidateSettings()
        {
            var config = GetDecryptedEnvSettings();

            return !string.IsNullOrWhiteSpace(config.ConnectionString) &&
                   !string.IsNullOrWhiteSpace(config.MailConnectionString);
        }

        public string GetConnectionString(string key)
        {
            var config = GetDecryptedEnvSettings();

            return key switch
            {
                "Database" => config.ConnectionString,
                "Mail" => config.MailConnectionString,
                _ => throw new ArgumentException("Ongeldige sleutel voor connectiestring.", nameof(key))
            };
        }

    }
}
