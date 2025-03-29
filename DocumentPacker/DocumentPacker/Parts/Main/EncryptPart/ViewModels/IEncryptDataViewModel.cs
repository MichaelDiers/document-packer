using Libs.Wpf.Localization;
using System.Collections.ObjectModel;

namespace DocumentPacker.Parts.Main.EncryptPart.ViewModels
{
    public interface IEncryptDataViewModel
    {
        TranslatableAndValidable<string>? Description { get; set; }
        ObservableCollection<IEncryptDataItemViewModel>? Items { get; set; }
        string RsaPublicKey { get; set; }

        bool Validate();
    }
}