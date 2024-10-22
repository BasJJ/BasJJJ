using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Navigation;

namespace CoursesManager.UI.ViewModels
{
    public class StudentManagerViewModel : NavigatableViewModel
    {
        public StudentManagerViewModel(INavigationService navigationService) : base(navigationService)
        {
            ViewTitle = "Cursisten beheer";
        }
    }
}
