namespace DocumentPacker.Parts2.Header.HeaderPart;

using DocumentPacker.EventHandling;

/// <summary>
///     Interaction logic for HeaderView.xaml
/// </summary>
[DataContext(ApplicationElementPart.Header)]
public partial class HeaderView : IApplicationView
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="HeaderView" /> class.
    /// </summary>
    public HeaderView()
    {
        this.InitializeComponent();
    }
}
