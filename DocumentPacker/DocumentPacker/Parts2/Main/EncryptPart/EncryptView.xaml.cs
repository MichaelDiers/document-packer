namespace DocumentPacker.Parts2.Main.EncryptPart;

using System.Windows;
using System.Windows.Controls;
using DocumentPacker.EventHandling;

/// <summary>
///     Interaction logic for EncryptView.xaml
/// </summary>
[DataContext(ApplicationElementPart.EncryptFeature)]
public partial class EncryptView : IApplicationView
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="EncryptView" /> class.
    /// </summary>
    public EncryptView()
    {
        this.InitializeComponent();
    }

    private void HandleDragAndDrop(object sender, DragEventArgs e, bool isPreview)
    {
        if (!e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            return;
        }

        var dropped = e.Data.GetData(DataFormats.FileDrop);
        if (dropped is not IEnumerable<string> files ||
            sender is not TextBox textBox ||
            textBox.DataContext is not IHandleDragAndDrop dragAndDropHandler)
        {
            return;
        }

        if (isPreview)
        {
            if (dragAndDropHandler.CanHandleDragAndDrop(files))
            {
                e.Effects = DragDropEffects.Move;
                e.Handled = true;
            }
        }
        else
        {
            dragAndDropHandler.HandleDragAndDrop(files);
            e.Handled = true;
        }
    }

    private void OnDrop(object sender, DragEventArgs e)
    {
        this.HandleDragAndDrop(
            sender,
            e,
            false);
    }

    private void OnPreviewDragOver(object sender, DragEventArgs e)
    {
        this.HandleDragAndDrop(
            sender,
            e,
            true);
    }
}
