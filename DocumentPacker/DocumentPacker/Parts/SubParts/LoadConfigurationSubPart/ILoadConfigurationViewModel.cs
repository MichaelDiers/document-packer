namespace DocumentPacker.Parts.SubParts.LoadConfigurationSubPart;

using System.Security;
using System.Windows.Input;
using Libs.Wpf.Commands;
using Libs.Wpf.Localization;

public interface ILoadConfigurationViewModel : IDisposable
{
    TranslatableAndValidable<string> ConfigurationFile { get; }
    string ConfigurationFileFilter { get; set; }
    TranslatableButton<IAsyncCommand> LoadConfigurationCommand { get; set; }
    TranslatableAndValidable<SecureString> Password { get; set; }
    TranslatableButton<ICommand> SelectConfigurationFileCommand { get; set; }

    event EventHandler<EventArgs>? ConfigurationInvalidated;
    event EventHandler<LoadConfigurationEventArgs>? ConfigurationLoaded;
}
