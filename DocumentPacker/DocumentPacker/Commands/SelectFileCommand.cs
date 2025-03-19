namespace DocumentPacker.Commands;

using System.Windows.Input;
using DocumentPacker.Extensions;
using Libs.Wpf.Commands;
using Libs.Wpf.Localization;

internal class SelectFileCommand<TCommandParameter>(
    ICommandFactory commandFactory,
    Action<TCommandParameter?, string> execute,
    string? filter = null
) : TranslatableButton<ICommand>(
    commandFactory.CreateOpenFileDialogCommand(
        execute,
        filter),
    "material_symbol_file_open.png".ToBitmapImage(),
    CommandTranslations.ResourceManager,
    toolTipResourceKey: nameof(CommandTranslations.SelectFileCommandToolTip));
