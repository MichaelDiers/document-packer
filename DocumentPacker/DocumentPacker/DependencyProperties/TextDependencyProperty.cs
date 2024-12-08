namespace DocumentPacker.DependencyProperties;

using System.Windows;

/// <summary>
///     Attached dependency property for additional text.
/// </summary>
public static class TextDependencyProperty
{
    /// <summary>
    ///     The text property.
    /// </summary>
    public static DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
        "Text",
        typeof(string),
        typeof(TextDependencyProperty),
        new PropertyMetadata(null));

    /// <summary>
    ///     Gets the text.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns>The text.</returns>
    public static string GetText(DependencyObject obj)
    {
        return (string) obj.GetValue(TextDependencyProperty.TextProperty);
    }

    /// <summary>
    ///     Sets the text.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="value">The value.</param>
    public static void SetText(DependencyObject obj, string value)
    {
        obj.SetValue(
            TextDependencyProperty.TextProperty,
            value);
    }
}
