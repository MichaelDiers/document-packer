namespace DocumentPacker.Mvvm;

using System.ComponentModel;
using System.Runtime.CompilerServices;

/// <summary>
///     Basic view model implementation.
/// </summary>
/// <seealso cref="INotifyPropertyChanged" />
internal class BaseViewModel : INotifyPropertyChanged
{
    /// <summary>
    ///     Occurs when a property value changed.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    ///     Called when a property changed.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    ///     Sets the field <paramref name="field" /> to <paramref name="value" /> and raises <see cref="PropertyChanged" />.
    /// </summary>
    /// <typeparam name="T">The type of the <paramref name="field" />.</typeparam>
    /// <param name="field">The field to be set.</param>
    /// <param name="value">The new value of <paramref name="field" />.</param>
    /// <param name="propertyName">Name of the property.</param>
    protected void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(
                field,
                value))
        {
            return;
        }

        field = value;
        this.OnPropertyChanged(propertyName);
    }
}
