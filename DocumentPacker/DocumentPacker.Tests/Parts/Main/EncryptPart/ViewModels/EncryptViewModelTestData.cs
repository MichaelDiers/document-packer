namespace DocumentPacker.Tests.Parts.Main.EncryptPart.ViewModels;

using DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;
using DocumentPacker.Tests.Parts.Main.CreateConfigurationPart.ViewModels;

public class EncryptViewModelTestData : IDisposable
{
    public EncryptViewModelTestData()
    {
        this.ConfigurationViewModelTestData = new CreateConfigurationViewModelTestData();

        this.OutputFolder = CreateConfigurationViewModelTestData.CreateDirectory(
            CreateConfigurationViewModelTestData.TestDataFolder,
            Guid.NewGuid().ToString());

        this.InputFolder = CreateConfigurationViewModelTestData.CreateDirectory(
            this.OutputFolder,
            Guid.NewGuid().ToString());

        this.Items = this.ConfigurationViewModelTestData.Items.Select(
                item =>
                {
                    string value;
                    string? fileContent;

                    switch (item.itemType)
                    {
                        case ConfigurationItemType.File:
                            value = Path.Combine(
                                this.InputFolder,
                                $"{Guid.NewGuid()}.pdf");
                            fileContent = Guid.NewGuid().ToString();
                            File.WriteAllText(
                                value,
                                fileContent);
                            break;
                        case ConfigurationItemType.Text:
                            value = Guid.NewGuid().ToString();
                            fileContent = null;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    return (item.name, item.description, item.itemType, item.isRequired, value, fileContent);
                })
            .ToArray();
    }

    public CreateConfigurationViewModelTestData ConfigurationViewModelTestData { get; }
    public string InputFolder { get; }

    public ICollection<(string name, string description, ConfigurationItemType itemType, bool isRequired, string value,
        string? fileContent)> Items { get; }

    public string OutputFile =>
        Path.Combine(
            this.OutputFolder,
            $"{this.OutputFileName}{this.OutputFileExtension}");

    public string OutputFileExtension { get; } = ".dp";
    public string OutputFileName { get; } = Guid.NewGuid().ToString();

    public string OutputFolder { get; }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        this.ConfigurationViewModelTestData.Dispose();

        foreach (var item in this.Items.Where(item => item.itemType == ConfigurationItemType.File))
        {
            CreateConfigurationViewModelTestData.DeleteFile(
                Path.Combine(
                    this.OutputFolder,
                    item.value));
        }

        foreach (var item in this.Items.Where(item => item.itemType == ConfigurationItemType.File))
        {
            CreateConfigurationViewModelTestData.DeleteFile(item.value);
        }

        CreateConfigurationViewModelTestData.DeleteDirectory(this.InputFolder);

        CreateConfigurationViewModelTestData.DeleteFile(this.OutputFile);
        CreateConfigurationViewModelTestData.DeleteDirectory(this.OutputFolder);
    }

    public void Validate()
    {
        this.ConfigurationViewModelTestData.Validate();
        Assert.True(File.Exists(this.OutputFile));
    }
}
