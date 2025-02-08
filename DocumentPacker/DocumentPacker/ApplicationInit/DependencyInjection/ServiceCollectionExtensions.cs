﻿namespace DocumentPacker.ApplicationInit.DependencyInjection;

using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
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
