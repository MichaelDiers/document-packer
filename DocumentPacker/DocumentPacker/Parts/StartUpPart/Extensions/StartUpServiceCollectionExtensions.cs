namespace DocumentPacker.Parts.StartUpPart.Extensions;

using DocumentPacker.Mvvm;
using DocumentPacker.Parts.Contracts;
using DocumentPacker.Parts.StartUpPart.Contracts;
using DocumentPacker.Parts.StartUpPart.View;
using DocumentPacker.Parts.StartUpPart.ViewModel;
using DocumentPacker.Resources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
///     Extensions for <seealso cref="IServiceCollection" />.
/// </summary>
public static class StartUpServiceCollectionExtensions
{
    /// <summary>
    ///     Tries to add dependencies for the startup application part.
    /// </summary>
    /// <param name="services">The services collection to that the dependencies are added.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection TryAddStartUpDependencies(this IServiceCollection services)
    {
        // view
        services.TryAddKeyedTransient<IPartView, StartUpView>(Part.StartUp);

        // view model
        services.AddTransient<IStartUpLinkViewModel>(
            _ => new StartUpLinkViewModel(
                Translation.StartUpPartEncrypt,
                Translation.StartUpPartEncryptDescription,
                new SyncCommand(
                    _ => true,
                    _ => App.RequestView(Part.EncryptStartUp))));
        services.AddTransient<IStartUpLinkViewModel>(
            _ => new StartUpLinkViewModel(
                Translation.StartUpPartDecrypt,
                Translation.StartUpPartDecryptDescription,
                new SyncCommand(
                    _ => true,
                    _ => { })));

        services.TryAddKeyedTransient<IPartViewModel, StartUpViewModel>(Part.StartUp);

        return services;
    }
}
