namespace DocumentPacker.Parts.Header.HeaderPart;

using DocumentPacker.EventHandling;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for <see cref="IServiceCollection" />
/// </summary>
public static class HeaderPartServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the header part of the application.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection AddHeaderPart(this IServiceCollection services)
    {
        services.AddKeyedTransient<IApplicationView, HeaderView>(ApplicationElementPart.Header);
        services.AddKeyedTransient<IApplicationViewModel, HeaderViewModel>(ApplicationElementPart.Header);

        return services;
    }
}
