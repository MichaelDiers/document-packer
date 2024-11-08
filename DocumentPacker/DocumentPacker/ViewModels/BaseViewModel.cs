namespace DocumentPacker.ViewModels;

using System.ComponentModel;
using System.Runtime.CompilerServices;

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
}
