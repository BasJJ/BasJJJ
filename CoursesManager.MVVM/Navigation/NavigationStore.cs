using CoursesManager.MVVM.Data;

namespace CoursesManager.MVVM.Navigation;

public class NavigationStore : IsObservable
{
    private ViewModel? _currentViewModel;
    public ViewModel? CurrentViewModel
    {
        get => _currentViewModel;
        set => SetProperty(ref _currentViewModel, value);
    }
}