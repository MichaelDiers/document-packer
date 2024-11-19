namespace DocumentPacker.Contracts;

/// <summary>
///     The identifier of a sub view.
/// </summary>
public enum SubViewId
{
    /// <summary>
    ///     Undefined value.
    /// </summary>
    None = 0,

    /// <summary>
    ///     The collect documents view.
    /// </summary>
    CollectDocuments,

    /// <summary>
    ///     The load configuration view.
    /// </summary>
    LoadConfiguration,

    /// <summary>
    ///     The startup view.
    /// </summary>
    StartUp
}
