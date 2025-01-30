namespace DocumentPacker.Parts2.FooterPart;

using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;

/// <summary>
///     Interaction logic for FooterView.xaml
/// </summary>
[DataContext(ApplicationElementPart.Footer)]
public partial class FooterView : BaseUserControl, IApplicationView
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="FooterView" /> class.
    /// </summary>
    public FooterView()
    {
        this.InitializeComponent();
    }
}
