namespace DocumentPacker.Extensions;

using DocumentPacker.Contracts.ViewModels;
using DocumentPacker.Contracts.Views;
using DocumentPacker.ViewModels;
using DocumentPacker.Views;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds all dependencies of the application.
    /// </summary>
    /// <param name="services">Dependencies are added to the given services.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IMainWindow, MainWindow>();
        services.AddSingleton<IMainViewModel, MainViewModel>();
        services.AddSingleton<IDispatcher, ThreadDispatcher>();

        return services;
    }
}
