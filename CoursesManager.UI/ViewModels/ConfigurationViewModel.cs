using System.ComponentModel;
using System.Windows.Input;
using CoursesManager.MVVM.Commands;
using CoursesManager.UI.Models;
using CoursesManager.UI.Service;

namespace CoursesManager.UI.ViewModels
{
    public class ConfigurationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private  readonly  ConfigurationService _configurationService;

        private EnvModel _appConfig;

        public EnvModel AppConfig
        {
            get => _appConfig;
            set
            {
                _appConfig = value;
                OnPropertyChanged(nameof(AppConfig));
            }
        }

        public ICommand SaveCommand { get;  }

        public ConfigurationViewModel(ConfigurationService configurationService)
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

        
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
