using System.ComponentModel;
using System.Windows.Input;
using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Data;
using CoursesManager.UI.Models;
using CoursesManager.UI.Service;

namespace CoursesManager.UI.ViewModels
{
    public class ConfigurationViewModel : ViewModel
    {

        private readonly IConfigurationService _configurationService;

        private EnvModel _appConfig;

        public EnvModel AppConfig
        {
            get => _appConfig;
            set => SetProperty(ref _appConfig, value);
        }


        public ICommand SaveCommand { get; }

        public ConfigurationViewModel(IConfigurationService configurationService)
        {
            _configurationService = configurationService;

            InitializeSettings();

            SaveCommand = new RelayCommand(ValidateAndSave, CanSave);
        }

        public void InitializeSettings()
        {
            AppConfig = _configurationService.GetDecryptedEnvSettings();
        }

        public void ValidateAndSave()
        {
            try
            {
                if (!CanSave())
                {
                    throw new InvalidOperationException("Instellingen zijn ongeldig. Controleer de ingevoerde waarden.");
                }


                _configurationService.SaveEnvSettings(AppConfig);
                Console.WriteLine("Instellingen succesvol opgeslagen!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij opslaan: {ex.Message}");
            }
        }


        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(AppConfig.ConnectionString) &&
                   !string.IsNullOrWhiteSpace(AppConfig.MailConnectionString);
        }
    }
}
