namespace DocumentPacker.Parts.EncryptStartUpPart.Extensions;

using DocumentPacker.Mvvm;
using DocumentPacker.Parts.Contracts;
using DocumentPacker.Parts.EncryptStartUpPart.Contracts;
using DocumentPacker.Parts.EncryptStartUpPart.View;
using DocumentPacker.Parts.EncryptStartUpPart.ViewModel;
using DocumentPacker.Resources;
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
        services.TryAddKeyedTransient<IPartView, EncryptStartUpView>(Part.EncryptStartUp);

        // view model
        services.AddTransient<IEncryptStartUpLinkViewModel>(
            _ => new EncryptStartUpLinkViewModel(
                Translation.EncryptStartUpPartLoadConfiguration,
                Translation.EncryptStartUpPartLoadConfigurationDescription,
                new SyncCommand(
                    _ => true,
                    _ => { })));
        services.AddTransient<IEncryptStartUpLinkViewModel>(
            _ => new EncryptStartUpLinkViewModel(
                Translation.EncryptStartUpPartCreateConfiguration,
                Translation.EncryptStartUpPartCreateConfigurationDescription,
                new SyncCommand(
                    _ => true,
                    _ => { })));

        services.TryAddKeyedTransient<IPartViewModel, EncryptStartUpViewModel>(Part.EncryptStartUp);

        return services;
    }
}
