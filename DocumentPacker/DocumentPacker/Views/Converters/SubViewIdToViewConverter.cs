namespace DocumentPacker.Views.Converters;

using System.Globalization;
using System.Windows.Data;
using DocumentPacker.Contracts;
using DocumentPacker.Views.SubViews;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     A converter from <see cref="SubViewId" /> to view converter.
/// </summary>
/// <seealso cref="System.Windows.Data.IValueConverter" />
public class SubViewIdToViewConverter : IValueConverter
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
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (!Enum.TryParse<SubViewId>(
                value.ToString(),
                out var subViewId) ||
            subViewId == SubViewId.None)
        {
            throw new ArgumentException(
                $"Unknown {nameof(SubViewId)}: {value}",
                nameof(value));
        }

        return subViewId switch
        {
            SubViewId.CollectDocuments => App.ServiceProvider.GetRequiredService<CollectDocumentsView>(),
            SubViewId.CreateConfiguration => App.ServiceProvider.GetRequiredService<CreateConfigurationView>(),
            SubViewId.LoadConfiguration => App.ServiceProvider.GetRequiredService<LoadConfigurationView>(),
            SubViewId.StartUp => App.ServiceProvider.GetRequiredService<StartUpView>(),
            _ => throw new ArgumentException(
                $"Unsupported {nameof(SubViewId)}: {value}",
                nameof(value))
        };
    }

    /// <summary>Converts a value.</summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    /// <exception cref="NotImplementedException">The method is not implemented.</exception>
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
