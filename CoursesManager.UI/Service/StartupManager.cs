using System;

namespace CoursesManager.UI.Service
{
    public class StartupManager
    {
        private readonly IConfigurationService _configurationService;

        public StartupManager(IConfigurationService configurationService)
        {
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
        }

        public void CheckConfigurationOnStartup()
        {
            try
            {
                if (!_configurationService.ValidateSettings())
                {
                    Console.WriteLine("Configuratie is ongeldig. start de configuratie-instellingen");
                    OpenConfigurationUI();
                }
                else
                {
                    Console.WriteLine("Configuratie is geldig. Applicatie kan doorgaan.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout tijdens configuratiecontrole: {ex.Message}");
                OpenConfigurationUI();
            }
        }

        private void OpenConfigurationUI()
        {

            Console.WriteLine("Configuratie-UI wordt geopend...");

        }
    }
}

