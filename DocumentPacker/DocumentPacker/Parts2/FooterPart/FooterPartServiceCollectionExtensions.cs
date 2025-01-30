namespace DocumentPacker.Parts2.FooterPart;

using DocumentPacker.EventHandling;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for <see cref="IServiceCollection" />
/// </summary>
public static class FooterPartServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the footer part of the application.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection AddFooterPart(this IServiceCollection services)
    {
        services.AddKeyedTransient<IApplicationView, FooterView>(ApplicationElementPart.Footer);
        services.AddKeyedTransient<IApplicationViewModel, FooterViewModel>(ApplicationElementPart.Footer);

        return services;
    }
}
