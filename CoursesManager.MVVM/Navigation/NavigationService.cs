using CoursesManager.MVVM.Data;

namespace CoursesManager.MVVM.Navigation;

public class NavigationService : INavigationService
{
    public NavigationStore NavigationStore { get; } = new();

    private readonly Stack<NavigatableViewModel> _forwardViewModels = new();

    private readonly Stack<NavigatableViewModel> _backwardViewModels = new();

    public void NavigateTo<TViewModel>() where TViewModel : NavigatableViewModel
    {
        if (!INavigationService.ViewModelFactories.TryGetValue(typeof(TViewModel), out var factory))
        {
            throw new InvalidOperationException($"No factory registered for {typeof(TViewModel).Name}");
        }

        _forwardViewModels.Clear();

        if (NavigationStore.CurrentViewModel is not null)
        {
            _backwardViewModels.Push(NavigationStore.CurrentViewModel);
        }
        
        var existingViewModel = _backwardViewModels.OfType<TViewModel>().FirstOrDefault();

        if (existingViewModel is not null)
        {
            NavigationStore.CurrentViewModel = existingViewModel;
        }
        else
        {
            NavigationStore.CurrentViewModel = factory(this);
        }
    }

    public void GoBack()
    {
        if (!CanGoBack()) return;

        if (NavigationStore.CurrentViewModel is not null)
        {
            _forwardViewModels.Push(NavigationStore.CurrentViewModel);
        }

        NavigationStore.CurrentViewModel = _backwardViewModels.Pop();
    }

    public bool CanGoBack() => _backwardViewModels.Any();

    public void GoForward()
    {
        if (!CanGoForward()) return;

        if (NavigationStore.CurrentViewModel is not null)
        {
            _backwardViewModels.Push(NavigationStore.CurrentViewModel);
        }
        
        NavigationStore.CurrentViewModel = _forwardViewModels.Pop();
    }

    public bool CanGoForward() => _forwardViewModels.Any();
}
