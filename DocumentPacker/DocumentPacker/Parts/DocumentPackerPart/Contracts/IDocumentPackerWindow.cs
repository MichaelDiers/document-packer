namespace DocumentPacker.Parts.DocumentPackerPart.Contracts;

using System.Windows;

/// <summary>
///     Marker interface for the document packer window.
/// </summary>
public interface IDocumentPackerWindow
{
    /// <inheritdoc cref="FrameworkElement.DataContext" />
    object? DataContext { get; set; }

    /// <inheritdoc cref="System.Windows.Window.Show" />
    void Show();

    /// <inheritdoc cref="Window.ShowDialog" />
    bool? ShowDialog();
}
