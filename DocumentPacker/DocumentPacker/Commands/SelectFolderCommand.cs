namespace DocumentPacker.Commands;

using System.IO;
using System.Windows.Input;
using DocumentPacker.Extensions;
using Libs.Wpf.Commands;
using Libs.Wpf.Localization;
using Microsoft.Win32;

/// <summary>
///     The data of a <see cref="TranslatableButton{TCommand}" /> that opens an <see cref="OpenFolderDialog" />.
/// </summary>
/// <param name="commandFactory">A factory for creating commands.</param>
/// <param name="execute">The execute method of the command.</param>
internal class SelectFolderCommand(ICommandFactory commandFactory, Action<string> execute)
    : TranslatableButton<ICommand>(
        commandFactory.CreateOpenFolderDialogCommand(
            new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)),
            execute),
        "material_symbol_folder.png".ToBitmapImage(),
        CommandTranslations.ResourceManager,
        toolTipResourceKey: nameof(CommandTranslations.SelectFolderCommandToolTip));
