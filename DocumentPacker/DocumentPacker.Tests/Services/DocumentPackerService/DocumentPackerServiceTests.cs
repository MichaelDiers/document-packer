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
    }

    [Fact]
    public async Task CreateArchive()
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
            .ExecuteAsync(TestContext.Current.CancellationToken);

        var f = new FileInfo(this.archiveFileName);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        if (File.Exists(this.archiveFileName))
        {
            File.Delete(this.archiveFileName);
        }
    }
}
