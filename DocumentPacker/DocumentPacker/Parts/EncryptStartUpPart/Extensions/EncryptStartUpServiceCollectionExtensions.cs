namespace DocumentPacker.Parts.EncryptStartUpPart.Extensions;

using DocumentPacker.Mvvm;
using DocumentPacker.Parts.Contracts;
using DocumentPacker.Parts.EncryptStartUpPart.Contracts;
using DocumentPacker.Parts.EncryptStartUpPart.View;
using DocumentPacker.Parts.EncryptStartUpPart.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
///     Extensions for <seealso cref="IServiceCollection" />.
/// </summary>
public static class EncryptStartUpServiceCollectionExtensions
{
    /// <summary>
    ///     Tries to add dependencies for the encryption startup application part.
    /// </summary>
    /// <param name="services">The services collection to that the dependencies are added.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddEncryptStartUpDependencies(this IServiceCollection services)
    {
        // view
        services.TryAddKeyedSingleton<IPartView, EncryptStartUpView>(Part.EncryptStartUp);

        // view model
        services.AddSingleton<IEncryptStartUpLinkViewModel>(
            _ => new EncryptStartUpLinkViewModel(
                "Load a configuration",
                "Load",
                new SyncCommand(
                    _ => true,
                    _ => { })));
        services.AddSingleton<IEncryptStartUpLinkViewModel>(
            _ => new EncryptStartUpLinkViewModel(
                "Create a configuration",
                "Create",
                new SyncCommand(
                    _ => true,
                    _ => { })));

        services.TryAddKeyedSingleton<IPartViewModel, EncryptStartUpViewModel>(Part.EncryptStartUp);

        return services;
    }
}
