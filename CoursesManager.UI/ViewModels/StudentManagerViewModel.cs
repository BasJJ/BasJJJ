using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Navigation;

namespace CoursesManager.UI.ViewModels
{
    public class StudentManagerViewModel : NavigatableViewModel
    {
        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public StudentManagerViewModel(INavigationService navigationService) : base(navigationService)
        {
            ViewTitle = "Cursisten beheer";
        }
    }
}