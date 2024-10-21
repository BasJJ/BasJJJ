using CoursesManager.MVVM.Navigation;

namespace CoursesManager.MVVM.Data;

public class NavigatableViewModel : ViewModel
{
    protected INavigationService _navigationService;

    public NavigatableViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }           
}