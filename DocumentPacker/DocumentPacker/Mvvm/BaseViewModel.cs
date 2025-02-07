namespace DocumentPacker.Mvvm;

using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

/// <summary>
///     Basic view model implementation.
/// </summary>
/// <seealso cref="INotifyPropertyChanged" />
internal class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
{
    /// <summary>
    ///     Stores the error state of the model.
    /// </summary>
    private readonly IDictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

    /// <summary>Gets a value that indicates whether the entity has validation errors.</summary>
    /// <returns>
    ///     <see langword="true" /> if the entity currently has validation errors; otherwise, <see langword="false" />.
    /// </returns>
    public bool HasErrors => this.errors.Count != 0;

    /// <summary>Occurs when the validation errors have changed for a property or for the entire entity.</summary>
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    /// <summary>Gets the validation errors for a specified property or for the entire entity.</summary>
    /// <param name="propertyName">
    ///     The name of the property to retrieve validation errors for; or <see langword="null" /> or
    ///     <see cref="F:System.String.Empty" />, to retrieve entity-level errors.
    /// </param>
    /// <returns>The validation errors for the property or entity.</returns>
    public IEnumerable GetErrors(string? propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            foreach (var propertyErrors in this.errors.Values)
            {
                foreach (var propertyError in propertyErrors)
                {
                    yield return propertyError;
                }
            }
        }
        else
        {
            if (!this.errors.TryGetValue(
                    propertyName,
                    out var propertyErrors))
            {
                yield break;
            }

            foreach (var propertyError in propertyErrors)
            {
                yield return propertyError;
            }
        }
    }

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
    protected void SetField<T>(
        ref T field,
        T value,
        IEnumerable<Func<(string propertyName, IEnumerable<string> errors)>> validators,
        [CallerMemberName] string? propertyName = null
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);

        this.SetField(
            ref field,
            value,
            propertyName);

        this.Validate(validators);
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

    private void Validate(IEnumerable<Func<(string propertyName, IEnumerable<string> errors)>> validators)
    {
        foreach (var validator in validators)
        {
            var validationResult = validator();
            var propertyName = validationResult.propertyName;
            var validationErrors = validationResult.errors.ToList();

            var removed = this.errors.Remove(propertyName);

            if (validationErrors.Count > 0)
            {
                this.errors.Add(
                    propertyName,
                    validationErrors);
            }

            if (removed || validationErrors.Count > 0)
            {
                this.ErrorsChanged?.Invoke(
                    this,
                    new DataErrorsChangedEventArgs(propertyName));
            }
        }
    }
}
