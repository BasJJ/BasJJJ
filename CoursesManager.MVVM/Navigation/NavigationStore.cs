using CoursesManager.MVVM.Data;

namespace CoursesManager.MVVM.Navigation;

public class NavigationStore : IsObservable
{
    private NavigatableViewModel? _currentViewModel;
    public NavigatableViewModel? CurrentViewModel
    {
        get => _currentViewModel;
        set => SetProperty(ref _currentViewModel, value);
    }
}