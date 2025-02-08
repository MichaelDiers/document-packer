namespace DocumentPacker.Parts.Main.FeaturesPart;

using DocumentPacker.EventHandling;

/// <summary>
///     Interaction logic for FeatureView.xaml
/// </summary>
[DataContext(ApplicationElementPart.Features)]
public partial class FeaturesView : IApplicationView
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="FeaturesView" /> class.
    /// </summary>
    public FeaturesView()
    {
        this.InitializeComponent();
    }
}
