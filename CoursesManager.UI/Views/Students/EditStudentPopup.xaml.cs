using System.Diagnostics;
using System.Windows;
using CoursesManager.UI.ViewModels.Students;

namespace CoursesManager.UI.Views.Students
{
    /// <summary>
    /// Interaction logic for EditStudentPopup.xaml
    /// </summary>
    public partial class EditStudentPopup : Window
    {
        public EditStudentPopup()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is EditStudentViewModel viewModel && viewModel != null)
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