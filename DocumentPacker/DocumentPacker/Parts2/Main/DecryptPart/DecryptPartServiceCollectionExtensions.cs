namespace DocumentPacker.Parts2.Main.DecryptPart;

using DocumentPacker.EventHandling;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for <see cref="IServiceCollection" />
/// </summary>
public static class DecryptPartPartServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the decrypt part of the application.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection AddDecryptPart(this IServiceCollection services)
    {
        services.AddKeyedTransient<IApplicationView, DecryptView>(ApplicationElementPart.DecryptFeature);
        services.AddKeyedTransient<IApplicationViewModel, DecryptViewModel>(ApplicationElementPart.DecryptFeature);

        return services;
    }
}
