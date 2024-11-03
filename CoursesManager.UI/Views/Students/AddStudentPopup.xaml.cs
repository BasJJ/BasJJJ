using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            DataContext = new AddStudentViewModel();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Add logic to save the student details
            this.DialogResult = true;
            this.Close();
        }
    }
}
