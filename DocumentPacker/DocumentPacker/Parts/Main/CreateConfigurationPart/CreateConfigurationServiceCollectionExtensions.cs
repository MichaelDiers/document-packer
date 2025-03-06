namespace DocumentPacker.Parts.Main.CreateConfigurationPart;

using DocumentPacker.EventHandling;
using DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;
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
        services.AddKeyedSingleton<IApplicationView, CreateConfigurationView>(
            ApplicationElementPart.CreateConfiguration);
        services.AddKeyedSingleton<IApplicationViewModel, CreateConfigurationViewModel>(
            ApplicationElementPart.CreateConfiguration);

        return services;
    }
}
