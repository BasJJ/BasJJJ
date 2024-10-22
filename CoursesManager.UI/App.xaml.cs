using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Input;
using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Dialogs;
using CoursesManager.MVVM.Navigation;
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

            INavigationService.RegisterViewModelFactory<StudentManagerViewModel>((navigationService) => new StudentManagerViewModel(navigationService));

            INavigationService mainNavigationService = new NavigationService();
            IDialogService dialogService = new DialogService();

            mainNavigationService.NavigateTo<StudentManagerViewModel>();

            MainWindow mw = new();
            mw.DataContext = new MainWindowViewModel(mainNavigationService);

            mw.Show();
        }
    }
}