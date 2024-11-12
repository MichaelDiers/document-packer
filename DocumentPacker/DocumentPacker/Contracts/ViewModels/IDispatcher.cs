namespace DocumentPacker.Contracts.ViewModels;

/// <summary>
///     A dispatcher for the ui thread.
/// </summary>
public interface IDispatcher
{
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
    bool CheckAccess();

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
    void Invoke(Action callback);
}
