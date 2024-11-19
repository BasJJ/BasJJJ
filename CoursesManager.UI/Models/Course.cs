using System.Collections.ObjectModel;
using System.Text;

namespace CoursesManager.UI.Models
{
    public class Course
    {
        public int ID { get; set; }
        public string Name 
        { 
            get; 
            set; 
        } = string.Empty;

        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Participants { get; set; }
        public bool IsActive { get; set; }
        public bool IsPayed { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int LocationId { get; set; }
        public Location? Location { get; set; }
        public DateTime DateCreated { get; set; }
        public ObservableCollection<Student>? students { get; set; }

        public string GenerateFilterString()
        {
            var sb = new StringBuilder();

            sb.Append(Name);
            sb.Append(Code);
            sb.Append(Category);
            sb.Append(StartDate.ToString("yyyy-MM-dd"));

            if (Location != null)
            {
                sb.Append(Location.Name);
                sb.Append(Location.Address);
            }

            return sb.ToString().Replace(" ", string.Empty);
        }
    }
}
