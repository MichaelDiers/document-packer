namespace DocumentPacker.Commands;

using Libs.Wpf.ViewModels;

public class CommandSyncChangedEventArgs(
    bool isCommandActive,
    TranslatableCancellableButton? translatableCancellableButton
) : EventArgs
{
    public bool IsCommandActive => isCommandActive;

    public TranslatableCancellableButton? TranslatableCancellableButton => translatableCancellableButton;
}
