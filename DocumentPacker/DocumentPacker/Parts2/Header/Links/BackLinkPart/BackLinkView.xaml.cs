namespace DocumentPacker.Parts2.Header.Links.BackLinkPart;

using DocumentPacker.EventHandling;

/// <summary>
///     Interaction logic for BackLinkView.xaml
/// </summary>
[DataContext(ApplicationElementPart.BackLink)]
public partial class BackLinkView : IApplicationView
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="BackLinkView" /> class.
    /// </summary>
    public BackLinkView()
    {
        this.InitializeComponent();
    }
}
