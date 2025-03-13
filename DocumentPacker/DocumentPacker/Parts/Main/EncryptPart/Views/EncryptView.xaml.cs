namespace DocumentPacker.Parts.Main.EncryptPart.Views;

using DocumentPacker.EventHandling;

/// <summary>
///     Interaction logic for EncryptView.xaml
/// </summary>
[DataContext(ApplicationElementPart.EncryptFeature)]
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
