namespace DocumentPacker.Parts2.Main.EncryptPart;

public interface IHandleDragAndDrop
{
    bool CanHandleDragAndDrop(IEnumerable<string> files);
    void HandleDragAndDrop(IEnumerable<string> files);
}
