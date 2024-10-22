using CoursesManager.MVVM.Navigation;
using CoursesManager.UI.ViewModels;
using System.Windows;
using CoursesManager.MVVM.Dialogs;

namespace CoursesManager.UI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            INavigationService mainNavigationService = new NavigationService();
            IDialogService dialogService = new DialogService();

            INavigationService.RegisterViewModelFactory<StudentManagerViewModel>((navigationService) => new StudentManagerViewModel(navigationService));

            mainNavigationService.NavigateTo<StudentManagerViewModel>();

            MainWindowViewModel mainWindowViewModel = new(mainNavigationService);

            MainWindow mw = new();
            mw.DataContext = mainWindowViewModel; 

            mw.Show();
        }
    }
}