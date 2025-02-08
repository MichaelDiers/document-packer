namespace DocumentPacker.Parts.Header.Links.ChangeLanguageLinkPart;

using DocumentPacker.EventHandling;

/// <summary>
///     Interaction logic for ChangeLanguageLinkView.xaml
/// </summary>
[DataContext(ApplicationElementPart.ChangeLanguageLink)]
public partial class ChangeLanguageLinkView : IApplicationView
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ChangeLanguageLinkView" /> class.
    /// </summary>
    public ChangeLanguageLinkView()
    {
        this.InitializeComponent();
    }
}
