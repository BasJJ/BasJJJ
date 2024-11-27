using CoursesManager.MVVM.Navigation;

namespace CoursesManager.MVVM.Data;

public class ViewModelWithNavigation : ViewModel
{
    protected INavigationService _navigationService;

    public ViewModelWithNavigation(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }
}