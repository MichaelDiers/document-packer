namespace DocumentPacker.ApplicationInit.Configuration;

/// <summary>
///     The application configuration specified by the <code>appsettings.json</code> file.
/// </summary>
public interface IAppConfiguration
{
    /// <summary>
    ///     Gets the application icon.
    /// </summary>
    string Icon { get; }

    /// <summary>
    ///     Gets the version.
    /// </summary>
    string Version { get; }
}
