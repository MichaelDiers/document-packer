namespace DocumentPacker.Parts.Main.CreateConfigurationPart;

using DocumentPacker.EventHandling;
using DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;
using DocumentPacker.Parts.Main.CreateConfigurationPart.Views;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions of <see cref="IServiceCollection" />.
/// </summary>
public static class CreateConfigurationServiceCollectionExtensions
{
    /// <summary>
    ///     Adds all supported dependencies to the given <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddCreateConfigurationPart(this IServiceCollection services)
    {
        services.TryAddCreateConfigurationView();
        services.TryAddCreateConfigurationViewModel();

        return services;
    }

    /// <summary>
    ///     Adds the <see cref="IApplicationView" /> to the given <paramref name="services" />.
    /// </summary>
    /// <param name="services">The dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddCreateConfigurationView(this IServiceCollection services)
    {
        services.AddKeyedSingleton<IApplicationView, CreateConfigurationView>(
            ApplicationElementPart.CreateConfiguration);

        return services;
    }

    /// <summary>
    ///     Adds the <see cref="IApplicationViewModel" /> to the given <paramref name="services" />.
    /// </summary>
    /// <param name="services">The dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddCreateConfigurationViewModel(this IServiceCollection services)
    {
        services.AddKeyedSingleton<IApplicationViewModel, CreateConfigurationViewModel>(
            ApplicationElementPart.CreateConfiguration);

        return services;
    }
}
