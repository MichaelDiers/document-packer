namespace DocumentPacker.Parts.WindowPart;

using System.ComponentModel;
using Libs.Wpf.ViewModels;

public interface ITranslatableCancellableButton : INotifyPropertyChanged
{
    TranslatableCancellableButton? TranslatableCancellableButton { get; }
}
