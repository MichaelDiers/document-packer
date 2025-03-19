namespace DocumentPacker.Commands;

using System.IO;
using System.Windows.Input;
using DocumentPacker.Extensions;
using Libs.Wpf.Commands;
using Libs.Wpf.Localization;

internal class SelectFolderCommand(ICommandFactory commandFactory, Action<string> execute)
    : TranslatableButton<ICommand>(
        commandFactory.CreateOpenFolderDialogCommand(
            new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)),
            execute),
        "material_symbol_folder.png".ToBitmapImage(),
        CommandTranslations.ResourceManager,
        toolTipResourceKey: nameof(CommandTranslations.SelectFolderCommandToolTip));
