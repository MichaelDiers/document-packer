namespace DocumentPacker.Commands;

using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Libs.Wpf.Commands;
using Libs.Wpf.ViewModels;

internal class SelectFolderCommand(ICommandFactory commandFactory, Action<string> execute)
    : TranslatableButton<ICommand>(
        commandFactory.CreateOpenFolderDialogCommand(
            new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)),
            execute),
        new BitmapImage(
            new Uri(
                "pack://application:,,,/DocumentPacker;component/Assets/material_symbol_folder.png",
                UriKind.Absolute)),
        CommandTranslations.ResourceManager,
        toolTipResourceKey: nameof(CommandTranslations.SelectFolderCommandToolTip));
