namespace DocumentPacker.ApplicationInit.Configuration;

/// <summary>
///     Initialize the application configuration.
/// </summary>
public interface IAppConfigurationInitializer
{
    /// <summary>
    ///     Initializes the application configuration.
    /// </summary>
    /// <returns>An initialized <seealso cref="IAppConfiguration" /> instance.</returns>
    /// <exception cref="InvalidOperationException">Cannot initialize {nameof(AppConfiguration)}.</exception>
    IAppConfiguration Init();
}
