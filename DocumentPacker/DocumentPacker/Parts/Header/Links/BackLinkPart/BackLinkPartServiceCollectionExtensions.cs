namespace DocumentPacker.Parts.Header.Links.BackLinkPart;

using DocumentPacker.EventHandling;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for <see cref="IServiceCollection" />
/// </summary>
public static class BackLinkPartServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the back link part of the application.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection AddBackLinkPart(this IServiceCollection services)
    {
        services.AddKeyedTransient<IApplicationView, BackLinkView>(ApplicationElementPart.BackLink);
        services.AddKeyedTransient<IApplicationViewModel, BackLinkViewModel>(ApplicationElementPart.BackLink);

        return services;
    }
}
