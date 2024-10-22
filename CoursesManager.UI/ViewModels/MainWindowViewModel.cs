using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Navigation;
using CoursesManager.UI.ViewModels.Controls;

namespace CoursesManager.UI.ViewModels;

public class MainWindowViewModel
{
    public HeaderViewModel HeaderViewModel { get; }
    public INavigationService NavigationService { get; set; }

    public MainWindowViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
        HeaderViewModel = new HeaderViewModel(navigationService);
    }
}
