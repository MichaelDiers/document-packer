namespace DocumentPacker.Parts.Main.MainPart;

using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;

/// <summary>
///     The view model of the <see cref="MainView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
internal class MainViewModel : ApplicationBaseViewModel
{
    /// <summary>
    ///     The view model supports the specified list of <see cref="ApplicationElementPart" /> only.
    /// </summary>
    private readonly IEnumerable<ApplicationElementPart> allowedContent =
    [
        ApplicationElementPart.ChangeLanguage, ApplicationElementPart.Features, ApplicationElementPart.EncryptFeature,
        ApplicationElementPart.DecryptFeature, ApplicationElementPart.CreateConfiguration
    ];

    /// <summary>
    ///     The displayed content.
    /// </summary>
    private object? content;

    /// <summary>
    ///     The displayed <see cref="ApplicationElementPart" />.
    /// </summary>
    private ApplicationElementPart currentApplicationElementPart = ApplicationElementPart.Features;

    /// <summary>
    ///     Gets or sets the displayed content.
    /// </summary>
    [ApplicationPart(ApplicationElementPart.Features)]
    public object? Content
    {
        get => this.content;
        set =>
            this.SetField(
                ref this.content,
                value);
    }

    /// <inheritdoc cref="ApplicationBaseViewModel.HandleShowViewRequested" />
    public override void HandleShowViewRequested(object? sender, ShowViewRequestedEventArgs eventArgs)
    {
        // reject if the requested view is not displayed in this component
        if (!this.allowedContent.Contains(eventArgs.Part))
        {
            return;
        }

        // request to store a back link
        // do not create a back link if the new view is a requested back link
        if (!eventArgs.SuppressBackLink && this.Content is IApplicationView view)
        {
            this.InvokeBackLinkCreated(
                this,
                new BackLinkEventArgs(
                    this.currentApplicationElementPart,
                    view));
        }

        // set the new view
        this.currentApplicationElementPart = eventArgs.Part;
        this.Content = eventArgs.View;
    }
}
