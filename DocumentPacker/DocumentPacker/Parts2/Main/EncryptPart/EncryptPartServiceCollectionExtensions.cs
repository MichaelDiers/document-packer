namespace DocumentPacker.Parts2.Main.EncryptPart;

using DocumentPacker.EventHandling;
using Microsoft.Extensions.DependencyInjection;

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

        return services;
    }
}
