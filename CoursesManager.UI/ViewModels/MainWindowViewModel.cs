using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Messages;
using CoursesManager.MVVM.Navigation;
using System.Windows.Input;
using CoursesManager.MVVM.Data;
using CoursesManager.UI.Messages;
using CoursesManager.UI.ViewModels.Courses;
using System.Windows.Navigation;

namespace CoursesManager.UI.ViewModels;

public class MainWindowViewModel : NavigatableViewModel
{
    public INavigationService NavigationService { get; set; }
    private readonly IMessageBroker _messageBroker;


    public ICommand CloseCommand { get; private set; }

    public ICommand OpenSidebarCommand { get; private set; }

    public ICommand GoForwardCommand { get; private set; }

    public ICommand GoBackCommand { get; private set; }

    public ICommand MouseEnterButtonCommand { get; private set; }
    public ICommand MouseEnterBorderCommand { get; private set; }
    public ICommand MouseLeaveButtonCommand { get; private set; }
    public ICommand MouseLeaveBorderCommand { get; private set; }
    public ICommand GoToStudentManagementView { get; private set; }
    public ICommand GoToCourseManagementView { get; private set; }


    private bool _isSidebarHidden;
    public bool IsSidebarHidden
    {
        get => _isSidebarHidden;
        set => SetProperty(ref _isSidebarHidden, value);
    }

    private bool _isMouseOverButton;
    public bool IsMouseOverButton
    {
        get => _isMouseOverButton;
        set => SetProperty(ref _isMouseOverButton, value);
    }

    private bool _isMouseOverBorder;
    public bool IsMouseOverBorder
    {
        get => _isMouseOverBorder;
        set => SetProperty(ref _isMouseOverBorder, value);
    }

    public MainWindowViewModel(INavigationService navigationService, IMessageBroker messageBroker) : base(navigationService)
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

        MouseEnterButtonCommand = new RelayCommand(() =>
        {
            IsMouseOverButton = true;
            UpdateSidebarVisibility();
        });
        MouseEnterBorderCommand = new RelayCommand(() =>
        {
            IsMouseOverBorder = true;
            UpdateSidebarVisibility();
        });
        MouseLeaveBorderCommand = new RelayCommand(() =>
        {
            IsMouseOverBorder = false;
            UpdateSidebarVisibility();
        });
        MouseLeaveButtonCommand = new RelayCommand(() =>
        {
            IsMouseOverButton = false;
            UpdateSidebarVisibility();
        });
        GoToStudentManagementView = new RelayCommand(() =>
        {
            _navigationService.NavigateTo<StudentManagerViewModel>();
            IsSidebarHidden = false;
        });
        GoToCourseManagementView = new RelayCommand(() =>
        {
            _navigationService.NavigateTo<CoursesManagerViewModel>();
            IsSidebarHidden = false;
        });
    }

    private async void UpdateSidebarVisibility()
    {
        await Task.Delay(300);
        if (IsMouseOverButton || IsMouseOverBorder)
        {
            IsSidebarHidden = true;
        }
        else
        {
            IsSidebarHidden = false;
        }
    }
}