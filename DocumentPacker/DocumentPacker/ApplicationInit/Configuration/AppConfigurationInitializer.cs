namespace DocumentPacker.ApplicationInit.Configuration;

using System.IO;
using Microsoft.Extensions.Configuration;

/// <summary>
///     Initialize the application configuration.
/// </summary>
/// <seealso cref="IAppConfigurationInitializer" />
internal class AppConfigurationInitializer : IAppConfigurationInitializer
{
    /// <summary>
    ///     The name of the application settings json file.
    /// </summary>
    private const string AppSettingsJson = "appsettings.json";

    /// <summary>
    ///     Initializes the application configuration.
    /// </summary>
    /// <returns>An initialized <seealso cref="IAppConfiguration" /> instance.</returns>
    /// <exception cref="InvalidOperationException">Cannot initialize {nameof(AppConfiguration)}.</exception>
    public IAppConfiguration Init()
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(
                AppConfigurationInitializer.AppSettingsJson,
                false,
                false);

        var configuration = builder.Build();
        return configuration.Get<AppConfiguration>() ??
               throw new InvalidOperationException($"Cannot initialize {nameof(AppConfiguration)}.");
    }
}
