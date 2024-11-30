using CoursesManager.MVVM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesManager.UI.Models
{
    public class SelectableCourse : ViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}

