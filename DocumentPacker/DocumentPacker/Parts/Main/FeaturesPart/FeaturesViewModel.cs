namespace DocumentPacker.Parts.Main.FeaturesPart;

using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
using DocumentPacker.Parts.Main.CreateConfigurationPart.Translations;
using DocumentPacker.Parts.Main.DecryptPart;
using DocumentPacker.Parts.Main.EncryptPart;
using Libs.Wpf.Commands;
using Libs.Wpf.ViewModels;

/// <summary>
///     View model of the <see cref="FeaturesView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
internal class FeaturesViewModel : ApplicationBaseViewModel
{
    /// <summary>
    ///     The features.
    /// </summary>
    private ObservableCollection<TranslatableFeaturesButton> features;

    /// <summary>
    ///     The description of the view.
    /// </summary>
    private Translatable viewDescription = new(
        FeaturesPartTranslation.ResourceManager,
        nameof(FeaturesPartTranslation.Description));

    /// <summary>
    ///     The headline of the view.
    /// </summary>
    private Translatable viewHeadline = new(
        FeaturesPartTranslation.ResourceManager,
        nameof(FeaturesPartTranslation.Headline));

    /// <summary>
    ///     View model of the <see cref="FeaturesView" />.
    /// </summary>
    /// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
    public FeaturesViewModel(ICommandFactory commandFactory)
    {
        this.features = new ObservableCollection<TranslatableFeaturesButton>(
            new[]
            {
                new TranslatableFeaturesButton(
                    commandFactory.CreateSyncCommand(
                        _ => true,
                        _ =>
                        {
                            this.InvokeShowViewRequested(
                                this,
                                new ShowViewRequestedEventArgs(ApplicationElementPart.EncryptFeature));
                        }),
                    new BitmapImage(
                        new Uri(
                            "pack://application:,,,/DocumentPacker;component/Assets/material_symbol_compress_96dp.png",
                            UriKind.Absolute)),
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
                    new BitmapImage(
                        new Uri(
                            "pack://application:,,,/DocumentPacker;component/Assets/material_symbol_expand_96dp.png",
                            UriKind.Absolute)),
                    DecryptPartTranslation.ResourceManager,
                    nameof(DecryptPartTranslation.Headline),
                    descriptionResourceKey: nameof(DecryptPartTranslation.Description)),
                new TranslatableFeaturesButton(
                    commandFactory.CreateSyncCommand(
                        _ => true,
                        _ =>
                        {
                            this.InvokeShowViewRequested(
                                this,
                                new ShowViewRequestedEventArgs(ApplicationElementPart.CreateConfiguration));
                        }),
                    new BitmapImage(
                        new Uri(
                            "pack://application:,,,/DocumentPacker;component/Assets/material_symbol_edit_square_96dp.png",
                            UriKind.Absolute)),
                    CreateConfigurationPartTranslation.ResourceManager,
                    nameof(CreateConfigurationPartTranslation.Headline),
                    descriptionResourceKey: nameof(CreateConfigurationPartTranslation.Description))
            });
    }

    /// <summary>
    ///     Gets or sets the features.
    /// </summary>
    public ObservableCollection<TranslatableFeaturesButton> Features
    {
        get => this.features;
        set =>
            this.SetField(
                ref this.features,
                value);
    }

    /// <summary>
    ///     Gets or sets the description of the view.
    /// </summary>
    public Translatable ViewDescription
    {
        get => this.viewDescription;
        set =>
            this.SetField(
                ref this.viewDescription,
                value);
    }

    /// <summary>
    ///     Gets or sets the headline of the view.
    /// </summary>
    public Translatable ViewHeadline
    {
        get => this.viewHeadline;
        set =>
            this.SetField(
                ref this.viewHeadline,
                value);
    }
}
