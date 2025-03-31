namespace DocumentPacker.Tests.Parts.Main.DecryptPart.ViewModels;

using DocumentPacker.Tests.Parts.Main.CreateConfigurationPart.ViewModels;
using DocumentPacker.Tests.Parts.Main.EncryptPart.ViewModels;

public class DecryptViewModelTestData : IDisposable
{
    public EncryptViewModelTestData EncryptViewModelTestData { get; } = new();

    public string OutputFolder { get; } = CreateConfigurationViewModelTestData.CreateDirectory(
        CreateConfigurationViewModelTestData.TestDataFolder,
        Guid.NewGuid().ToString());

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        CreateConfigurationViewModelTestData.DeleteDirectory(this.OutputFolder);

        this.EncryptViewModelTestData.Dispose();
    }

    public void Validate()
    {
        this.EncryptViewModelTestData.Validate();
    }
}
