namespace DocumentPacker.Parts.LayoutPart;

using DocumentPacker.EventHandling;

/// <summary>
///     Interaction logic for LayoutView.xaml
/// </summary>
[DataContext(ApplicationElementPart.Layout)]
public partial class LayoutView : IApplicationView
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="LayoutView" /> class.
    /// </summary>
    public LayoutView()
    {
        this.InitializeComponent();
    }
}
