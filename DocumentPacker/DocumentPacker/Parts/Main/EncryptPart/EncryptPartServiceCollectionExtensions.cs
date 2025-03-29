namespace DocumentPacker.Parts.Main.EncryptPart;

using DocumentPacker.EventHandling;
using DocumentPacker.Parts.Main.EncryptPart.ViewModels;
using DocumentPacker.Parts.Main.EncryptPart.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
///     Extensions of <see cref="IServiceCollection" />.
/// </summary>
public static class EncryptPartServiceCollectionExtensions
{
    /// <summary>
    ///     Adds all supported dependencies to the given <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddEncryptPart(this IServiceCollection services)
    {
        services.TryAddEncryptView();
        services.TryAddEncryptViewModel();

        return services;
    }

    /// <summary>
    ///     Adds the <see cref="IApplicationView" /> to the given <paramref name="services" />.
    /// </summary>
    /// <param name="services">The dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddEncryptView(this IServiceCollection services)
    {
        services.TryAddKeyedSingleton<IApplicationView, EncryptView>(ApplicationElementPart.EncryptFeature);

        return services;
    }

    /// <summary>
    ///     Adds the <see cref="IApplicationViewModel" /> to the given <paramref name="services" />.
    /// </summary>
    /// <param name="services">The dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddEncryptViewModel(this IServiceCollection services)
    {
        services.TryAddKeyedSingleton<IApplicationViewModel, EncryptViewModel>(ApplicationElementPart.EncryptFeature);

        return services;
    }
}
