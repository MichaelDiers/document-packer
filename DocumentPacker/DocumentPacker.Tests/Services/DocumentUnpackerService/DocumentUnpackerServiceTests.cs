namespace DocumentPacker.Tests.Services.DocumentUnpackerService;

using System.Security.Cryptography;
using DocumentPacker.Services;
using DocumentPacker.Services.DocumentPackerService;
using DocumentPacker.Services.DocumentUnpackerService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class DocumentUnpackerServiceTests : IDisposable
{
    private readonly string archiveFileName;
    private readonly string destinationDirectory;
    private readonly IDocumentPackerService documentPackerService;
    private readonly IDocumentUnpackerService documentUnpackerService;
    private readonly string rsaPrivateKeyPem;
    private readonly string rsaPublicKeyPem;
    private readonly string testFile1;
    private readonly string testFile2;
    private readonly string testText1;
    private readonly string testText2;

    public DocumentUnpackerServiceTests()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.TryAddDocumentPackerService();
        builder.Services.TryAddDocumentUnpackerService();
        var host = builder.Build();
        this.documentPackerService = host.Services.GetRequiredService<IDocumentPackerService>();
        this.documentUnpackerService = host.Services.GetRequiredService<IDocumentUnpackerService>();
        this.archiveFileName = Guid.NewGuid().ToString();
        this.destinationDirectory = Guid.NewGuid().ToString();
        using var rsa = RSA.Create(4096);
        this.rsaPublicKeyPem = rsa.ExportRSAPublicKeyPem();
        this.rsaPrivateKeyPem = rsa.ExportRSAPrivateKeyPem();
        this.testText1 = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, ";
        this.testText2 =
            "sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et";

        this.testFile1 = Guid.NewGuid() + ".txt";
        File.WriteAllText(
            this.testFile1,
            this.testText1);

        this.testFile2 = Guid.NewGuid() + ".txt";
        File.WriteAllText(
            this.testFile2,
            this.testText2);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        if (File.Exists(this.archiveFileName))
        {
            File.Delete(this.archiveFileName);
        }

        if (Directory.Exists(this.destinationDirectory))
        {
            Directory.Delete(
                this.destinationDirectory,
                true);
        }
    }

    [Fact]
    public async Task PackAndUnpack()
    {
        await this.documentPackerService.CreateArchive(this.archiveFileName)
            .SetupRsa(this.rsaPublicKeyPem)
            .SetupAes()
            .Add(
                nameof(this.testText1),
                this.testText1)
            .Add(
                nameof(this.testFile1),
                new FileInfo(this.testFile1))
            .Add(
                nameof(this.testText2),
                this.testText2)
            .Add(
                nameof(this.testFile2),
                new FileInfo(this.testFile2))
            .ExecuteAsync(TestContext.Current.CancellationToken);

        Assert.True(File.Exists(this.archiveFileName));

        await this.documentUnpackerService.SetupArchive(
                new FileInfo(this.archiveFileName),
                new DirectoryInfo(this.destinationDirectory))
            .SetupRsa(this.rsaPrivateKeyPem)
            .SetupAes()
            .ExecuteAsync(TestContext.Current.CancellationToken);

        Assert.Equal(
            this.testText1,
            await File.ReadAllTextAsync(
                Path.Combine(
                    this.destinationDirectory,
                    nameof(this.testText1))));
        Assert.Equal(
            this.testText2,
            await File.ReadAllTextAsync(
                Path.Combine(
                    this.destinationDirectory,
                    nameof(this.testText2))));
        Assert.Equal(
            this.testText1,
            await File.ReadAllTextAsync(
                Path.Combine(
                    this.destinationDirectory,
                    nameof(this.testFile1))));
        Assert.Equal(
            this.testText2,
            await File.ReadAllTextAsync(
                Path.Combine(
                    this.destinationDirectory,
                    nameof(this.testFile2))));
    }
}
