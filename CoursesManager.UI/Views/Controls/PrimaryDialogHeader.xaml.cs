using System.Windows;
using System.Windows.Controls;

namespace CoursesManager.UI.Views.Controls
{
    /// <summary>
    /// Interaction logic for DialogHeader.xaml
    /// </summary>
    public partial class PrimaryDialogHeader : UserControl
    {
        public PrimaryDialogHeader()
        {
            InitializeComponent();
        }

        public string HeaderTitle
        {
            get => (string)GetValue(HeaderTitleProperty);
            set => SetValue(HeaderTitleProperty, value);
        }

        public static readonly DependencyProperty HeaderTitleProperty = DependencyProperty.Register(
            nameof(HeaderTitle),
            typeof(string),
            typeof(PrimaryDialogHeader),
            new PropertyMetadata(string.Empty));
    }
}