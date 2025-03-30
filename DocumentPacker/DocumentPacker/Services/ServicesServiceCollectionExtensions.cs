namespace DocumentPacker.Services;

using DocumentPacker.Services.DocumentPackerService;
using DocumentPacker.Services.DocumentUnpackerService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
///     Extensions of <see cref="IServiceCollection" />.
/// </summary>
public static class ServicesServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the <see cref="IDecryptService" /> to the given <paramref name="services" />.
    /// </summary>
    /// <param name="services">The dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddDecryptService(this IServiceCollection services)
    {
        services.TryAddSingleton<IDecryptService, DecryptService>();

        return services;
    }

    /// <summary>
    ///     Adds the <see cref="IDocumentPackerConfigurationFileService" /> to the given <paramref name="services" />.
    /// </summary>
    /// <param name="services">The dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddDocumentPackerConfigurationFileService(this IServiceCollection services)
    {
        services.TryAddSingleton<IDocumentPackerConfigurationFileService, DocumentPackerConfigurationFileService>();

        return services;
    }

    /// <summary>
    ///     Adds the <see cref="IDocumentPackerService" /> to the given <paramref name="services" />.
    /// </summary>
    /// <param name="services">The dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddDocumentPackerService(this IServiceCollection services)
    {
        services.TryAddSingleton<IDocumentPackerService, DocumentPackerService.DocumentPackerService>();

        return services;
    }

    /// <summary>
    ///     Adds the <see cref="IDocumentUnpackerService" /> to the given <paramref name="services" />.
    /// </summary>
    /// <param name="services">The dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddDocumentUnpackerService(this IServiceCollection services)
    {
        services.TryAddSingleton<IDocumentUnpackerService, DocumentUnpackerService.DocumentUnpackerService>();

        return services;
    }

    /// <summary>
    ///     Adds the <see cref="IEncryptService" /> to the given <paramref name="services" />.
    /// </summary>
    /// <param name="services">The dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddEncryptService(this IServiceCollection services)
    {
        services.TryAddSingleton<IEncryptService, EncryptService>();

        return services;
    }

    /// <summary>
    ///     Adds all supported dependencies to the given <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddServices(this IServiceCollection services)
    {
        services.TryAddDecryptService();
        services.TryAddDocumentPackerConfigurationFileService();
        services.TryAddDocumentPackerService();
        services.TryAddDocumentUnpackerService();
        services.TryAddEncryptService();

        return services;
    }
}
