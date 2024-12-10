namespace DocumentPacker.Mvvm;

using System.Windows.Threading;

/// <summary>
///     A wrapper for <see cref="Dispatcher" />.
/// </summary>
/// <seealso cref="IDispatcher" />
internal class ThreadDispatcher : IDispatcher
{
    /// <summary>
    ///     The ui thread dispatcher.
    /// </summary>
    private readonly Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

    /// <summary>
    ///     Checks that the calling thread has access to this object.
    /// </summary>
    /// <remarks>
    ///     Only the dispatcher thread may access DispatcherObjects.
    ///     <p />
    ///     This method is public so that any thread can probe to
    ///     see if it has access to the DispatcherObject.
    /// </remarks>
    /// <returns>
    ///     True if the calling thread has access to this object.
    /// </returns>
    public bool CheckAccess()
    {
        return this.dispatcher.CheckAccess();
    }

    /// <summary>
    ///     Executes the specified Action synchronously on the thread that
    ///     the Dispatcher was created on.
    /// </summary>
    /// <param name="callback">
    ///     An Action delegate to invoke through the dispatcher.
    /// </param>
    /// <remarks>
    ///     Note that the default priority is DispatcherPriority.Send.
    /// </remarks>
    public void Invoke(Action callback)
    {
        this.dispatcher.Invoke(callback);
    }
}
