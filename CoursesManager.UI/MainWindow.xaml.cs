using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using CoursesManager.UI.Resources;

namespace CoursesManager.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // var resizer = new WindowResizer(this);
            Loaded += (s, e) =>
            {
                var window = Window.GetWindow(this);
                if (window != null)
                {
                    window.StateChanged += (sender, args) =>
                    {
                        WindowState = window.WindowState;
                    };
                }
            };
        }

        public static readonly DependencyProperty WindowStateProperty =
            DependencyProperty.Register("WindowState", typeof(WindowState), typeof(MainWindow), new PropertyMetadata(WindowState.Normal));

        public WindowState WindowState
        {
            get => (WindowState)GetValue(WindowStateProperty);
            set => SetValue(WindowStateProperty, value);
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        public static Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);

            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window is null) return;

            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 1)
            {
                if (window.WindowState == WindowState.Maximized)
                {
                    var mousePos = GetMousePosition();

                    var xRatio = e.GetPosition(window).X / window.ActualWidth;
                    var yRatio = e.GetPosition(window).Y / window.ActualHeight;

                    window.WindowState = WindowState.Normal;

                    window.Left = mousePos.X - (window.ActualWidth * xRatio);
                    window.Top = mousePos.Y - (window.ActualHeight * yRatio);
                }

                window.DragMove();
            }
        }

        private void MinimizeWindow_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window is not null) window.WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window is null) return;

            window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
    }
}