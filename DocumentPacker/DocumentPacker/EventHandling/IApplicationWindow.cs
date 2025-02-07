namespace DocumentPacker.EventHandling;

public interface IApplicationWindow : IApplicationView
{
    void Close();
    event EventHandler Closed;
    void Show();
}
