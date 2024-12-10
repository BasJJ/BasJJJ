using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoursesManager.MVVM.Data;

namespace CoursesManager.UI.Models
{
    public class Location : ICopyable<Location>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public Location Copy()
        {
            return new Location
            {
                Id = Id,
                Name = Name,
                Address = Address
            };
        }
    }
}
