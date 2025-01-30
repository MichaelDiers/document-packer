namespace DocumentPacker.Parts2.WindowPart;

using DocumentPacker.EventHandling;

/// <summary>
///     Interaction logic for WindowView.xaml
/// </summary>
[DataContext(ApplicationElementPart.Window)]
public partial class WindowView : IApplicationWindow
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="WindowView" /> class.
    /// </summary>
    public WindowView()
    {
        this.InitializeComponent();
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        if (this.DataContext is IApplicationViewModel viewModel)
        {
            viewModel.Dispose();
        }

        this.DataContext = null;
    }
}
