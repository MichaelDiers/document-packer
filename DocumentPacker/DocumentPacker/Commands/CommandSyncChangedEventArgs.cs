namespace DocumentPacker.Commands;

public class CommandSyncChangedEventArgs(bool isCommandActive) : EventArgs
{
    public bool IsCommandActive => isCommandActive;
}
