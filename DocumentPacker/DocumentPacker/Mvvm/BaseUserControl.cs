namespace DocumentPacker.Mvvm;

using System.Windows.Controls;
using DocumentPacker.EventHandling;

public class BaseUserControl : UserControl, IDisposable
{
    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        if (this.DataContext is IApplicationViewModel viewModel)
        {
            viewModel.Dispose();
        }

        this.DataContext = null;
    }
}
