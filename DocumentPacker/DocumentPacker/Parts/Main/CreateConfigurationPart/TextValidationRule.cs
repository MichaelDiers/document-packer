namespace DocumentPacker.Parts.Main.CreateConfigurationPart;

using System.Globalization;
using System.Windows.Controls;

internal class TextValidationRule : ValidationRule
{
    /// <summary>When overridden in a derived class, performs validation checks on a value.</summary>
    /// <param name="value">The value from the binding target to check.</param>
    /// <param name="cultureInfo">The culture to use in this rule.</param>
    /// <returns>A <see cref="T:System.Windows.Controls.ValidationResult" /> object.</returns>
    public override ValidationResult Validate(object? value, CultureInfo cultureInfo)
    {
        return value is string s && !string.IsNullOrWhiteSpace(s)
            ? ValidationResult.ValidResult
            : new ValidationResult(
                false,
                "The error");
    }
}
