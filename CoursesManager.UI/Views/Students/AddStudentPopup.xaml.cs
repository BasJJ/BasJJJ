using System.Diagnostics;
using System.Windows;
using CoursesManager.UI.ViewModels.Students;

namespace CoursesManager.UI.Views.Students
{
    /// <summary>
    /// Interaction logic for AddStudentPopup.xaml
    /// </summary>
    public partial class AddStudentPopup : Window
    {
        public AddStudentPopup()
        {
            InitializeComponent();
        }

        //can be better but for now it is ok
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is AddStudentViewModel viewModel && viewModel != null)
            {
                viewModel.ParentWindow = this;
            }
            else
            {
                Debug.WriteLine("DataContext is either null or not of type AddStudentViewModel.");
            }
        }
    }
}