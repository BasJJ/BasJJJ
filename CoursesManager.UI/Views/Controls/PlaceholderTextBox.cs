using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace CoursesManager.UI.Views.Controls;

public class PlaceholderTextBox : TextBox
{
    private BindingExpression? _bindingExpression;
    private bool _isShowingPlaceholder;

    public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register(
            nameof(Placeholder),
            typeof(string),
            typeof(PlaceholderTextBox),
            new PropertyMetadata(string.Empty));

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public static readonly DependencyProperty PlaceholderBrushProperty =
        DependencyProperty.Register(
            nameof(PlaceholderBrush),
            typeof(Brush),
            typeof(PlaceholderTextBox),
            new PropertyMetadata(Brushes.Gray));

    public Brush PlaceholderBrush
    {
        get => (Brush)GetValue(PlaceholderBrushProperty);
        set => SetValue(PlaceholderBrushProperty, value);
    }

    public PlaceholderTextBox()
    {
        Loaded += OnLoaded;
        GotFocus += RemovePlaceholder;
        LostFocus += ShowPlaceholderIfEmpty;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        ShowPlaceholderIfEmpty(this, e);
    }

    private void ShowPlaceholderIfEmpty(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(Text))
        {
            ShowPlaceholder();
        }
    }

    private void ShowPlaceholder()
    {
        _bindingExpression = GetBindingExpression(TextProperty);
        if (_bindingExpression != null)
        {
            BindingOperations.ClearBinding(this, TextProperty);
        }

        _isShowingPlaceholder = true;
        Text = Placeholder;
        Foreground = PlaceholderBrush;
    }

    private void RemovePlaceholder(object sender, RoutedEventArgs e)
    {
        if (!_isShowingPlaceholder) return;

        _isShowingPlaceholder = false;
        Text = string.Empty;

        Foreground = Brushes.Black;

        if (_bindingExpression != null)
        {
            SetBinding(TextProperty, _bindingExpression.ParentBinding);
        }
    }
}