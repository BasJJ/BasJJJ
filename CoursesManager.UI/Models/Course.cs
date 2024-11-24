using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using CoursesManager.MVVM.Data;

namespace CoursesManager.UI.Models
{
    public class Course : ICopyable<Course>
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

        public Image? Image { get; set; }

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

        public Course Copy()
        {
            return new Course
            {
                ID = this.ID,
                Name = this.Name,
                Code = this.Code,
                Description = this.Description,
                Participants = this.Participants,
                IsActive = this.IsActive,
                IsPayed = this.IsPayed,
                Category = this.Category,
                StartDate = this.StartDate,
                EndDate = this.EndDate,
                LocationId = this.LocationId,
                Location = this.Location, // shallow copy? 
                DateCreated = this.DateCreated,
                students = this.students != null ? new ObservableCollection<Student>(this.students) : null, // Diepe kopie van de collectie
                Image = this.Image // diep of shallow copy?
            };
        }
    }
}
