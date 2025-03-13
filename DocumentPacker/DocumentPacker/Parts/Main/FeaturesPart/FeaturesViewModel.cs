namespace DocumentPacker.Parts.Main.FeaturesPart;

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using DocumentPacker.EventHandling;
using DocumentPacker.Extensions;
using DocumentPacker.Mvvm;
using DocumentPacker.Parts.Main.CreateConfigurationPart.Translations;
using DocumentPacker.Parts.Main.DecryptPart;
using DocumentPacker.Parts.Main.EncryptPart.Translations;
using Libs.Wpf.Commands;
using Libs.Wpf.ViewModels;

/// <summary>
///     View model of the <see cref="FeaturesView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
internal class FeaturesViewModel : ApplicationBaseViewModel
{
    /// <summary>
    ///     View model of the <see cref="FeaturesView" />.
    /// </summary>
    /// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
    public FeaturesViewModel(ICommandFactory commandFactory)
    {
        this.Features = new ObservableCollection<TranslatableFeaturesButton>(
            new[]
            {
                new TranslatableFeaturesButton(
                    commandFactory.CreateSyncCommand(
                        _ =>
                        {
                            this.InvokeShowViewRequested(
                                this,
                                new ShowViewRequestedEventArgs(ApplicationElementPart.CreateConfiguration));
                        }),
                    "material_symbol_edit_square_96dp.png".ToBitmapImage(),
                    CreateConfigurationPartTranslation.ResourceManager,
                    nameof(CreateConfigurationPartTranslation.ViewHeadline),
                    descriptionResourceKey: nameof(CreateConfigurationPartTranslation.ViewDescription),
                    background: new LinearGradientBrush(
                        (Color) ColorConverter.ConvertFromString("#ccdaff"),
                        (Color) ColorConverter.ConvertFromString("#644dcb"),
                        new Point(
                            0,
                            0),
                        new Point(
                            1,
                            1))),
                new TranslatableFeaturesButton(
                    commandFactory.CreateSyncCommand(
                        _ => true,
                        _ =>
                        {
                            this.InvokeShowViewRequested(
                                this,
                                new ShowViewRequestedEventArgs(ApplicationElementPart.EncryptFeature));
                        }),
                    "material_symbol_compress_96dp.png".ToBitmapImage(),
                    EncryptPartTranslation.ResourceManager,
                    nameof(EncryptPartTranslation.Headline),
                    descriptionResourceKey: nameof(EncryptPartTranslation.Description)),
                new TranslatableFeaturesButton(
                    commandFactory.CreateSyncCommand(
                        _ => true,
                        _ =>
                        {
                            this.InvokeShowViewRequested(
                                this,
                                new ShowViewRequestedEventArgs(ApplicationElementPart.DecryptFeature));
                        }),
                    "material_symbol_expand_96dp.png".ToBitmapImage(),
                    DecryptPartTranslation.ResourceManager,
                    nameof(DecryptPartTranslation.Headline),
                    descriptionResourceKey: nameof(DecryptPartTranslation.Description))
            });
    }

    /// <summary>
    ///     Gets the list of features.
    /// </summary>
    public ObservableCollection<TranslatableFeaturesButton> Features { get; }

    /// <summary>
    ///     Gets the description of the view.
    /// </summary>
    public Translatable ViewDescription { get; } = new(
        FeaturesPartTranslation.ResourceManager,
        nameof(FeaturesPartTranslation.Description));

    /// <summary>
    ///     Gets the headline of the view.
    /// </summary>
    public Translatable ViewHeadline { get; } = new(
        FeaturesPartTranslation.ResourceManager,
        nameof(FeaturesPartTranslation.Headline));
}
