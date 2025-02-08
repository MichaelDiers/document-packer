namespace DocumentPacker.Parts.Header.NavigationBarPart;

using DocumentPacker.EventHandling;

/// <summary>
///     Interaction logic for NavigationBarView.xaml
/// </summary>
[DataContext(ApplicationElementPart.NavigationBar)]
public partial class NavigationBarView : IApplicationView
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="NavigationBarView" /> class.
    /// </summary>
    public NavigationBarView()
    {
        this.InitializeComponent();
    }
}
