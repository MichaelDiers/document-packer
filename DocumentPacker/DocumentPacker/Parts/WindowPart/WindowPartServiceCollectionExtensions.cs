namespace DocumentPacker.Parts.WindowPart;

using System.Windows;
using DocumentPacker.EventHandling;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
///     Extensions for <see cref="IServiceCollection" />
/// </summary>
public static class WindowPartServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the window part of the application.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection AddWindowPart(this IServiceCollection services)
    {
        var window = new WindowView();
        services.TryAddKeyedSingleton<IApplicationWindow>(
            ApplicationElementPart.Window,
            window);
        services.TryAddSingleton(window as Window);
        services.AddKeyedTransient<IApplicationViewModel, WindowViewModel>(ApplicationElementPart.Window);

        return services;
    }
}
