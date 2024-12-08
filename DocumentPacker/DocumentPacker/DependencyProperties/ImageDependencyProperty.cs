namespace DocumentPacker.DependencyProperties;

using System.Windows;

/// <summary>
///     Attached dependency property for additional image.
/// </summary>
public static class ImageDependencyProperty
{
    /// <summary>
    ///     The image property.
    /// </summary>
    public static DependencyProperty ImageProperty = DependencyProperty.RegisterAttached(
        "Image",
        typeof(string),
        typeof(ImageDependencyProperty),
        new PropertyMetadata(null));

    /// <summary>
    ///     Gets the image.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns>The path to the image.</returns>
    public static string GetImage(DependencyObject obj)
    {
        return (string) obj.GetValue(ImageDependencyProperty.ImageProperty);
    }

    /// <summary>
    ///     Sets the image.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="value">The value.</param>
    public static void SetImage(DependencyObject obj, string value)
    {
        obj.SetValue(
            ImageDependencyProperty.ImageProperty,
            value);
    }
}
