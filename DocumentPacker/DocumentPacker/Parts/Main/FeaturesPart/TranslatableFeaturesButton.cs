namespace DocumentPacker.Parts.Main.FeaturesPart;

using System.ComponentModel;
using System.Resources;
using System.Windows.Input;
using System.Windows.Media;
using Libs.Wpf.Localization;

/// <summary>
///     Describes the data of a feature button.
/// </summary>
internal class TranslatableFeaturesButton : TranslatableButton<ICommand>
{
    /// <summary>
    ///     The name of the description resource key.
    /// </summary>
    private readonly string? descriptionResourceKey;

    /// <summary>
    ///     The background.
    /// </summary>
    private Brush? background;

    /// <summary>
    ///     The translation of the <see cref="descriptionResourceKey" />.
    /// </summary>
    private string? descriptionTranslation;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TranslatableFeaturesButton" /> class.
    /// </summary>
    public TranslatableFeaturesButton(
        ICommand command,
        ImageSource? imageSource,
        ResourceManager resourceManager,
        string? labelResourceKey = null,
        string? toolTipResourceKey = null,
        string? descriptionResourceKey = null,
        Brush? background = null
    )
        : base(
            command,
            imageSource,
            resourceManager,
            labelResourceKey,
            toolTipResourceKey)
    {
        this.background = background;

        this.descriptionResourceKey = descriptionResourceKey;

        TranslationSource.Instance.PropertyChanged += this.OnCurrentCultureChanged;
        this.OnCurrentCultureChanged(
            this,
            new PropertyChangedEventArgs(string.Empty));
    }

    /// <summary>
    ///     Gets or sets the background.
    /// </summary>
    public Brush? Background
    {
        get => this.background;
        set =>
            this.SetField(
                ref this.background,
                value);
    }

    /// <summary>
    ///     Gets or sets the description translation.
    /// </summary>
    public string? DescriptionTranslation
    {
        get => this.descriptionTranslation;
        private set =>
            this.SetField(
                ref this.descriptionTranslation,
                value);
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public override void Dispose()
    {
        TranslationSource.Instance.PropertyChanged -= this.OnCurrentCultureChanged;

        base.Dispose();
    }

    /// <summary>
    ///     Handles the culture change of the application.
    /// </summary>
    /// <param name="sender">The sender that raised the event.</param>
    /// <param name="e">The args of the event.</param>
    private void OnCurrentCultureChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (this.descriptionResourceKey is not null)
        {
            this.DescriptionTranslation = this.GetTranslation(this.descriptionResourceKey);
        }
    }
}
