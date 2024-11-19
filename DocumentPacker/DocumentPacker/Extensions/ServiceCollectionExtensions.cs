namespace DocumentPacker.Extensions;

using DocumentPacker.Contracts;
using DocumentPacker.Contracts.ViewModels;
using DocumentPacker.Contracts.ViewModels.SubViewModels;
using DocumentPacker.Contracts.Views;
using DocumentPacker.ViewModels;
using DocumentPacker.ViewModels.SubViewModels;
using DocumentPacker.Views;
using DocumentPacker.Views.SubViews;
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
        services.AddSingleton<IMainViewModel, MainViewModel>();
        services.AddSingleton<ICollectDocumentsViewModel, CollectDocumentsViewModel>();
        services.AddSingleton<ILoadConfigurationViewModel, LoadConfigurationViewModel>();
        services.AddSingleton<IStartUpViewModel, StartUpViewModel>();

        services.AddSingleton<IMainWindow, MainWindow>();
        services.AddSingleton<CollectDocumentsView>();
        services.AddSingleton<LoadConfigurationView>();
        services.AddSingleton<StartUpView>();

        services.AddSingleton<IDispatcher, ThreadDispatcher>();
        services.AddSingleton<IViewHandler, ViewHandler>();

        return services;
    }
}
