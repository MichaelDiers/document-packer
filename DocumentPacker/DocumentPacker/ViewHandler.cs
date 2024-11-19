namespace DocumentPacker;

using DocumentPacker.Contracts;

/// <summary>
///     Handles view related callbacks.
/// </summary>
internal class ViewHandler : IViewHandler
{
    private readonly IDictionary<SubViewId, List<Action<object?>>> registered =
        new Dictionary<SubViewId, List<Action<object?>>>();

    /// <summary>
    ///     Registers the view <paramref name="subViewId" /> and its <paramref name="callback" />.
    ///     If the <paramref name="subViewId" /> is requested, the <paramref name="callback" />
    ///     is executed.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the <paramref name="callback" />.</typeparam>
    /// <param name="subViewId">The sub view identifier.</param>
    /// <param name="callback">The callback that is executed if <paramref name="subViewId" /> is requested.</param>
    public void RegisterView<T>(SubViewId subViewId, Action<T?> callback)
    {
        if (!this.registered.TryGetValue(
                subViewId,
                out var callbacks))
        {
            callbacks = new List<Action<object?>>();
            this.registered[subViewId] = callbacks;
        }

        callbacks.Add(obj => callback((T?) obj));
    }

    /// <summary>
    ///     Requests the view <paramref name="subViewId" /> and executes all registered callbacks using parameter
    ///     <paramref name="value" />.
    /// </summary>
    /// <typeparam name="T">The type of the callback parameter <paramref name="value" />.</typeparam>
    /// <param name="subViewId">The sub view identifier.</param>
    /// <param name="value">The registered callback is executed with this <paramref name="value" />.</param>
    public void RequestView<T>(SubViewId subViewId, T? value)
    {
        if (!this.registered.TryGetValue(
                subViewId,
                out var callbacks))
        {
            throw new ArgumentException(
                $"Unknown view {subViewId}.",
                nameof(subViewId));
        }

        callbacks.ForEach(action => action(value));
    }

    /// <summary>
    ///     Requests the view <paramref name="subViewId" /> and executes all registered callbacks null as parameter.
    /// </summary>
    /// <param name="subViewId">The sub view identifier.</param>
    public void RequestView(SubViewId subViewId)
    {
        if (!this.registered.TryGetValue(
                subViewId,
                out var callbacks))
        {
            throw new ArgumentException(
                $"Unknown view {subViewId}.",
                nameof(subViewId));
        }

        callbacks.ForEach(action => action(null));
    }
}
