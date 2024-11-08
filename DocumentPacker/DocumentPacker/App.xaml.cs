namespace DocumentPacker;

using System.Windows;
using DocumentPacker.Contracts.Views;
using DocumentPacker.Extensions;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    /// <summary>
    ///     The provider used for resolving dependencies.
    /// </summary>
    private static IServiceProvider? provider;

    /// <summary>
    ///     Gets the service provider used for resolving dependencies.
    /// </summary>
    /// <remarks>Value is initialized on first access.</remarks>
    public static IServiceProvider ServiceProvider
    {
        get => App.provider ?? (App.provider = App.InitServiceProvider());
        private set => App.provider = value;
    }

    /// <summary>
    ///     Initializes the service provider and the application dependencies.
    /// </summary>
    /// <returns>The initialized <see cref="IServiceProvider" />.</returns>
    private static IServiceProvider InitServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddDependencies();
        return services.BuildServiceProvider();
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
