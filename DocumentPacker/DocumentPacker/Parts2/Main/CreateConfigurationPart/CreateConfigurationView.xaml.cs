namespace DocumentPacker.Parts2.Main.CreateConfigurationPart;

using DocumentPacker.EventHandling;

/// <summary>
///     Interaction logic for CreateConfigurationView.xaml
/// </summary>
[DataContext(ApplicationElementPart.Main)]
public partial class CreateConfigurationView : IApplicationView
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CreateConfigurationView" /> class.
    /// </summary>
    public CreateConfigurationView()
    {
        this.InitializeComponent();
    }
}
