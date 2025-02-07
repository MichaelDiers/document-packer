namespace DocumentPacker.Converters;

using System.Globalization;
using System.Windows;
using System.Windows.Data;

/// <summary>
///     Converter from <see cref="bool" /> to <see cref="Visibility" />.
/// </summary>
/// <seealso cref="IValueConverter" />
public class BoolToVisibilityConverter : IValueConverter
{
    /// <summary>Converts a value.</summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    public object Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        if (value == null)
        {
            return Visibility.Collapsed;
        }

        try
        {
            var boolValue = (bool) value;
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }
        catch (InvalidCastException)
        {
            return Visibility.Collapsed;
        }
    }

    /// <summary>Converts a value.</summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    /// <exception cref="NotImplementedException">Method is not implemented.</exception>
    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }
}
