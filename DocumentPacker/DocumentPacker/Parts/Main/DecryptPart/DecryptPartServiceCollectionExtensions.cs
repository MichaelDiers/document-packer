namespace DocumentPacker.Parts.Main.DecryptPart;

using DocumentPacker.EventHandling;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
///     Extensions of <see cref="IServiceCollection" />.
/// </summary>
public static class DecryptPartServiceCollectionExtensions
{
    /// <summary>
    ///     Adds all supported dependencies to the given <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddDecryptPart(this IServiceCollection services)
    {
        services.TryAddDecryptView();
        services.TryAddDecryptViewModel();

        return services;
    }

    /// <summary>
    ///     Adds the <see cref="DecryptViewModel" /> to the given <paramref name="services" />.
    /// </summary>
    /// <param name="services">The dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddDecryptView(this IServiceCollection services)
    {
        services.TryAddKeyedSingleton<IApplicationView, DecryptView>(ApplicationElementPart.DecryptFeature);

        return services;
    }

    /// <summary>
    ///     Adds the <see cref="IDecryptViewModel" /> to the given <paramref name="services" />.
    /// </summary>
    /// <param name="services">The dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddDecryptViewModel(this IServiceCollection services)
    {
        services.TryAddKeyedSingleton<IApplicationViewModel, DecryptViewModel>(ApplicationElementPart.DecryptFeature);
        services.TryAddSingleton<IDecryptViewModel, DecryptViewModel>();

        return services;
    }
}
