using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesManager.UI.Models.Repositories.LocationRepository
{
    public interface ILocationRepository
    { IEnumerable<Location> GetAll();
        Location GetById(int id);
        void Add(Location location);
        void Update(Location location);
        void Delete(int id);
    }
}
