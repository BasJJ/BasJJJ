using System.Windows;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.MVVM.Navigation;
using CoursesManager.UI.ViewModels;
using CoursesManager.MVVM.Messages;
using CoursesManager.UI.Dialogs.ViewModels;
using CoursesManager.UI.Dialogs.Windows;
using CoursesManager.UI.Messages;
using CoursesManager.UI.Dialogs.ResultTypes;

namespace CoursesManager.UI;

public partial class App : Application
{
    public static INavigationService NavigationService { get; set; } = new NavigationService();
    public static IMessageBroker MessageBroker { get; set; } = new MessageBroker();
    public static IDialogService DialogService { get; set; } = new DialogService();

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        RegisterViewModels();
        RegisterDialogs();

        MessageBroker.Subscribe<ApplicationCloseRequestedMessage>(ApplicationCloseRequestedHandler);

        NavigationService.NavigateTo<StudentManagerViewModel>();
        NavigationService.NavigateTo<TestViewModel>();
        NavigationService.NavigateTo<StudentManagerViewModel>();
        NavigationService.NavigateTo<TestViewModel>();
        NavigationService.NavigateTo<StudentManagerViewModel>();
        NavigationService.NavigateTo<TestViewModel>();
        NavigationService.NavigateTo<StudentManagerViewModel>();
        NavigationService.NavigateTo<TestViewModel>();
        NavigationService.NavigateTo<StudentManagerViewModel>();
        //NavigationService.NavigateTo<FixingTableViewModel>();

        MainWindow mw = new()
        {
            DataContext = new MainWindowViewModel(NavigationService, MessageBroker)
        };
        mw.Show();
    }

    private void RegisterDialogs()
    {
        DialogService.RegisterDialog<YesNoDialogViewModel, YesNoDialogWindow, YesNoDialogResultType>((initial) => new YesNoDialogViewModel(initial));
    }

    private void RegisterViewModels()
    {
        INavigationService.RegisterViewModelFactory(() => new StudentManagerViewModel());
        INavigationService.RegisterViewModelFactory(() => new TestViewModel());
        INavigationService.RegisterViewModelFactory(() => new FixingTableViewModel());
    }

    /// <summary>
    /// Enables us to close the app by sending a message through the messenger.
    /// </summary>
    /// <param name="obj"></param>
    private static async void ApplicationCloseRequestedHandler(ApplicationCloseRequestedMessage obj)
    {
        var result = await DialogService.ShowDialogAsync<YesNoDialogViewModel, YesNoDialogResultType>(new YesNoDialogResultType
        {
            DialogTitle = "CoursesManager",
            DialogText = "Wil je de app afsluiten?"
        });

        if (result.Data is null) return;

        if (result.Data.Result) Application.Current.Shutdown();
    }
}