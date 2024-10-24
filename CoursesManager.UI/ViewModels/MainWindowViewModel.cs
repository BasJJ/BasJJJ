using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Messages;
using CoursesManager.MVVM.Navigation;
using System.Windows.Input;
using CoursesManager.MVVM.Data;
using CoursesManager.UI.Messages;

namespace CoursesManager.UI.ViewModels;

public class MainWindowViewModel : ViewModel
{
    public INavigationService NavigationService { get; set; }
    private readonly IMessageBroker _messageBroker;

    public ICommand CloseCommand { get; private set; }

    public ICommand OpenSidebarCommand { get; private set; }

    public ICommand GoForwardCommand { get; private set; }

    public ICommand GoBackCommand { get; private set; }

    private bool _isSidebarHidden;

    public bool IsSidebarHidden
    {
        get => _isSidebarHidden;
        set => SetProperty(ref _isSidebarHidden, value);
    }

    public MainWindowViewModel(INavigationService navigationService, IMessageBroker messageBroker)
    {
        NavigationService = navigationService;
        _messageBroker = messageBroker;

        CloseCommand = new RelayCommand(() =>
        {
            _messageBroker.Publish<ApplicationCloseRequestedMessage>(new ApplicationCloseRequestedMessage());
        });

        OpenSidebarCommand = new RelayCommand(() =>
        {
            IsSidebarHidden = !IsSidebarHidden;
        });

        GoForwardCommand = new RelayCommand(() =>
        {
            NavigationService.GoForward();
        }, NavigationService.CanGoForward);

        GoBackCommand = new RelayCommand(() =>
        {
            NavigationService.GoBack();
        }, NavigationService.CanGoBack);
    }
}