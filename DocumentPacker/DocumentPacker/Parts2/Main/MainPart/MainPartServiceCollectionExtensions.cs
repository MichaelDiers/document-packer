namespace DocumentPacker.Parts2.Main.MainPart;

using DocumentPacker.EventHandling;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for <see cref="IServiceCollection" />
/// </summary>
public static class MainPartServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the main part of the application.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection AddMainPart(this IServiceCollection services)
    {
        services.AddKeyedTransient<IApplicationView, MainView>(ApplicationElementPart.Main);
        services.AddKeyedTransient<IApplicationViewModel, MainViewModel>(ApplicationElementPart.Main);

        return services;
    }
}
