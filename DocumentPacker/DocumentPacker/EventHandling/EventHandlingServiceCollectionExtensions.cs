namespace DocumentPacker.EventHandling;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
///     Extensions for <see cref="IServiceCollection" />
/// </summary>
public static class EventHandlingServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the <see cref="IEventHandlerCenter" /> of the application.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddEventHandling(this IServiceCollection services)
    {
        services.TryAddSingleton<IEventHandlerCenter, EventHandlerCenter>();

        return services;
    }
}
