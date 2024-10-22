using CoursesManager.MVVM.Navigation;
using CoursesManager.UI.ViewModels;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;
using CoursesManager.MVVM.Dialogs;

namespace CoursesManager.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        { 
            MainWindow mw = new();
            INavigationService navigationService = new NavigationService();
            MainWindowViewModel mainWindowViewModel = new(navigationService);
            IDialogService dialogService = new DialogService();
            mw.Show();
            INavigationService.RegisterViewModelFactory<StudentManagerViewModel>((navigationService) =>
            {
                return new StudentManagerViewModel(navigationService);
            });
        }
    }

}
