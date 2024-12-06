using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoursesManager.UI.Models;

namespace CoursesManager.UI.Service
{
    public interface IConfigurationService
    {
        void SaveEnvSettings(EnvModel appConfig);
        EnvModel GetDecryptedEnvSettings();
        bool ValidateSettings();
    }
}
