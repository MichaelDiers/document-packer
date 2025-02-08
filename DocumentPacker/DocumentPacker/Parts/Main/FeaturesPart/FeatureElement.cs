namespace DocumentPacker.Parts.Main.FeaturesPart;

using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;

/// <summary>
///     Describes a feature of the application.
/// </summary>
/// <param name="headline">A headline text that describes the feature.</param>
/// <param name="description">A longer description of the feature.</param>
/// <param name="applicationPart">The <see cref="ApplicationElementPart" /> that specifies the feature.</param>
/// <param name="iconPath">A path to an image.</param>
/// <seealso cref="DocumentPacker.Mvvm.BaseViewModel" />
internal class FeatureElement(
    string headline,
    string description,
    ApplicationElementPart applicationPart,
    string iconPath
) : BaseViewModel
{
    /// <summary>
    ///     The <see cref="ApplicationElementPart" /> that specifies the feature.
    /// </summary>
    private ApplicationElementPart applicationPart = applicationPart;

    /// <summary>
    ///     A longer description of the feature.
    /// </summary>
    private string description = description;

    /// <summary>
    ///     A headline text that describes the feature.
    /// </summary>
    private string headline = headline;

    /// <summary>
    ///     A path to an image.
    /// </summary>
    private string iconPath = iconPath;

    /// <summary>
    ///     Gets or sets the <see cref="ApplicationElementPart" /> that specifies the feature.
    /// </summary>
    public ApplicationElementPart ApplicationPart
    {
        get => this.applicationPart;
        set =>
            this.SetField(
                ref this.applicationPart,
                value);
    }

    /// <summary>
    ///     Gets or sets a longer description of the feature.
    /// </summary>
    public string Description
    {
        get => this.description;
        set =>
            this.SetField(
                ref this.description,
                value);
    }

    /// <summary>
    ///     Gets or sets a headline text that describes the feature.
    /// </summary>
    public string Headline
    {
        get => this.headline;
        set =>
            this.SetField(
                ref this.headline,
                value);
    }

    /// <summary>
    ///     Gets or sets a path to an image.
    /// </summary>
    public string IconPath
    {
        get => this.iconPath;
        set =>
            this.SetField(
                ref this.iconPath,
                value);
    }
}
