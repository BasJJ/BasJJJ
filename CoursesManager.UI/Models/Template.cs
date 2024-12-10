using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesManager.UI.Models
{
    public class Template
    {
        public string HtmlString { get; set; }
        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
