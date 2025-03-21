﻿namespace DocumentPacker;

using System.Globalization;
using System.Windows;
using System.Windows.Threading;
using DocumentPacker.ApplicationInit;
using DocumentPacker.EventHandling;
using Libs.Wpf.DependencyInjection;
using Libs.Wpf.Localization;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
// ReSharper disable once RedundantExtendsListEntry
public partial class App : Application
{
    /// <summary>
    ///     The provider used for resolving dependencies.
    /// </summary>
    private static IServiceProvider? provider;

    /// <summary>
    ///     Gets the service provider used for resolving dependencies.
    /// </summary>
    /// <remarks>Name is initialized on first access.</remarks>
    public static IServiceProvider ServiceProvider =>
        App.provider ??
        (App.provider = CustomServiceProviderBuilder.Build(ServiceCollectionExtensions.AddDependencies));

    /// <summary>
    ///     Called when an unhandled exception is raised.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="DispatcherUnhandledExceptionEventArgs" /> instance containing the event data.</param>
    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        MessageBox.Show(
            e.Exception.Message,
            "Error",
            MessageBoxButton.OK,
            MessageBoxImage.Error,
            MessageBoxResult.OK,
            MessageBoxOptions.None);
        e.Handled = true;
    }

    /// <summary>
    ///     Called when at application startup.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="StartupEventArgs" /> instance containing the event data.</param>
    private void OnStartup(object sender, StartupEventArgs e)
    {
        TranslationSource.Instance.CurrentCulture =
            Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToLower() switch
            {
                "de" => Thread.CurrentThread.CurrentUICulture,
                _ => CultureInfo.InvariantCulture
            };

        this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
        var eventHandlerCenter = App.ServiceProvider.GetRequiredService<IEventHandlerCenter>();
        eventHandlerCenter.Closed += (_, _) => this.Shutdown();
        eventHandlerCenter.Initialize(App.ServiceProvider);
    }
}
