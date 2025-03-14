namespace DocumentPacker.Services;

using DocumentPacker.Services.DocumentPackerService;
using DocumentPacker.Services.DocumentUnpackerService;
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
        services.TryAddSingleton<IDocumentPackerConfigurationFileService, DocumentPackerConfigurationFileService>();
        services.TryAddSingleton<IDocumentPackerService, DocumentPackerService.DocumentPackerService>();
        services.TryAddSingleton<IEncryptService, EncryptService>();
        services.TryAddSingleton<IRsaService, RsaService>();
        services.TryAddSingleton<IDocumentUnpackerService, DocumentUnpackerService.DocumentUnpackerService>();
        services.TryAddSingleton<IDecryptService, DecryptService>();
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
}
