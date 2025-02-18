namespace DocumentPacker.Parts.Main.FeaturesPart;

using System.Windows.Input;
using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
using Libs.Wpf.Commands;

/// <summary>
///     View model of the <see cref="FeaturesView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationBaseViewModel" />
internal class FeaturesViewModel(ICommandFactory commandFactory) : ApplicationBaseViewModel
{
    /// <summary>
    ///     Gets the request feature command.
    /// </summary>
    public ICommand RequestFeatureCommand =>
        commandFactory.CreateSyncCommand<ApplicationElementPart>(
            _ => true,
            applicationElementPart =>
            {
                this.InvokeShowViewRequested(
                    this,
                    new ShowViewRequestedEventArgs(applicationElementPart));
            });
}
