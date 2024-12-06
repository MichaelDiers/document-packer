namespace DocumentPacker.ViewModels.SubViewModels;

using System.Collections.ObjectModel;
using System.Windows.Input;
using DocumentPacker.Contracts;
using DocumentPacker.Contracts.ViewModels.SubViewModels;

internal class CreateConfigurationViewModel : SubViewModel, ICreateConfigurationViewModel
{
    private ObservableCollection<IConfigurationItemViewModel> configurationItems = new();

    public CreateConfigurationViewModel()
        : base(SubViewId.CreateConfiguration)
    {
        this.AddConfigurationItemCommand = new SyncCommand(
            _ => true,
            _ => { this.ConfigurationItems.Add(new ConfigurationItemViewModel()); });
    }

    public ICommand AddConfigurationItemCommand { get; }

    public ObservableCollection<IConfigurationItemViewModel> ConfigurationItems
    {
        get => this.configurationItems;
        set =>
            this.SetField(
                ref this.configurationItems,
                value);
    }
}
