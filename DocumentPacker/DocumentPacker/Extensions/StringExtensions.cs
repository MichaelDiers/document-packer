namespace DocumentPacker.Extensions;

using System.Windows.Media.Imaging;

/// <summary>
///     Extensions for <see cref="string" />.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     Convert a <see cref="string" /> to a <see cref="BitmapImage" />.
    /// </summary>
    /// <param name="image">The name of the image file.</param>
    /// <returns>A new <see cref="BitmapImage" />.</returns>
    public static BitmapImage ToBitmapImage(this string image)
    {
        return new BitmapImage(
            new Uri(
                $"pack://application:,,,/DocumentPacker;component/Assets/{image}",
                UriKind.Absolute));
    }
}
