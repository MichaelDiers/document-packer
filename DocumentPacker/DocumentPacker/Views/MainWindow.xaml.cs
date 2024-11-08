namespace DocumentPacker.Views;

using System.Windows;
using DocumentPacker.Contracts.ViewModels;
using DocumentPacker.Contracts.Views;

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
}
