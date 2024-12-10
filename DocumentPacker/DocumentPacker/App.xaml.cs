namespace DocumentPacker;

using System.Windows;
using System.Windows.Threading;
using DocumentPacker.ApplicationInit.DependencyInjection;
using DocumentPacker.Parts.Contracts;
using DocumentPacker.Parts.DocumentPackerPart.Contracts;
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
    ///     Requests the view specified by <paramref name="part" />.
    /// </summary>
    /// <param name="part">The requested application part.</param>
    public static void RequestView(Part part)
    {
        App.RequestViewEvent?.Invoke(
            null,
            new RequestViewEventArgs(part));
    }

    /// <summary>
    ///     Occurs when an application part is requested.
    /// </summary>
    public static event EventHandler<RequestViewEventArgs>? RequestViewEvent;

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
        var view = App.ServiceProvider.GetRequiredService<IDocumentPackerWindow>();
        view.DataContext = App.ServiceProvider.GetRequiredService<IDocumentPackerViewModel>();
        view.Show();
    }
}
