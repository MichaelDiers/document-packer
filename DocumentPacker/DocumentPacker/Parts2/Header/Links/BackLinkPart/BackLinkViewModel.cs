namespace DocumentPacker.Parts2.Header.Links.BackLinkPart;

using System.Windows.Input;
using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
using DocumentPacker.Resources;

/// <summary>
///     The view model of <see cref="BackLinkView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationViewModel" />
/// <seealso cref="DocumentPacker.EventHandling.IHandleBackLink" />
internal class BackLinkViewModel : ApplicationViewModel, IHandleBackLink
{
    /// <summary>
    ///     A <see cref="Stack{T}" /> that stores the available back links.
    /// </summary>
    private readonly Stack<BackLinkEventArgs> backLinks = new();

    /// <summary>
    ///     The text of the back command button.
    /// </summary>
    private string commandText = Translation.BackLinkPartBack;

    /// <summary>
    ///     Gets the back command.
    /// </summary>
    public ICommand BackCommand =>
        new SyncCommand(
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
            });

    /// <summary>
    ///     Gets or sets the command button text.
    /// </summary>
    public string CommandText
    {
        get => this.commandText;
        set =>
            this.SetField(
                ref this.commandText,
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
