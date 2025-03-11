namespace DocumentPacker.Converters;

using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Libs.Wpf.ViewModels;

/// <summary>
///     Converts a <see cref="TranslatableAndValidable{TValue}" /> of <see cref="string" /> to <see cref="Visibility" />.
/// </summary>
internal class TranslatableAndValidableStringToVisibilityConverter : IValueConverter
{
    /// <summary>Converts a value.</summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    public object? Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        return value is TranslatableAndValidable<string> data && !string.IsNullOrWhiteSpace(data.Value)
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    /// <summary>Converts a value.</summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    public object? ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    )
    {
        throw new NotImplementedException();
    }
}
