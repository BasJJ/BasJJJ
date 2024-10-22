using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesManager.UI.ViewModels
{
    public class StudentManagerViewModel : NavigatableViewModel
    {
        public StudentManagerViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
