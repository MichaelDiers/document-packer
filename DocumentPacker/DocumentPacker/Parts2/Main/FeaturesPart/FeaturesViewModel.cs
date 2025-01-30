namespace DocumentPacker.Parts2.Main.FeaturesPart;

using System.Collections.ObjectModel;
using System.Windows.Input;
using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
using DocumentPacker.Resources;

/// <summary>
///     View model of the <see cref="FeaturesView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationViewModel" />
internal class FeaturesViewModel : ApplicationViewModel
{
    /// <summary>
    ///     The supported features of the application.
    /// </summary>
    private ObservableCollection<FeatureElement> featureElements = new(
    [
        new FeatureElement(
            Translation.FeaturesPartEncryptHeadline,
            Translation.FeaturesPartEncryptDescription,
            ApplicationElementPart.EncryptFeature,
            "../../../Assets/material_symbol_compress_96dp.png"),
        new FeatureElement(
            Translation.FeaturesPartDecryptHeadline,
            Translation.FeaturesPartDecryptDescription,
            ApplicationElementPart.DecryptFeature,
            "../../../Assets/material_symbol_expand_96dp.png"),
        new FeatureElement(
            Translation.FeaturesPartCreateConfigHeadline,
            Translation.FeaturesPartCreateConfigDescription,
            ApplicationElementPart.CreateConfiguration,
            "../../../Assets/material_symbol_edit_square_96dp.png")
    ]);

    /// <summary>
    ///     The headline that is displayed in the view.
    /// </summary>
    private string headline = Translation.FeaturesPartHeadline;

    /// <summary>
    ///     Gets or sets the supported features of the application.
    /// </summary>
    public ObservableCollection<FeatureElement> FeatureElements
    {
        get => this.featureElements;
        set =>
            this.SetField(
                ref this.featureElements,
                value);
    }

    /// <summary>
    ///     Gets or sets the headline that is displayed in the view.
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
    ///     Gets the request feature command.
    /// </summary>
    public ICommand RequestFeatureCommand =>
        new SyncCommand(
            _ => true,
            obj =>
            {
                if (obj is not FeatureElement featureElement)
                {
                    return;
                }

                this.InvokeShowViewRequested(
                    this,
                    new ShowViewRequestedEventArgs(featureElement.ApplicationPart));
            });
}
