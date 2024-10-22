using System.Windows;

namespace CoursesManager.UI.Resources;

public static class TextBoxHelper
{
    public static readonly DependencyProperty PlaceholderTextProperty =
        DependencyProperty.RegisterAttached(
            "PlaceholderText",
            typeof(string),
            typeof(TextBoxHelper),
            new PropertyMetadata(string.Empty));

    public static string GetPlaceholderText(DependencyObject obj)
    {
        return (string)obj.GetValue(PlaceholderTextProperty);
    }

    public static void SetPlaceholderText(DependencyObject obj, string value)
    {
        obj.SetValue(PlaceholderTextProperty, value);
    }
}