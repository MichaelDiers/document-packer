namespace DocumentPacker.Views;

using System.Windows;
using System.Windows.Input;
using DocumentPacker.Contracts.ViewModels;
using DocumentPacker.Contracts.Views;
using DocumentPacker.ViewModels;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, IMainWindow
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MainWindow" /> class.
    /// </summary>
    /// <param name="mainViewModel">View model that is used as <see cref="FrameworkElement.DataContext" />.</param>
    public MainWindow(IMainViewModel mainViewModel)
    {
        this.InitializeComponent();
        this.DataContext = mainViewModel;
    }

    /// <summary>
    ///     Suppress tab key if a command is active.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs" /> instance containing the event data.</param>
    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key is Key.Tab or Key.OemBackTab && TaskCommand.IsExecutingCommands)
        {
            e.Handled = true;
        }
    }
}
