namespace DocumentPacker.Parts.Header.Links.BackLinkPart;

using System.Windows.Input;
using DocumentPacker.EventHandling;
using DocumentPacker.Extensions;
using DocumentPacker.Mvvm;
using Libs.Wpf.Commands;
using Libs.Wpf.ViewModels;

/// <summary>
///     The view model of <see cref="BackLinkView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
/// <seealso cref="DocumentPacker.EventHandling.IHandleBackLink" />
internal class BackLinkViewModel : ApplicationBaseViewModel, IHandleBackLink
{
    /// <summary>
    ///     A <see cref="Stack{T}" /> that stores the available back links.
    /// </summary>
    private readonly Stack<BackLinkEventArgs> backLinks = new();

    /// <summary>
    ///     The back command.
    /// </summary>
    private TranslatableButton<ICommand> backCommand;

    /// <summary>
    ///     The view model of <see cref="BackLinkView" />.
    /// </summary>
    /// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
    /// <seealso cref="DocumentPacker.EventHandling.IHandleBackLink" />
    public BackLinkViewModel(ICommandFactory commandFactory)
    {
        this.backCommand = new TranslatableButton<ICommand>(
            commandFactory.CreateSyncCommand(
                _ => this.backLinks.Count != 0,
                _ =>
                {
                    // check if a back link is available
                    if (this.backLinks.TryPop(out var backLink))
                    {
                        // request to show the previous view 
                        this.InvokeShowViewRequested(
                            this,
                            new ShowViewRequestedEventArgs(backLink.Part)
                            {
                                SuppressBackLink = true,
                                View = backLink.View
                            });
                    }
                }),
            "material_symbol_arrow_back.png".ToBitmapImage(),
            BackLinkPartTranslation.ResourceManager,
            toolTipResourceKey: nameof(BackLinkPartTranslation.Back));
    }

    /// <summary>
    ///     Gets or sets the back command.
    /// </summary>
    public TranslatableButton<ICommand> BackCommand
    {
        get => this.backCommand;
        set =>
            this.SetField(
                ref this.backCommand,
                value);
    }

    /// <summary>
    ///     Handles raised back link events. If a view is replaced in an application part, it can raise the back link event.
    ///     The back link information is handled by this component.
    /// </summary>
    /// <param name="sender">The sender that raised the event.</param>
    /// <param name="eventArgs">The <see cref="BackLinkEventArgs" /> instance containing the event data.</param>
    public void HandleBackLink(object? sender, BackLinkEventArgs eventArgs)
    {
        this.backLinks.Push(eventArgs);
    }
}
