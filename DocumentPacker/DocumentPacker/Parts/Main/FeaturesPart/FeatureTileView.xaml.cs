namespace DocumentPacker.Parts.Main.FeaturesPart;

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DocumentPacker.EventHandling;

/// <summary>
///     Interaction logic for FeatureTileView.xaml
/// </summary>
public partial class FeatureTileView
{
    /// <summary>
    ///     Extends the <see cref="FeatureTileView" /> by a <see cref="DependencyProperty" /> wrapped by
    ///     <see cref="ApplicationElementPart" />.
    /// </summary>
    public static readonly DependencyProperty ApplicationElementPartProperty = DependencyProperty.Register(
        nameof(FeatureTileView.ApplicationElementPart),
        typeof(ApplicationElementPart),
        typeof(FeatureTileView),
        new PropertyMetadata(default(ApplicationElementPart)));

    /// <summary>
    ///     Extends the <see cref="FeatureTileView" /> by a <see cref="DependencyProperty" /> wrapped by <see cref="Command" />
    ///     .
    /// </summary>
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(FeatureTileView.Command),
        typeof(ICommand),
        typeof(FeatureTileView),
        new PropertyMetadata(default(ICommand)));

    /// <summary>
    ///     Extends the <see cref="FeatureTileView" /> by a <see cref="DependencyProperty" /> wrapped by
    ///     <see cref="ImageSource" />.
    /// </summary>
    public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
        nameof(FeatureTileView.ImageSource),
        typeof(ImageSource),
        typeof(FeatureTileView),
        new PropertyMetadata(default(ImageSource)));

    public FeatureTileView()
    {
        this.InitializeComponent();
    }

    /// <summary>
    ///     Gets or sets the value of <see cref="ApplicationElementPartProperty" /> <see cref="DependencyProperty" />.
    /// </summary>
    public ApplicationElementPart ApplicationElementPart
    {
        get => (ApplicationElementPart) this.GetValue(FeatureTileView.ApplicationElementPartProperty);
        set =>
            this.SetValue(
                FeatureTileView.ApplicationElementPartProperty,
                value);
    }

    /// <summary>
    ///     Gets or sets the value of <see cref="CommandProperty" /> <see cref="DependencyProperty" />.
    /// </summary>
    public ICommand Command
    {
        get => (ICommand) this.GetValue(FeatureTileView.CommandProperty);
        set =>
            this.SetValue(
                FeatureTileView.CommandProperty,
                value);
    }

    /// <summary>
    ///     Gets or sets the value of <see cref="ImageSourceProperty" /> <see cref="DependencyProperty" />.
    /// </summary>
    public ImageSource ImageSource
    {
        get => (ImageSource) this.GetValue(FeatureTileView.ImageSourceProperty);
        set =>
            this.SetValue(
                FeatureTileView.ImageSourceProperty,
                value);
    }
}
