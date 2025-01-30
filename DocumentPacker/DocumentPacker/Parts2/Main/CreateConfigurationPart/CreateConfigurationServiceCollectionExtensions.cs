namespace DocumentPacker.Parts2.Main.CreateConfigurationPart;

using DocumentPacker.EventHandling;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for <see cref="IServiceCollection" />
/// </summary>
public static class CreateConfigurationServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the create-configuration part of the application.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection AddCreateConfigurationPart(this IServiceCollection services)
    {
        services.AddKeyedTransient<IApplicationView, CreateConfigurationView>(
            ApplicationElementPart.CreateConfiguration);
        services.AddKeyedTransient<IApplicationViewModel, CreateConfigurationViewModel>(
            ApplicationElementPart.CreateConfiguration);

        return services;
    }
}
