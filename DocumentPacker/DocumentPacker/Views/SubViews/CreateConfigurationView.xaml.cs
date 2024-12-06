namespace DocumentPacker.Views.SubViews;

using System.Windows.Controls;

/// <summary>
///     Interaction logic for CreateConfigurationView.xaml
/// </summary>
public partial class CreateConfigurationView : UserControl
{
    public CreateConfigurationView()
    {
        this.InitializeComponent();
        var row = 0;
        Grid.SetRow(
            this.Headline,
            row++);
        Grid.SetRow(
            this.ConfigurationItemList,
            row++);
        Grid.SetRow(
            this.AddConfigurationItem,
            row++);
    }
}
