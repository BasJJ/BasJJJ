using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CoursesManager.UI.Resources;

public class PlaceholderTextBox : TextBox
{
    public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register(
            nameof(Placeholder),
            typeof(string),
            typeof(PlaceholderTextBox),
            new PropertyMetadata(string.Empty, OnPlaceholderChanged));

    public string Placeholder
    {
        get { return (string)GetValue(PlaceholderProperty); }
        set { SetValue(PlaceholderProperty, value); }
    }

    public static readonly DependencyProperty PlaceholderBrushProperty =
        DependencyProperty.Register(
            nameof(PlaceholderBrush),
            typeof(Brush),
            typeof(PlaceholderTextBox),
            new PropertyMetadata(Brushes.Gray));

    public Brush PlaceholderBrush
    {
        get { return (Brush)GetValue(PlaceholderBrushProperty); }
        set { SetValue(PlaceholderBrushProperty, value); }
    }

    private static void OnPlaceholderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var textBox = (PlaceholderTextBox)d;
        textBox.SetPlaceholder();
    }

    public PlaceholderTextBox()
    {
        Foreground = Brushes.Black;

        GotFocus += RemovePlaceholder;
        LostFocus += SetPlaceholder;
    }

    private void SetPlaceholder(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(Text))
        {
            Text = Placeholder;
            Foreground = PlaceholderBrush;
        }
    }

    private void RemovePlaceholder(object sender, RoutedEventArgs e)
    {
        if (Text == Placeholder)
        {
            Text = string.Empty;

            Foreground = Brushes.Black;
        }
    }

    private void SetPlaceholder()
    {
        if (string.IsNullOrEmpty(Text) && !IsFocused)
        {
            Text = Placeholder;
            Foreground = PlaceholderBrush;
        }
    }
}