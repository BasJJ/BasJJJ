using CoursesManager.MVVM.Data;

namespace CoursesManager.MVVM.Navigation;

public interface INavigationService
{
    NavigationStore NavigationStore { get; }
    void NavigateTo<TViewModel>() where TViewModel : NavigatableViewModel;
    void GoBack();
    bool CanGoBack();
    void GoForward();
    bool CanGoForward();

    public static readonly Dictionary<Type, Func<NavigationService, NavigatableViewModel>> ViewModelFactories = new();

    public static void RegisterViewModelFactory<TViewModel>(Func<NavigationService, TViewModel> viewModelFactory) where TViewModel : NavigatableViewModel
    {
        ArgumentNullException.ThrowIfNull(viewModelFactory);

        if (ViewModelFactories.ContainsKey(typeof(TViewModel))) return;

        ViewModelFactories.Add(typeof(TViewModel), viewModelFactory);
    }
}