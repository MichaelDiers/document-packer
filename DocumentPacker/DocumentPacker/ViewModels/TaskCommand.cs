namespace DocumentPacker.ViewModels;

using System.Windows.Input;
using DocumentPacker.Contracts.ViewModels;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     An <see cref="ICommand" />  implementation that runs the <see cref="ICommand.Execute" />
///     operation in a separate background <see cref="Task" /> and support cancellation.
/// </summary>
/// <seealso cref="System.Windows.Input.ICommand" />
public class TaskCommand : ICommand
{
    /// <summary>
    ///     The global cancel command that cancels all running commands.
    /// </summary>
    private static TaskCommand? cancelCommand;

    /// <summary>
    ///     The <see cref="CancellationTokenSource" /> reinatialized if a <see cref="ICommand.Execute" />
    ///     is started and the <see cref="numberOfExecutingCommands" /> is equal to zero.
    /// </summary>
    private static CancellationTokenSource? cancellationTokenSource;

    /// <summary>
    ///     The lock object used for updating <see cref="numberOfExecutingCommands" />.
    /// </summary>
    private static readonly object LockObject = new();

    /// <summary>
    ///     The number of executing background tasks.
    /// </summary>
    private static int numberOfExecutingCommands;

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
    private readonly Func<object?, CancellationToken, Task> execute;

    /// <summary>
    ///     Specifies if the command is a cancel command. Used in <see cref="ICommand.CanExecute" />.
    /// </summary>
    private readonly bool isCancelCommand;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TaskCommand" /> class.
    /// </summary>
    /// <param name="canExecute">Defines the method that determines whether the command can execute in its current state.</param>
    /// <param name="execute">Defines the method to be called when the command is invoked.</param>
    /// <param name="isCancelCommand">if set to <c>true</c> the command is used for the cancellation of another command.</param>
    /// <param name="dispatcher">A dispatcher for the ui thread.</param>
    public TaskCommand(
        Func<object?, bool> canExecute,
        Func<object?, CancellationToken, Task> execute,
        bool isCancelCommand,
        IDispatcher? dispatcher = null
    )
    {
        this.canExecute = canExecute;
        this.execute = execute;
        this.isCancelCommand = isCancelCommand;
        this.dispatcher = dispatcher ?? App.ServiceProvider.GetRequiredService<IDispatcher>();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TaskCommand" /> class.
    /// </summary>
    /// <param name="canExecute">Defines the method that determines whether the command can execute in its current state.</param>
    /// <param name="execute">Defines the method to be called when the command is invoked.</param>
    public TaskCommand(Func<object?, bool> canExecute, Func<object?, CancellationToken, Task> execute)
        : this(
            canExecute,
            execute,
            false)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TaskCommand" /> class.
    /// </summary>
    /// <param name="execute">Defines the method to be called when the command is invoked.</param>
    public TaskCommand(Func<object?, CancellationToken, Task> execute)
        : this(
            _ => true,
            execute,
            false)
    {
    }

    /// <summary>
    ///     Gets the cancel command that cancels all <see cref="ICommand.Execute" /> that are
    ///     associated to <see cref="cancellationTokenSource" />.
    /// </summary>
    public static TaskCommand? CancelCommand
    {
        get => TaskCommand.cancelCommand;
        private set
        {
            TaskCommand.cancelCommand = value;
            TaskCommand.CancelCommandChanged?.Invoke(
                null,
                EventArgs.Empty);
        }
    }

    /// <summary>
    ///     Gets a value indicating whether this instance is executing commands.
    /// </summary>
    /// <value>
    ///     <c>true</c> if this instance is executing commands; otherwise, <c>false</c>.
    /// </value>
    public static bool IsExecutingCommands => TaskCommand.numberOfExecutingCommands != 0;

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
        return (TaskCommand.numberOfExecutingCommands == 0 || this.isCancelCommand) && this.canExecute(parameter);
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
        if (this.dispatcher.CheckAccess())
        {
            this.PreExecute();
        }
        else
        {
            this.dispatcher.Invoke(this.PreExecute);
        }

        Task.Run(
                () => this.execute(
                    parameter,
                    TaskCommand.cancellationTokenSource?.Token ?? CancellationToken.None))
            .ContinueWith(
                task =>
                {
                    this.dispatcher.Invoke(
                        () =>
                        {
                            lock (TaskCommand.LockObject)
                            {
                                TaskCommand.numberOfExecutingCommands--;
                                if (TaskCommand.numberOfExecutingCommands == 0)
                                {
                                    CommandManager.InvalidateRequerySuggested();
                                    TaskCommand.IsExecutingCommandsChanged?.Invoke(
                                        null,
                                        EventArgs.Empty);
                                    TaskCommand.cancellationTokenSource = null;
                                    TaskCommand.CancelCommand = null;
                                }
                            }

                            if (task is not {IsFaulted: true, Exception: not null})
                            {
                                return;
                            }

                            // todo: user friendly error message
                            var message = task.Exception.Flatten()
                                .InnerExceptions.Select(ex => ex.Message)
                                .Aggregate((message1, message2) => $"{message1} {message2}");
                            TaskCommand.FatalError?.Invoke(
                                this,
                                message);
                        });
                });
    }

    /// <summary>
    ///     Occurs when <see cref="CancelCommand" /> changed.
    /// </summary>
    public static event EventHandler? CancelCommandChanged;

    /// <summary>
    ///     Occurs when <see cref="CancelCommand" /> changed.
    /// </summary>
    public static event EventHandler<string>? FatalError;

    /// <summary>
    ///     Occurs when <see cref="IsExecutingCommands" /> changed.
    /// </summary>
    public static event EventHandler? IsExecutingCommandsChanged;

    /// <summary>
    ///     Is executed as the first step of <see cref="ICommand.Execute" />.
    /// </summary>
    private void PreExecute()
    {
        lock (TaskCommand.LockObject)
        {
            TaskCommand.numberOfExecutingCommands++;

            if (TaskCommand.numberOfExecutingCommands != 1)
            {
                return;
            }

            // check for can execute changes
            CommandManager.InvalidateRequerySuggested();

            // update the dependent isExecutingCommands
            TaskCommand.IsExecutingCommandsChanged?.Invoke(
                null,
                EventArgs.Empty);

            // init the token source for cancellation operations
            TaskCommand.cancellationTokenSource = new CancellationTokenSource();

            TaskCommand.CancelCommand = new TaskCommand(
                _ => TaskCommand.IsExecutingCommands,
                (_, _) =>
                {
                    TaskCommand.cancellationTokenSource?.Cancel();
                    return Task.CompletedTask;
                },
                true,
                this.dispatcher);
        }
    }
}
