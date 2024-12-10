namespace DocumentPacker.Parts.StartUpPart.Extensions;

using DocumentPacker.Mvvm;
using DocumentPacker.Parts.Contracts;
using DocumentPacker.Parts.StartUpPart.Contracts;
using DocumentPacker.Parts.StartUpPart.View;
using DocumentPacker.Parts.StartUpPart.ViewModel;
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
        services.TryAddKeyedSingleton<IPartView, StartUpView>(Part.StartUp);

        // view model
        services.AddSingleton<IStartUpLinkViewModel>(
            _ => new StartUpLinkViewModel(
                "Encrypt",
                "Encrypt",
                new SyncCommand(
                    _ => false,
                    _ => { })));
        services.AddSingleton<IStartUpLinkViewModel>(
            _ => new StartUpLinkViewModel(
                "Decrypt",
                "Decrypt",
                new SyncCommand(
                    _ => false,
                    _ => { })));

        services.TryAddSingleton<IStartUpViewModel, StartUpViewModel>();

        return services;
    }
}
