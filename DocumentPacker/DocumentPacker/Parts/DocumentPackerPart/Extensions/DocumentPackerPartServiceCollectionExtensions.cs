namespace DocumentPacker.Parts.DocumentPackerPart.Extensions;

using DocumentPacker.Parts.DocumentPackerPart.Contracts;
using DocumentPacker.Parts.DocumentPackerPart.View;
using DocumentPacker.Parts.DocumentPackerPart.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
///     Extensions for <seealso cref="IServiceCollection" />.
/// </summary>
public static class DocumentPackerPartServiceCollectionExtensions
{
    /// <summary>
    ///     Tries to add dependencies for the document packer application part.
    /// </summary>
    /// <param name="services">The services collection to that the dependencies are added.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddDocumentPackerPartDependencies(this IServiceCollection services)
    {
        // view
        services.TryAddSingleton<IDocumentPackerWindow, DocumentPackerWindow>();

        // view model
        services.TryAddSingleton<IDocumentPackerViewModel, DocumentPackerViewModel>();

        return services;
    }
}
