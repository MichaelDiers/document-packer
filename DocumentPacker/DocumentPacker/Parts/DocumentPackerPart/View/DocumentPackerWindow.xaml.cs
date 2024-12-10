namespace DocumentPacker.Parts.DocumentPackerPart.View;

using System.Windows;
using DocumentPacker.Parts.DocumentPackerPart.Contracts;

/// <summary>
///     Interaction logic for DocumentPackerWindow.xaml
/// </summary>
public partial class DocumentPackerWindow : Window, IDocumentPackerWindow
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DocumentPackerWindow" /> class.
    /// </summary>
    public DocumentPackerWindow()
    {
        this.InitializeComponent();
    }
}
