namespace DocumentPacker.Mvvm;

using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     An <see cref="ICommand" />  implementation that runs in the ui thread.
/// </summary>
/// <seealso cref="System.Windows.Input.ICommand" />
public class SyncCommand : ICommand
{
    /// <summary>
    ///     Defines the method that determines whether the command can execute in its current state.
    /// </summary>
    private readonly Func<object?, bool> canExecute;

    /// <summary>
    ///     A dispatcher for the ui thread.
    /// </summary>
    private readonly IDispatcher dispatcher;

    /// <summary>
    ///     Defines the method to be called when the command is invoked.
    /// </summary>
    private readonly Action<object?> execute;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TaskCommand" /> class.
    /// </summary>
    /// <param name="canExecute">Defines the method that determines whether the command can execute in its current state.</param>
    /// <param name="execute">Defines the method to be called when the command is invoked.</param>
    /// <param name="dispatcher">A dispatcher for the ui thread.</param>
    public SyncCommand(Func<object?, bool> canExecute, Action<object?> execute, IDispatcher? dispatcher = null)
    {
        this.canExecute = canExecute;
        this.execute = execute;
        this.dispatcher = dispatcher ?? App.ServiceProvider.GetRequiredService<IDispatcher>();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TaskCommand" /> class.
    /// </summary>
    /// <param name="execute">Defines the method to be called when the command is invoked.</param>
    public SyncCommand(Action<object?> execute)
        : this(
            _ => true,
            execute)
    {
    }

    /// <summary>Defines the method that determines whether the command can execute in its current state.</summary>
    /// <param name="parameter">
    ///     Data used by the command.  If the command does not require data to be passed, this object can
    ///     be set to <see langword="null" />.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.
    /// </returns>
    public virtual bool CanExecute(object? parameter)
    {
        return this.canExecute(parameter);
    }

    /// <summary>Occurs when changes occur that affect whether or not the command should execute.</summary>
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    /// <summary>Defines the method to be called when the command is invoked.</summary>
    /// <param name="parameter">
    ///     Data used by the command.  If the command does not require data to be passed, this object can
    ///     be set to <see langword="null" />.
    /// </param>
    public void Execute(object? parameter)
    {
        try
        {
            if (this.dispatcher.CheckAccess())
            {
                this.execute(parameter);
            }
            else
            {
                this.dispatcher.Invoke(() => this.execute(parameter));
            }
        }
        catch (Exception e)
        {
            // todo: user friendly error message
            SyncCommand.FatalError?.Invoke(
                this,
                e.Message);
        }
    }

    /// <summary>
    ///     Occurs when <see cref="FatalError" /> occurs.
    /// </summary>
    public static event EventHandler<string>? FatalError;
}
