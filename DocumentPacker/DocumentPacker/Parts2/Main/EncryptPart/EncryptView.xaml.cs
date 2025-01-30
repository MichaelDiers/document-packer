namespace DocumentPacker.Parts2.Main.EncryptPart;

using DocumentPacker.EventHandling;

/// <summary>
///     Interaction logic for EncryptView.xaml
/// </summary>
[DataContext(ApplicationElementPart.Main)]
public partial class EncryptView : IApplicationView
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="EncryptView" /> class.
    /// </summary>
    public EncryptView()
    {
        this.InitializeComponent();
    }
}
