using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace CoursesManager.UI.Views.Controls
{
    public partial class Toggle : UserControl
    {
        public Toggle()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IsToggledProperty = DependencyProperty.Register(
            nameof(IsToggled), 
            typeof(bool), 
            typeof(Toggle),
            new FrameworkPropertyMetadata(false)
        );


        public static readonly DependencyProperty ToggleCommandProperty = DependencyProperty.Register(
            nameof(ToggleCommand), 
            typeof(ICommand), 
            typeof(Toggle),
            new PropertyMetadata(null)
        );

        public ICommand ToggleCommand
        {
            get => (ICommand)GetValue(ToggleCommandProperty);
            set => SetValue(ToggleCommandProperty, value);
        }

        public bool IsToggled
        {
            get { return (bool)GetValue(IsToggledProperty); }
            set { SetValue(IsToggledProperty, value); }
        }
    }
}