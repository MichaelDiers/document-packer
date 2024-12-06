namespace DocumentPacker.ApplicationInit.Configuration;

/// <summary>
///     The application configuration specified by the <code>appsettings.json</code> file.
/// </summary>
public interface IAppConfiguration
{
    /// <summary>
    ///     Gets the title.
    /// </summary>
    string Title { get; }

    /// <summary>
    ///     Gets the version.
    /// </summary>
    string Version { get; }
}
