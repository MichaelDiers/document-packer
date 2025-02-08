namespace DocumentPacker.Parts.LayoutPart;

using DocumentPacker.EventHandling;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for <see cref="IServiceCollection" />
/// </summary>
public static class LayoutPartServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the layout part of the application.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection AddLayoutPart(this IServiceCollection services)
    {
        services.AddKeyedTransient<IApplicationView, LayoutView>(ApplicationElementPart.Layout);
        services.AddKeyedTransient<IApplicationViewModel, LayoutViewModel>(ApplicationElementPart.Layout);

        return services;
    }
}
