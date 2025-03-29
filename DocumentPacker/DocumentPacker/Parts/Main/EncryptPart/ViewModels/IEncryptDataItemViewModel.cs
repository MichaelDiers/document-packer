namespace DocumentPacker.Parts.Main.EncryptPart.ViewModels;

using System.Windows.Input;
using DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;
using Libs.Wpf.Localization;

public interface IEncryptDataItemViewModel
{
    ConfigurationItemType ConfigurationItemType { get; set; }
    TranslatableAndValidable<string> Description { get; set; }
    TranslatableAndValidable<string> Id { get; }
    TranslatableAndValidable<bool> IsRequired { get; set; }
    TranslatableButton<ICommand> SelectFileCommand { get; set; }
    TranslatableAndValidable<string> Value { get; set; }

    bool Validate();
}
