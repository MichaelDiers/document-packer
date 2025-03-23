namespace DocumentPacker.Commands;

using System.Windows.Input;
using DocumentPacker.Extensions;
using Libs.Wpf.Commands;
using Libs.Wpf.Localization;
using Microsoft.Win32;

/// <summary>
///     The data of a <see cref="TranslatableButton{TCommand}" /> that opens an <see cref="OpenFileDialog" />.
/// </summary>
/// <param name="commandFactory">A factory for creating commands.</param>
/// <param name="execute">The execute method of the command.</param>
/// <param name="filter">An optional filter of the <see cref="OpenFileDialog" /> for selectable files.</param>
internal class SelectFileCommand(ICommandFactory commandFactory, Action<string?, string> execute, string? filter = null)
    : TranslatableButton<ICommand>(
        commandFactory.CreateOpenFileDialogCommand(
            execute,
            filter),
        "material_symbol_file_open.png".ToPackImage(),
        CommandTranslations.ResourceManager,
        toolTipResourceKey: nameof(CommandTranslations.SelectFileCommandToolTip));
