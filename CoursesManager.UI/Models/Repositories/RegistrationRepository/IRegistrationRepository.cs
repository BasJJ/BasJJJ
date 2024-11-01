using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesManager.UI.Models.Repositories.RegistrationRepository
{
    public interface IRegistrationRepository
    { IEnumerable<Registration> GetAll();
        Registration GetById(int id);
        void Add(Registration registration);
        void Update(Registration registration);
        void Delete(int id);
    }
}
