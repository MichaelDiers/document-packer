namespace DocumentPacker.ApplicationInit.DependencyInjection;

using DocumentPacker.ApplicationInit.Configuration;
using DocumentPacker.Mvvm;
using DocumentPacker.Parts.DocumentPackerPart.Extensions;
using DocumentPacker.Parts.StartUpPart.Extensions;
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
        services.AddConfiguration();

        // parts
        services.TryAddStartUpDependencies();

        // window
        services.TryAddDocumentPackerPartDependencies();

        services.AddSingleton<IDispatcher, ThreadDispatcher>();
        //services.AddSingleton<IViewHandler, ViewHandler>();

        return services;
    }
}
