﻿namespace DocumentPacker;

using System.Windows;
using System.Windows.Threading;
using DocumentPacker.ApplicationInit.DependencyInjection;
using DocumentPacker.Contracts.Views;
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
        App.provider ?? (App.provider = ServiceProviderInitializer.Init());

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
    ///     Called when at the startup of the application.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="StartupEventArgs" /> instance containing the event data.</param>
    private void OnStartup(object sender, StartupEventArgs e)
    {
        App.ServiceProvider.GetRequiredService<IMainWindow>().Show();
    }
}
