namespace DocumentPacker.Parts.Main.EncryptPart.ViewModels;

using System.Windows.Input;
using DocumentPacker.Parts.SubParts.LoadConfigurationSubPart;
using Libs.Wpf.Commands;
using Libs.Wpf.Localization;

public interface IEncryptViewModel : IDisposable
{
    IEncryptDataViewModel? EncryptDataViewModel { get; set; }
    ILoadConfigurationViewModel LoadConfigurationViewModel { get; set; }
    TranslatableAndValidable<string> OutputFile { get; set; }
    TranslatableAndValidable<string> OutputFileExtension { get; }
    TranslatableAndValidable<string> OutputFolder { get; set; }
    TranslatableButton<IAsyncCommand> SaveCommand { get; set; }
    TranslatableButton<ICommand> SelectOutputFolderCommand { get; set; }
    Translatable ViewDescription { get; set; }
    Translatable ViewHeadline { get; set; }
}
