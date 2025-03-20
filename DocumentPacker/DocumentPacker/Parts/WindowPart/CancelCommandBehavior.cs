namespace DocumentPacker.Parts.WindowPart;

using System.Windows;
using DocumentPacker.Commands;
using Microsoft.Xaml.Behaviors;

/// <summary>
///     A <see cref="Behavior{T}" /> to handle cancel commands.
/// </summary>
public class CancelCommandBehavior : Behavior<Window>
{
    /// <summary>
    ///     The <see cref="Window" /> that displays the cancel command.
    /// </summary>
    private Window? cancelWindow;

    /// <summary>
    ///     Called after the behavior is attached to an AssociatedObject.
    /// </summary>
    /// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
    protected override void OnAttached()
    {
        base.OnAttached();

        this.AssociatedObject.DataContextChanged += this.OnWindowDataContextChanged;
    }

    /// <summary>
    ///     Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
    /// </summary>
    /// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
    protected override void OnDetaching()
    {
        this.AssociatedObject.DataContextChanged += this.OnWindowDataContextChanged;

        base.OnDetaching();
    }

    /// <summary>
    ///     Adds a handler for the <see cref="ICommandSync.CommandSyncChanged" /> event.
    /// </summary>
    /// <param name="dataContext">The data context of the <see cref="Behavior{T}.AssociatedObject" />.</param>
    private void AddCommandSyncChangedHandler(object dataContext)
    {
        if (dataContext is not ICancelCommandViewModel cancelCommandViewModel)
        {
            return;
        }

        cancelCommandViewModel.CommandSync.CommandSyncChanged += this.OnCommandSyncChanged;
        cancelCommandViewModel.IsCommandActive = false;
        cancelCommandViewModel.IsWindowEnabled = !cancelCommandViewModel.IsCommandActive;
    }

    /// <summary>
    ///     The handler of the <see cref="ICommandSync.CommandSyncChanged" /> event.
    /// </summary>
    /// <param name="sender">The sender that raised the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> of the raised event.</param>
    private void OnCommandSyncChanged(object? sender, CommandSyncChangedEventArgs e)
    {
        if (Application.Current.Dispatcher.Thread != Thread.CurrentThread)
        {
            Application.Current.Dispatcher.Invoke(
                () => this.OnCommandSyncChanged(
                    sender,
                    e));
            return;
        }

        if (this.AssociatedObject.DataContext is not ICancelCommandViewModel cancelCommandViewModel)
        {
            return;
        }

        cancelCommandViewModel.IsCommandActive = e.IsCommandActive;
        cancelCommandViewModel.IsWindowEnabled = !cancelCommandViewModel.IsCommandActive;

        cancelCommandViewModel.TranslatableCancellableButton =
            e is {IsCommandActive: true, TranslatableCancellableButton: not null}
                ? e.TranslatableCancellableButton
                : null;

        if (this.cancelWindow is not null)
        {
            this.cancelWindow.Close();
            this.cancelWindow = null;
        }

        if (cancelCommandViewModel.TranslatableCancellableButton is null)
        {
            return;
        }

        this.cancelWindow = new CancelCommandWindow
        {
            DataContext = cancelCommandViewModel.TranslatableCancellableButton,
            Owner = this.AssociatedObject
        };

        this.cancelWindow.Show();
    }

    /// <summary>
    ///     A handler for the <see cref="FrameworkElement.DataContextChanged" /> event.
    /// </summary>
    /// <param name="sender">The sender that raised the event.</param>
    /// <param name="e">The <see cref="EventArgs" /> of the event.</param>
    private void OnWindowDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        this.RemoveCommandSyncChangedHandler(e.OldValue);
        this.AddCommandSyncChangedHandler(e.NewValue);
    }

    /// <summary>
    ///     Removes a handler for the <see cref="ICommandSync.CommandSyncChanged" /> event.
    /// </summary>
    /// <param name="dataContext">The data context of the <see cref="Behavior{T}.AssociatedObject" />.</param>
    private void RemoveCommandSyncChangedHandler(object dataContext)
    {
        if (dataContext is not ICancelCommandViewModel cancelCommandViewModel)
        {
            return;
        }

        cancelCommandViewModel.CommandSync.CommandSyncChanged -= this.OnCommandSyncChanged;
        cancelCommandViewModel.IsCommandActive = false;
        cancelCommandViewModel.IsWindowEnabled = !cancelCommandViewModel.IsCommandActive;
        cancelCommandViewModel.TranslatableCancellableButton = null;
    }
}
