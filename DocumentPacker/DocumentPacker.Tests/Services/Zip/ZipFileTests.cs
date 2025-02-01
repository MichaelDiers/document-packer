namespace DocumentPacker.Tests.Services.Zip;

using System.IO.Compression;
using DocumentPacker.Services;
using DocumentPacker.Services.Zip;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class ZipFileTests : IDisposable
{
    private readonly IEnumerable<(string filePath, string data, string entryName)> addFiles;
    private readonly IZipFileCreator zipFileCreator;

    private readonly string zipFilePath = Path.Combine(
        Path.GetTempPath(),
        Guid.NewGuid().ToString());

    public ZipFileTests()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.TryAddZipServices();
        var host = builder.Build();
        this.zipFileCreator = host.Services.GetRequiredService<IZipFileCreator>();

        this.addFiles =
        [
            (Path.Combine(
                    Path.GetTempPath(),
                    Guid.NewGuid().ToString()),
                "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.",
                Guid.NewGuid().ToString()),
            (Path.Combine(
                Path.GetTempPath(),
                Guid.NewGuid().ToString()), new string(
                'a',
                10000), Guid.NewGuid().ToString())
        ];

        foreach (var valueTuple in this.addFiles)
        {
            File.WriteAllText(
                valueTuple.filePath,
                valueTuple.data);
        }
    }

    [Fact]
    public async Task AddFile()
    {
        using (var archive = this.zipFileCreator.Create(this.zipFilePath))
        {
            foreach (var (filePath, _, entryName) in this.addFiles)
            {
                archive.AddFile(
                    filePath,
                    entryName);
            }
        }

        Assert.True(File.Exists(this.zipFilePath));
        await ZipFileTests.CheckZip(
            this.zipFilePath,
            this.addFiles.Select(addFile => (addFile.data, addFile.entryName)).ToArray());
    }

    [Fact]
    public async Task AddTextAndFilesAsync()
    {
        using (var archive = this.zipFileCreator.Create(this.zipFilePath))
        {
            foreach (var (filePath, data, entryName) in this.addFiles)
            {
                await archive.AddTextAsync(
                    data,
                    $"text_{entryName}",
                    CancellationToken.None);

                archive.AddFile(
                    filePath,
                    $"file_{entryName}");
            }
        }

        Assert.True(File.Exists(this.zipFilePath));
        await ZipFileTests.CheckZip(
            this.zipFilePath,
            this.addFiles.Select(addFile => (addFile.data, $"text_{addFile.entryName}"))
                .Concat(this.addFiles.Select(addFile => (addFile.data, $"file_{addFile.entryName}")))
                .ToArray());
    }

    [Fact]
    public async Task AddTextAsync()
    {
        using (var archive = this.zipFileCreator.Create(this.zipFilePath))
        {
            foreach (var (_, data, entryName) in this.addFiles)
            {
                await archive.AddTextAsync(
                    data,
                    entryName,
                    CancellationToken.None);
            }
        }

        Assert.True(File.Exists(this.zipFilePath));
        await ZipFileTests.CheckZip(
            this.zipFilePath,
            this.addFiles.Select(addFile => (addFile.data, addFile.entryName)).ToArray());
    }

    [Fact]
    public void Create()
    {
        using var zipFile = this.zipFileCreator.Create(this.zipFilePath);

        Assert.True(File.Exists(this.zipFilePath));
    }

    [Fact]
    public void Create_Fails_FileExists()
    {
        File.WriteAllText(
            this.zipFilePath,
            string.Empty);

        var exception = Assert.Throws<ArgumentException>(() => this.zipFileCreator.Create(this.zipFilePath));

        Assert.Equal(
            $"File already exists: {this.zipFilePath} (Parameter 'filePath')",
            exception.Message);
        Assert.True(File.Exists(this.zipFilePath));
    }

    [Fact]
    public void Create_Fails_FilePathNull()
    {
        Assert.Throws<ArgumentNullException>(() => this.zipFileCreator.Create(null));

        Assert.False(File.Exists(this.zipFilePath));
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void Create_Fails_FilePathWhitespace(string filePath)
    {
        var exception = Assert.Throws<ArgumentException>(() => this.zipFileCreator.Create(filePath));

        Assert.Equal(
            "The value cannot be an empty string or composed entirely of whitespace. (Parameter 'filePath')",
            exception.Message);
        Assert.False(File.Exists(this.zipFilePath));
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        if (File.Exists(this.zipFilePath))
        {
            File.Delete(this.zipFilePath);
        }

        foreach (var valueTuple in this.addFiles)
        {
            if (File.Exists(valueTuple.filePath))
            {
                File.Delete(valueTuple.filePath);
            }
        }
    }

    private static async Task CheckZip(string filePath, IList<(string data, string entryName)> expected)
    {
        using var zipFile = ZipFile.Open(
            filePath,
            ZipArchiveMode.Read);

        Assert.Equal(
            expected.Count,
            zipFile.Entries.Count);
        foreach (var (data, entryName) in expected)
        {
            var entry = zipFile.Entries.FirstOrDefault(entry => entry.Name == entryName);
            Assert.NotNull(entry);

            using var streamReader = new StreamReader(entry.Open());
            var actualContent = await streamReader.ReadToEndAsync();
            Assert.Equal(
                data,
                actualContent);
        }
    }
}
