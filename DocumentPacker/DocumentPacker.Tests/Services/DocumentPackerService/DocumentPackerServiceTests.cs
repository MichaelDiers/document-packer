namespace DocumentPacker.Tests.Services.DocumentPackerService;

using System.Security.Cryptography;
using DocumentPacker.Services;
using DocumentPacker.Services.DocumentPackerService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class DocumentPackerServiceTests : IDisposable
{
    private readonly string archiveFileName;
    private readonly ICreateArchive documentPackerService;
    private readonly string rsaPublicKeyPem;
    private readonly FileInfo testFile1;
    private readonly FileInfo testFile2;
    private readonly string testText1;
    private readonly string testText2;

    public DocumentPackerServiceTests()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.TryAddDocumentPackerService();
        var host = builder.Build();
        this.documentPackerService = host.Services.GetRequiredService<IDocumentPackerService>();
        this.archiveFileName = Guid.NewGuid().ToString();

        using var rsa = RSA.Create(4096);
        this.rsaPublicKeyPem = rsa.ExportRSAPublicKeyPem();
        this.testText1 = "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, ";
        this.testText2 =
            "sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et";

        this.testFile1 = new FileInfo(Guid.NewGuid().ToString());
        File.WriteAllText(
            this.testFile1.FullName,
            this.testText1);
        this.testFile2 = new FileInfo(Guid.NewGuid().ToString());
        File.WriteAllText(
            this.testFile2.FullName,
            this.testText2);
    }

    [Fact]
    public void Add_File_ShouldFail_IfCalledMoreThanOnceForTheSameEntry()
    {
        var addData = this.documentPackerService.CreateArchive(this.archiveFileName)
            .SetupRsa(this.rsaPublicKeyPem)
            .SetupAes();
        addData.Add(
            nameof(this.testFile1),
            this.testFile1);

        Assert.Throws<InvalidOperationException>(
            () => addData.Add(
                nameof(this.testFile1),
                this.testFile1));
    }

    [Fact]
    public void Add_Text_ShouldFail_IfCalledMoreThanOnceForTheSameEntry()
    {
        var addData = this.documentPackerService.CreateArchive(this.archiveFileName)
            .SetupRsa(this.rsaPublicKeyPem)
            .SetupAes();
        addData.Add(
            nameof(this.testText1),
            this.testText1);

        Assert.Throws<InvalidOperationException>(
            () => addData.Add(
                nameof(this.testText1),
                this.testText1));
    }

    [Fact]
    public void CreateArchive_ShouldFail_IfCalledMoreThanOnce()
    {
        this.documentPackerService.CreateArchive(this.archiveFileName);
        Assert.Throws<InvalidOperationException>(() => this.documentPackerService.CreateArchive(this.archiveFileName));
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        if (File.Exists(this.archiveFileName))
        {
            File.Delete(this.archiveFileName);
        }

        if (this.testFile1.Exists)
        {
            this.testFile1.Delete();
        }

        if (this.testFile2.Exists)
        {
            this.testFile2.Delete();
        }
    }

    [Fact]
    public async Task ExecuteAsync()
    {
        await this.documentPackerService.CreateArchive(this.archiveFileName)
            .SetupRsa(this.rsaPublicKeyPem)
            .SetupAes()
            .Add(
                nameof(this.testText1),
                this.testText1)
            .Add(
                nameof(this.testText2),
                this.testText2)
            .Add(
                nameof(this.testFile1),
                this.testFile1)
            .Add(
                nameof(this.testFile2),
                this.testFile2)
            .ExecuteAsync(TestContext.Current.CancellationToken);

        var f = new FileInfo(this.archiveFileName);
        Assert.True(f.Exists);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_IfCalledMoreThanOnce()
    {
        var addData = this.documentPackerService.CreateArchive(this.archiveFileName)
            .SetupRsa(this.rsaPublicKeyPem)
            .SetupAes()
            .Add(
                nameof(this.testText1),
                this.testText1)
            .Add(
                nameof(this.testText2),
                this.testText2)
            .Add(
                nameof(this.testFile1),
                this.testFile1)
            .Add(
                nameof(this.testFile2),
                this.testFile2);

        await addData.ExecuteAsync(TestContext.Current.CancellationToken);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => addData.ExecuteAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public void SetupAes_ShouldFail_IfCalledMoreThanOnce()
    {
        var aesSetup = this.documentPackerService.CreateArchive(this.archiveFileName).SetupRsa(this.rsaPublicKeyPem);
        aesSetup.SetupAes();
        Assert.Throws<InvalidOperationException>(() => aesSetup.SetupAes());
    }

    [Fact]
    public void SetupRsa_ShouldFail_IfCalledMoreThanOnce()
    {
        var rsaSetup = this.documentPackerService.CreateArchive(this.archiveFileName);
        rsaSetup.SetupRsa(this.rsaPublicKeyPem);
        Assert.Throws<InvalidOperationException>(() => rsaSetup.SetupRsa(this.rsaPublicKeyPem));
    }
}
