namespace DocumentPacker.Parts.Header.Links.BackLinkPart;

using System.Windows.Input;
using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
using Libs.Wpf.Commands;

/// <summary>
///     The view model of <see cref="BackLinkView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
/// <seealso cref="DocumentPacker.EventHandling.IHandleBackLink" />
internal class BackLinkViewModel(ICommandFactory commandFactory) : ApplicationBaseViewModel, IHandleBackLink
{
    /// <summary>
    ///     A <see cref="Stack{T}" /> that stores the available back links.
    /// </summary>
    private readonly Stack<BackLinkEventArgs> backLinks = new();

    /// <summary>
    ///     Gets the back command.
    /// </summary>
    public ICommand BackCommand =>
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
            });

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
