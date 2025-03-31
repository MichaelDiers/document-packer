namespace DocumentPacker.ApplicationInit;

using DocumentPacker.EventHandling;
using DocumentPacker.Parts.FooterPart;
using DocumentPacker.Parts.Header.HeaderPart;
using DocumentPacker.Parts.Header.Links.BackLinkPart;
using DocumentPacker.Parts.Header.Links.ChangeLanguageLinkPart;
using DocumentPacker.Parts.Header.NavigationBarPart;
using DocumentPacker.Parts.LayoutPart;
using DocumentPacker.Parts.Main.ChangeLanguagePart;
using DocumentPacker.Parts.Main.CreateConfigurationPart;
using DocumentPacker.Parts.Main.DecryptPart;
using DocumentPacker.Parts.Main.EncryptPart;
using DocumentPacker.Parts.Main.FeaturesPart;
using DocumentPacker.Parts.Main.MainPart;
using DocumentPacker.Parts.WindowPart;
using DocumentPacker.Services;
using Libs.Wpf.Commands;
using Libs.Wpf.Controls.CustomMessageBox;
using Microsoft.Extensions.DependencyInjection;
using Sreid.Libs.Crypto.Factory;

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
        services.TryAddCommandFactory();

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
        services.TryAddEncryptPart();
        services.TryAddDecryptPart();
        services.TryAddCreateConfigurationPart();

        services.TryAddServices();

        services.TryAddCommands();

        services.TryAddCustomMessageBoxServiceCollectionExtensions();

        services.TryAddFactory();

        return services;
    }
}
