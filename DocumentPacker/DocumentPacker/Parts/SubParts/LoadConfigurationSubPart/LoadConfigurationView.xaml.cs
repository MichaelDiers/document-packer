namespace DocumentPacker.Parts.SubParts.LoadConfigurationSubPart;

using System.Windows;
using System.Windows.Media;

/// <summary>
///     Interaction logic for LoadConfigurationView.xaml
/// </summary>
public partial class LoadConfigurationView
{
    /// <summary>
    ///     Extends the <see cref="LoadConfigurationView" /> by a <see cref="DependencyProperty" /> wrapped by
    ///     <see cref="GridBackground" />.
    /// </summary>
    public static readonly DependencyProperty GridBackgroundProperty = DependencyProperty.Register(
        nameof(LoadConfigurationView.GridBackground),
        typeof(Brush),
        typeof(LoadConfigurationView),
        new PropertyMetadata(default(Brush)));

    /// <summary>
    ///     Initializes a new instance of the <see cref="LoadConfigurationView" /> class.
    /// </summary>
    public LoadConfigurationView()
    {
        this.InitializeComponent();
    }

    /// <summary>
    ///     Gets or sets the value of <see cref="GridBackgroundProperty" /> <see cref="DependencyProperty" />.
    /// </summary>
    public Brush GridBackground
    {
        get => (Brush) this.GetValue(LoadConfigurationView.GridBackgroundProperty);
        set =>
            this.SetValue(
                LoadConfigurationView.GridBackgroundProperty,
                value);
    }
}
