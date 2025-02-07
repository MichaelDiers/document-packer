namespace DocumentPacker.ApplicationInit.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
///     Extensions for <see cref="IServiceCollection" />
/// </summary>
public static class ConfigurationServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the <see cref="IAppConfigurationInitializer" />.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection AddAppConfigurationInitializer(this IServiceCollection services)
    {
        services.TryAddSingleton<IAppConfigurationInitializer, AppConfigurationInitializer>();

        return services;
    }

    /// <summary>
    ///     Adds the <see cref="IAppConfiguration" />.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection AddConfiguration(this IServiceCollection services)
    {
        services.TryAddSingleton(_ => new AppConfigurationInitializer().Init());

        return services;
    }
}
