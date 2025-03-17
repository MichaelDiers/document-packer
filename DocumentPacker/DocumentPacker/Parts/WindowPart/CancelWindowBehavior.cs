namespace DocumentPacker.Parts.WindowPart;

using System.ComponentModel;
using System.Windows;
using Microsoft.Xaml.Behaviors;

public class CancelWindowBehavior : Behavior<Window>
{
    private Window? cancelWindow;

    /// <summary>
    ///     Called after the behavior is attached to an AssociatedObject.
    /// </summary>
    /// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
    protected override void OnAttached()
    {
        base.OnAttached();

        this.AssociatedObject.DataContextChanged += this.OnDataContextChanged;
    }

    protected override void OnDetaching()
    {
        this.AssociatedObject.DataContextChanged -= this.OnDataContextChanged;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is ITranslatableCancellableButton oldTranslatableCancellableButton)
        {
            oldTranslatableCancellableButton.PropertyChanged -= this.OnPropertyChanged;
            if (this.cancelWindow is not null)
            {
                this.cancelWindow.Close();
                this.cancelWindow = null;
            }
        }

        if (e.NewValue is ITranslatableCancellableButton newTranslatableCancellableButton)
        {
            newTranslatableCancellableButton.PropertyChanged += this.OnPropertyChanged;
        }
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!string.Equals(
                e.PropertyName,
                nameof(ITranslatableCancellableButton.TranslatableCancellableButton)))
        {
            return;
        }

        if (Application.Current.Dispatcher.Thread != Thread.CurrentThread)
        {
            Application.Current.Dispatcher.Invoke(
                () => this.OnPropertyChanged(
                    sender,
                    e));
            return;
        }

        if (this.AssociatedObject.DataContext is not ITranslatableCancellableButton translatableCancellableButton)
        {
            return;
        }

        if (this.cancelWindow is not null)
        {
            this.cancelWindow.Close();
            this.cancelWindow = null;
        }

        if (translatableCancellableButton.TranslatableCancellableButton is not null)
        {
            this.cancelWindow = new CancelCommandWindow
            {
                DataContext = translatableCancellableButton.TranslatableCancellableButton,
                Owner = this.AssociatedObject
            };
            this.cancelWindow.Show();
        }
    }
}
