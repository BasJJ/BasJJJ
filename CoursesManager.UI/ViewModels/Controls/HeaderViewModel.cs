using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Navigation;

namespace CoursesManager.UI.ViewModels.Controls;

public class HeaderViewModel(INavigationService navigationService) : NavigatableViewModel(navigationService)
{
    public INavigationService NavigationService => _navigationService;
}