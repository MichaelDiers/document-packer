namespace DocumentPacker.Parts.Header.NavigationBarPart;

using DocumentPacker.EventHandling;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for <see cref="IServiceCollection" />
/// </summary>
public static class NavigationBarServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the navigation bar part of the application.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection AddNavigationBarPart(this IServiceCollection services)
    {
        services.AddKeyedTransient<IApplicationView, NavigationBarView>(ApplicationElementPart.NavigationBar);
        services.AddKeyedTransient<IApplicationViewModel, NavigationBarViewModel>(ApplicationElementPart.NavigationBar);

        return services;
    }
}
