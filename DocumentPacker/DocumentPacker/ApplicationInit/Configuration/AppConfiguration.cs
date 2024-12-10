namespace DocumentPacker.ApplicationInit.Configuration;

/// <summary>
///     The application configuration specified by the <code>appsettings.json</code> file.
/// </summary>
/// <seealso cref="IAppConfiguration" />
internal class AppConfiguration : IAppConfiguration
{
    /// <summary>
    ///     Gets or sets the application icon.
    /// </summary>
    public string Icon { get; set; } = "";

    /// <summary>
    ///     Gets or sets the title.
    /// </summary>
    public string Title { get; set; } = "";

    /// <summary>
    ///     Gets or sets the version.
    /// </summary>
    public string Version { get; set; } = "";
}
