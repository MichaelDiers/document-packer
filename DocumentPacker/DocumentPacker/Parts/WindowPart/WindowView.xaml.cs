namespace DocumentPacker.Parts.WindowPart;

using System.ComponentModel;
using System.Windows;
using DocumentPacker.EventHandling;

/// <summary>
///     Interaction logic for WindowView.xaml
/// </summary>
[DataContext(ApplicationElementPart.Window)]
public partial class WindowView : IApplicationWindow
{
    private Window? cancelWindow;

    /// <summary>
    ///     Initializes a new instance of the <see cref="WindowView" /> class.
    /// </summary>
    public WindowView()
    {
        this.InitializeComponent();

        this.DataContextChanged += this.OnDataContextChanged;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        if (this.DataContext is IApplicationViewModel viewModel)
        {
            viewModel.Dispose();
        }

        this.DataContext = null;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is ITranslatableCancellableButton oldTranslatableCancellableButton)
        {
            oldTranslatableCancellableButton.PropertyChanged -= this.OnDataContextPropertyChanged;
            if (this.cancelWindow is not null)
            {
                this.cancelWindow.Close();
                this.cancelWindow = null;
            }
        }

        if (e.NewValue is ITranslatableCancellableButton newTranslatableCancellableButton)
        {
            newTranslatableCancellableButton.PropertyChanged += this.OnDataContextPropertyChanged;
        }
    }

    private void OnDataContextPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!string.Equals(
                e.PropertyName,
                nameof(ITranslatableCancellableButton.TranslatableCancellableButton)))
        {
            return;
        }

        if (Application.Current.Dispatcher.Thread != Thread.CurrentThread)
        {
            Application.Current.Dispatcher.Invoke(
                () => this.OnDataContextPropertyChanged(
                    sender,
                    e));
            return;
        }

        if (this.DataContext is not ITranslatableCancellableButton translatableCancellableButton)
        {
            return;
        }

        if (this.cancelWindow is not null)
        {
            this.cancelWindow.Close();
            this.cancelWindow = null;
        }

        if (translatableCancellableButton.TranslatableCancellableButton is not null)
        {
            this.cancelWindow = new CancelCommandWindow
            {
                DataContext = translatableCancellableButton.TranslatableCancellableButton,
                Owner = this
            };
            this.cancelWindow.Show();
        }
    }
}
