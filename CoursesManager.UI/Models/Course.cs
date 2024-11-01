using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesManager.UI.Models
{
    public class Course
    {
        public int ID { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string Category { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
