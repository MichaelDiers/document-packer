namespace DocumentPacker.Parts.FooterPart;

using DocumentPacker.EventHandling;

/// <summary>
///     Interaction logic for FooterView.xaml
/// </summary>
[DataContext(ApplicationElementPart.Footer)]
public partial class FooterView : IApplicationView
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="FooterView" /> class.
    /// </summary>
    public FooterView()
    {
        this.InitializeComponent();
    }
}
