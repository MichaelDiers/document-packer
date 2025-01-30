namespace DocumentPacker.Parts2.Main.DecryptPart;

using DocumentPacker.EventHandling;

/// <summary>
///     Interaction logic for DecryptView.xaml
/// </summary>
[DataContext(ApplicationElementPart.Main)]
public partial class DecryptView : IApplicationView
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DecryptView" /> class.
    /// </summary>
    public DecryptView()
    {
        this.InitializeComponent();
    }
}
