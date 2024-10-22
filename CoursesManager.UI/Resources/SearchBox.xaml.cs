﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CoursesManager.UI.Resources
{
    public partial class SearchBox : UserControl
    {
        public SearchBox()
        {
            InitializeComponent();
            SearchTextBox.KeyDown += SearchTextBox_KeyDown; // Handle Enter key for search
        }

        // Dependency property for Text binding
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(SearchBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        // Dependency property for SearchCommand binding
        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register(nameof(SearchCommand), typeof(ICommand), typeof(SearchBox),
                new PropertyMetadata(null));

        public ICommand SearchCommand
        {
            get => (ICommand)GetValue(SearchCommandProperty);
            set => SetValue(SearchCommandProperty, value);
        }

        // Handle Enter key press to execute SearchCommand
        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && SearchCommand != null && SearchCommand.CanExecute(null))
            {
                SearchCommand.Execute(null);
            }
        }

        // Button click event handler to execute SearchCommand
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchCommand != null && SearchCommand.CanExecute(null))
            {
                SearchCommand.Execute(null);
            }
        }
    }
}