namespace DocumentPacker.Parts.Main.FeaturesPart;

using System.Collections.ObjectModel;
using System.Windows.Input;
using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
using DocumentPacker.Parts.Main.CreateConfigurationPart;
using DocumentPacker.Parts.Main.DecryptPart;
using DocumentPacker.Parts.Main.EncryptPart;
using Libs.Wpf.Commands;

/// <summary>
///     View model of the <see cref="FeaturesView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
internal class FeaturesViewModel(ICommandFactory commandFactory) : ApplicationBaseViewModel
{
    /// <summary>
    ///     The supported features of the application.
    /// </summary>
    private ObservableCollection<FeatureElement> featureElements = new(
    [
        new FeatureElement(
            EncryptPartTranslation.Headline,
            EncryptPartTranslation.Description,
            ApplicationElementPart.EncryptFeature,
            "../../../Assets/material_symbol_compress_96dp.png"),
        new FeatureElement(
            DecryptPartTranslation.Headline,
            DecryptPartTranslation.Description,
            ApplicationElementPart.DecryptFeature,
            "../../../Assets/material_symbol_expand_96dp.png"),
        new FeatureElement(
            CreateConfigurationPartTranslation.Headline,
            CreateConfigurationPartTranslation.Description,
            ApplicationElementPart.CreateConfiguration,
            "../../../Assets/material_symbol_edit_square_96dp.png")
    ]);

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
    ///     Gets the request feature command.
    /// </summary>
    public ICommand RequestFeatureCommand =>
        commandFactory.CreateSyncCommand(
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
