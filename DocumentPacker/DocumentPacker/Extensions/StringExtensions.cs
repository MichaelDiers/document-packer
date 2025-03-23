namespace DocumentPacker.Extensions;

/// <summary>
///     Extensions for <see cref="string" />.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     Convert an image filename to the pack syntax.
    /// </summary>
    /// <param name="image">The name of the image file.</param>
    /// <returns>A new <see cref="string" />.</returns>
    public static string ToPackImage(this string image)
    {
        return $"pack://application:,,,/DocumentPacker;component/Assets/{image}";
    }
}
