namespace DocumentPacker.Contracts;

/// <summary>
///     Handles view related callbacks.
/// </summary>
internal interface IViewHandler
{
    /// <summary>
    ///     Registers the view <paramref name="subViewId" /> and its <paramref name="callback" />.
    ///     If the <paramref name="subViewId" /> is requested, the <paramref name="callback" />
    ///     is executed.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the <paramref name="callback" />.</typeparam>
    /// <param name="subViewId">The sub view identifier.</param>
    /// <param name="callback">The callback that is executed if <paramref name="subViewId" /> is requested.</param>
    void RegisterView<T>(SubViewId subViewId, Action<T?> callback);

    /// <summary>
    ///     Requests the view <paramref name="subViewId" /> and executes all registered callbacks using parameter
    ///     <paramref name="value" />.
    /// </summary>
    /// <typeparam name="T">The type of the callback parameter <paramref name="value" />.</typeparam>
    /// <param name="subViewId">The sub view identifier.</param>
    /// <param name="value">The registered callback is executed with this <paramref name="value" />.</param>
    void RequestView<T>(SubViewId subViewId, T? value);

    /// <summary>
    ///     Requests the view <paramref name="subViewId" /> and executes all registered callbacks null as parameter.
    /// </summary>
    /// <param name="subViewId">The sub view identifier.</param>
    void RequestView(SubViewId subViewId);
}
