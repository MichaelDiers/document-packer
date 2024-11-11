namespace DocumentPacker.ViewModels;

using System.ComponentModel;
using System.Runtime.CompilerServices;

/// <summary>
///     Basic view model implementation.
/// </summary>
/// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
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
