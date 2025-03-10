namespace DocumentPacker.Commands;

using System.Windows.Input;
using System.Windows.Media.Imaging;
using Libs.Wpf.Commands;
using Libs.Wpf.ViewModels;

internal class
    SelectFileCommand<TCommandParameter>(
        ICommandFactory commandFactory,
        Action<TCommandParameter?, string> execute,
        string? filter = null
    ) : TranslatableButton<ICommand>(
    commandFactory.CreateOpenFileDialogCommand(
        execute,
        filter),
    new BitmapImage(
        new Uri(
            "pack://application:,,,/DocumentPacker;component/Assets/material_symbol_file_open.png",
            UriKind.Absolute)),
    CommandTranslations.ResourceManager,
    toolTipResourceKey: nameof(CommandTranslations.SelectFileCommandToolTip));
