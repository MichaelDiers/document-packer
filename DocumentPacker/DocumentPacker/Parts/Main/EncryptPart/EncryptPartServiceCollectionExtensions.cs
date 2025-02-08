namespace DocumentPacker.Parts.Main.EncryptPart;

using DocumentPacker.EventHandling;
using DocumentPacker.Parts.Main.EncryptPart.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
///     Extensions for <see cref="IServiceCollection" />
/// </summary>
public static class EncryptPartServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the encrypt part of the application.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection AddEncryptPart(this IServiceCollection services)
    {
        services.AddKeyedTransient<IApplicationView, EncryptView>(ApplicationElementPart.EncryptFeature);
        services.AddKeyedTransient<IApplicationViewModel, EncryptViewModel>(ApplicationElementPart.EncryptFeature);

        services.TryAddSingleton<IEncryptService, EncryptService>();

        return services;
    }
}
