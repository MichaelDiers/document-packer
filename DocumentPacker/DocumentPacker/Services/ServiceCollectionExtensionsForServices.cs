namespace DocumentPacker.Services;

using DocumentPacker.Services.Crypto;
using DocumentPacker.Services.DocumentPackerService;
using DocumentPacker.Services.DocumentUnpackerService;
using DocumentPacker.Services.Packer;
using DocumentPacker.Services.Zip;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
///     Add services dependencies by using extensions for <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensionsForServices
{
    /// <summary>
    ///     Tries to add all services as a dependency.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddAllServices(this IServiceCollection services)
    {
        return services.TryAddZipServices().TryAddCryptoServices().TryAddPackerServices();
    }

    /// <summary>
    ///     Tries to add all services that handle crypto data as a dependency.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddCryptoServices(this IServiceCollection services)
    {
        services.TryAddSingleton<ICryptoFactory, CryptoFactory>();

        return services;
    }

    /// <summary>
    ///     Tries to add the document packer service.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddDocumentPackerService(this IServiceCollection services)
    {
        services.TryAddSingleton<IDocumentPackerService, DocumentPackerService.DocumentPackerService>();

        return services;
    }

    /// <summary>
    ///     Tries to add the document unpacker service.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddDocumentUnpackerService(this IServiceCollection services)
    {
        services.TryAddSingleton<IDocumentUnpackerService, DocumentUnpackerService.DocumentUnpackerService>();

        return services;
    }

    /// <summary>
    ///     Tries to add all services that handle package files as a dependency.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddPackerServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IPackerFactory, Packer.Packer>();

        return services;
    }

    /// <summary>
    ///     Tries to add all services that handle zip files as a dependency.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddZipServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IZipFileCreator, ZipFileCreator>();

        return services;
    }
}
