namespace DocumentPacker.EventHandling;

internal static class QueueExtensions
{
    public static Queue<T> Enqueue<T>(this Queue<T> queue, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            queue.Enqueue(item);
        }

        return queue;
    }
}
