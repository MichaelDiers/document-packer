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

    /// <summary>
    ///     Handles drag and drop for files.
    /// </summary>
    /// <param name="sender">The sender that raised the event.</param>
    /// <param name="e">The <see cref="DragEventArgs" /> instance containing the event data.</param>
    /// <param name="isPreview">if set to <c>true</c> it is a preview only without changes.</param>
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

    /// <summary>
    ///     Handles the drop event.
    /// </summary>
    /// <param name="sender">The sender that raised the event.</param>
    /// <param name="e">The <see cref="DragEventArgs" /> instance containing the event data.</param>
    private void OnDrop(object sender, DragEventArgs e)
    {
        this.HandleDragAndDrop(
            sender,
            e,
            false);
    }

    /// <summary>
    ///     Handles the preview drag over event.
    /// </summary>
    /// <param name="sender">The sender that raised the event.</param>
    /// <param name="e">The <see cref="DragEventArgs" /> instance containing the event data.</param>
    private void OnPreviewDragOver(object sender, DragEventArgs e)
    {
        this.HandleDragAndDrop(
            sender,
            e,
            true);
    }
}
