namespace DocumentPacker.ViewModels;

using System.Windows.Input;
using DocumentPacker.Contracts;
using DocumentPacker.Contracts.ViewModels;
using DocumentPacker.Contracts.ViewModels.SubViewModels;
using DocumentPacker.ViewModels.SubViewModels;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Describes the main view model.
/// </summary>
internal class MainViewModel : BaseViewModel, IMainViewModel
{
    /// <summary>
    ///     The fatal error message text.
    /// </summary>
    private string? fatalErrorMessage;

    /// <summary>
    ///     The current sub view model.
    /// </summary>
    private ISubViewModel subViewModel = new StartUpViewModel();

    /// <summary>
    ///     Initializes a new instance of the <see cref="MainViewModel" /> class.
    /// </summary>
    public MainViewModel()
        : this(
            App.ServiceProvider.GetRequiredService<ICollectDocumentsViewModel>(),
            App.ServiceProvider.GetRequiredService<ILoadConfigurationViewModel>(),
            App.ServiceProvider.GetRequiredService<IStartUpViewModel>(),
            App.ServiceProvider.GetRequiredService<IViewHandler>())
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MainViewModel" /> class.
    /// </summary>
    /// <param name="collectDocumentsViewModel">The collect documents sub view model.</param>
    /// <param name="loadConfigurationViewModel">The load configuration sub view model.</param>
    /// <param name="startUpViewModel">The startup sub view model.</param>
    /// <param name="viewHandler">The view handler.</param>
    public MainViewModel(
        ICollectDocumentsViewModel collectDocumentsViewModel,
        ILoadConfigurationViewModel loadConfigurationViewModel,
        IStartUpViewModel startUpViewModel,
        IViewHandler viewHandler
    )
    {
        TaskCommand.FatalError += (_, message) => this.FatalErrorMessage = message;

        viewHandler.RegisterView<object>(
            SubViewId.CollectDocuments,
            _ => this.SubViewModel = collectDocumentsViewModel);
        viewHandler.RegisterView<object>(
            SubViewId.LoadConfiguration,
            _ => this.SubViewModel = loadConfigurationViewModel);
        viewHandler.RegisterView<object>(
            SubViewId.StartUp,
            _ => this.SubViewModel = startUpViewModel);
    }

    /// <summary>
    ///     Gets the close fatal error message command.
    /// </summary>
    public ICommand CloseFatalErrorMessageCommand =>
        new TaskCommand(
            _ => this.FatalErrorMessage is not null,
            (_, _) =>
            {
                this.FatalErrorMessage = null;
                return Task.CompletedTask;
            });

    /// <summary>
    ///     Gets or sets the fatal error message text.
    /// </summary>
    public string? FatalErrorMessage
    {
        get => this.fatalErrorMessage;
        set =>
            this.SetField(
                ref this.fatalErrorMessage,
                value);
    }

    /// <summary>
    ///     Gets the home command.
    /// </summary>
    public ICommand HomeCommand =>
        new TaskCommand(
            _ => this.SubViewModel.SubViewId != SubViewId.StartUp,
            (_, _) =>
            {
                App.ServiceProvider.GetRequiredService<IViewHandler>().RequestView(SubViewId.StartUp);
                return Task.CompletedTask;
            });

    /// <summary>
    ///     Gets or sets the sub view model.
    /// </summary>
    public ISubViewModel SubViewModel
    {
        get => this.subViewModel;
        set =>
            this.SetField(
                ref this.subViewModel,
                value);
    }
}
