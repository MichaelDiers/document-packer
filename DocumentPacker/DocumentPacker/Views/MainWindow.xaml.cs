namespace DocumentPacker.Views;

using System.Windows;
using DocumentPacker.Contracts.ViewModels;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
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
