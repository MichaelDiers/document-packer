namespace DocumentPacker.ApplicationInit.DependencyInjection;

using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
using DocumentPacker.Parts2.FooterPart;
using DocumentPacker.Parts2.Header.HeaderPart;
using DocumentPacker.Parts2.Header.Links.BackLinkPart;
using DocumentPacker.Parts2.Header.Links.ChangeLanguageLinkPart;
using DocumentPacker.Parts2.Header.NavigationBarPart;
using DocumentPacker.Parts2.LayoutPart;
using DocumentPacker.Parts2.Main.ChangeLanguagePart;
using DocumentPacker.Parts2.Main.CreateConfigurationPart;
using DocumentPacker.Parts2.Main.DecryptPart;
using DocumentPacker.Parts2.Main.EncryptPart;
using DocumentPacker.Parts2.Main.FeaturesPart;
using DocumentPacker.Parts2.Main.MainPart;
using DocumentPacker.Parts2.WindowPart;
using DocumentPacker.Services;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Extensions for <see cref="IServiceCollection" />.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds all dependencies of the application.
    /// </summary>
    /// <param name="services">Dependencies are added to this <see cref="IServiceCollection" />.</param>
    /// <returns>The given <paramref name="services" />.</returns>
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IDispatcher, ThreadDispatcher>();
        services.TryAddEventHandling();

        services.AddWindowPart();
        services.AddLayoutPart();

        services.AddHeaderPart();
        services.AddMainPart();
        services.AddFooterPart();

        services.AddBackLinkPart();
        services.AddLanguageLinkPart();
        services.AddNavigationBarPart();

        services.AddChangeLanguagePart();
        services.AddFeaturesPart();
        services.AddEncryptPart();
        services.AddDecryptPart();
        services.AddCreateConfigurationPart();

        services.TryAddAllServices();

        return services;
    }
}
