namespace DocumentPacker.Parts.Main.EncryptPart;

using System.Globalization;
using System.Windows.Data;
using DocumentPacker.Resources;

internal class EncryptItemTypeToStringConverter : IValueConverter
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
        var suppressNone = parameter as bool? ?? false;

        if (value is EncryptItemType encryptItemType)
        {
            return this.Convert(
                encryptItemType,
                suppressNone);
        }

        throw new ArgumentException(
            string.Empty,
            nameof(value));
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

    private string Convert(EncryptItemType encryptItemType, bool suppressNone)
    {
        if (suppressNone && encryptItemType == EncryptItemType.None)
        {
            return string.Empty;
        }

        return Translation.ResourceManager.GetString(
                   $"{nameof(EncryptItemType)}{encryptItemType.ToString()}",
                   Translation.Culture) ??
               encryptItemType.ToString();
    }
}
