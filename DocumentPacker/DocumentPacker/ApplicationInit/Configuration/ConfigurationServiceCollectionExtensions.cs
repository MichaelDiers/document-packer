namespace DocumentPacker.ApplicationInit.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
///     Extensions for <see cref="IServiceCollection" /> for adding the application configuration.
/// </summary>
public static class ConfigurationServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the <seealso cref="IAppConfigurationInitializer" /> as a dependency.
    /// </summary>
    /// <param name="services">Dependencies are added to the given services.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection AddAppConfigurationInitializer(this IServiceCollection services)
    {
        services.TryAddSingleton<IAppConfigurationInitializer, AppConfigurationInitializer>();

        return services;
    }

    /// <summary>
    ///     Adds the <seealso cref="IAppConfiguration" /> as a dependency.
    /// </summary>
    /// <param name="services">Dependencies are added to the given services.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection AddConfiguration(this IServiceCollection services)
    {
        services.TryAddSingleton(_ => new AppConfigurationInitializer().Init());

        return services;
    }
}
