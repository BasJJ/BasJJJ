
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using CoursesManager.UI.ViewModels.Students;

namespace CoursesManager.UI.Views.Students
{
    /// <summary>
    /// Interaction logic for StudentDetailView.xaml
    /// </summary>
    public partial class StudentDetailView : UserControl
    {
        public StudentDetailView()
        {
            InitializeComponent();

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is StudentDetailViewModel viewModel && viewModel != null)
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
