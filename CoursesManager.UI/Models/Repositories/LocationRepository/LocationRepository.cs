using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesManager.UI.Models.Repositories.LocationRepository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly List<Location> _locations = new List<Location>();

        public IEnumerable<Location> GetAll()
        {
            return _locations.ToList();
        }

        public Location GetById(int id)
        {
            return _locations.FirstOrDefault(l => l.Id == id);
        }

        public void Add(Location location)
        {
            location.DateCreated = DateTime.Now;
            _locations.Add(location);
        }

        public void Update(Location location)
        {
            var existingLocation = GetById(location.Id);
            if (existingLocation != null)
            {
                existingLocation.Name = location.Name;
                existingLocation.Address = location.Address;
                existingLocation.Capacity = location.Capacity;
            }
        }

        public void Delete(int id)
        {
            var location = GetById(id);
            if (location != null)
            {
                _locations.Remove(location);
            }
        }
    }
}
