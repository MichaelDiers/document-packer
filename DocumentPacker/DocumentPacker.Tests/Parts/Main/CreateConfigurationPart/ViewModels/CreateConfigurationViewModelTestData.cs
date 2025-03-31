namespace DocumentPacker.Tests.Parts.Main.CreateConfigurationPart.ViewModels;

using System.Security;
using DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;

public class CreateConfigurationViewModelTestData : IDisposable
{
    public const string TestDataFolder = nameof(CreateConfigurationViewModelTestData.TestDataFolder);

    public CreateConfigurationViewModelTestData()
    {
        this.Password = new SecureString();
        foreach (var c in "password")
        {
            this.Password.AppendChar(c);
        }

        this.OutputFolder = CreateConfigurationViewModelTestData.CreateDirectory(
            CreateConfigurationViewModelTestData.TestDataFolder,
            this.CurrentTestFolderName);
    }

    public string CurrentTestFolderName { get; } = Guid.NewGuid().ToString();

    public string Description { get; } = $"The description of the configuration. ({Guid.NewGuid()})";

    public IEnumerable<(string name, string description, ConfigurationItemType itemType, bool isRequired)> Items
    {
        get;
    } =
    [
        ("name1", "description1", ConfigurationItemType.Text, false),
        ("name2", "description2", ConfigurationItemType.Text, true),
        ("name3", "description3", ConfigurationItemType.File, false),
        ("name4", "description4", ConfigurationItemType.File, true)
    ];

    public string OutputFolder { get; }

    public SecureString Password { get; }

    public string PrivateFile =>
        Path.Combine(
            this.OutputFolder,
            $"{this.PrivateFileName}{this.PrivateFileExtension}");

    public string PrivateFileExtension { get; } = ".private.dpc";

    public string PrivateFileName { get; } = Guid.NewGuid().ToString();

    public string PublicFile =>
        Path.Combine(
            this.OutputFolder,
            $"{this.PublicFileName}{this.PublicFileExtension}");

    public string PublicFileExtension { get; } = ".public.dpc";
    public string PublicFileName { get; } = Guid.NewGuid().ToString();

    public static string CreateDirectory(params string[] directories)
    {
        var current = string.Empty;
        foreach (var directory in directories)
        {
            current = Path.Combine(
                current,
                directory);

            if (!Directory.Exists(current))
            {
                Directory.CreateDirectory(current);
            }
        }

        return current;
    }

    public static void DeleteDirectory(string directory)
    {
        try
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(
                    directory,
                    true);
            }
        }
        catch
        {
            // ignore
        }
    }

    public static void DeleteFile(string file)
    {
        try
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
        catch
        {
            // ignore
        }
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        CreateConfigurationViewModelTestData.DeleteFile(this.PrivateFile);
        CreateConfigurationViewModelTestData.DeleteFile(this.PublicFile);
        CreateConfigurationViewModelTestData.DeleteDirectory(this.OutputFolder);
    }

    public void Validate()
    {
        Assert.True(File.Exists(this.PrivateFile));
        Assert.True(File.Exists(this.PublicFile));
    }
}
