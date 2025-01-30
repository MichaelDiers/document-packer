namespace DocumentPacker.Parts2.Main.MainPart;

using DocumentPacker.EventHandling;

/// <summary>
///     Interaction logic for MainView.xaml
/// </summary>
[DataContext(ApplicationElementPart.Main)]
public partial class MainView : IApplicationView
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MainView" /> class.
    /// </summary>
    public MainView()
    {
        this.InitializeComponent();
    }
}
