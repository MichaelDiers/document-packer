namespace DocumentPacker.EventHandling;

public interface IApplicationView : IDisposable
{
    object? DataContext { get; set; }
}
