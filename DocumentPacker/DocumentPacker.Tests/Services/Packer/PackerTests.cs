namespace DocumentPacker.Tests.Services.Packer;

using System.Security;
using System.Security.Cryptography;
using DocumentPacker.Extensions;
using DocumentPacker.Services;
using DocumentPacker.Services.Crypto;
using DocumentPacker.Services.Packer;
using DocumentPacker.Services.Zip;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class PackerTests : IDisposable
{
    private readonly ICryptoFactory cryptoFactory;

    private readonly string[] inputFiles =
    [
        Guid.NewGuid().ToString(),
        Guid.NewGuid().ToString()
    ];

    private readonly string[] inputTexts =
    [
        new byte[101].FillRandom().AsString(),
        new byte[101].FillRandom().AsString()
    ];

    private readonly string packageFile = $"{Guid.NewGuid().ToString()}.zip";
    private readonly IPackerFactory packerFactory;

    private readonly SecureString rsaPublicKey;
    private readonly IZipFileCreator zipFileCreator;

    public PackerTests()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.TryAddPackerServices();
        builder.Services.TryAddZipServices();
        builder.Services.TryAddCryptoServices();
        var host = builder.Build();
        this.packerFactory = host.Services.GetRequiredService<IPackerFactory>();
        this.zipFileCreator = host.Services.GetRequiredService<IZipFileCreator>();
        this.cryptoFactory = host.Services.GetRequiredService<ICryptoFactory>();

        var rsa = RSA.Create(4096);
        this.rsaPublicKey = rsa.ExportRSAPublicKeyPem().AsSecureString();

        foreach (var inputFile in this.inputFiles)
        {
            File.WriteAllBytes(
                inputFile,
                new byte[100].FillRandom());
        }
    }

    [Fact]
    public async Task AddFile()
    {
        var file = this.inputFiles.First();
        using (var packer = await this.packerFactory.CreatePackerAsync(
                   this.zipFileCreator,
                   this.cryptoFactory,
                   new FileInfo(this.packageFile),
                   this.rsaPublicKey,
                   TestContext.Current.CancellationToken))
        {
            await packer.AddAsync(
                file,
                new FileInfo(file),
                TestContext.Current.CancellationToken);
        }

        Assert.True(File.Exists(this.packageFile));
    }

    [Fact]
    public async Task AddFiles()
    {
        using (var packer = await this.packerFactory.CreatePackerAsync(
                   this.zipFileCreator,
                   this.cryptoFactory,
                   new FileInfo(this.packageFile),
                   this.rsaPublicKey,
                   TestContext.Current.CancellationToken))
        {
            foreach (var inputFile in this.inputFiles)
            {
                await packer.AddAsync(
                    inputFile,
                    new FileInfo(inputFile),
                    TestContext.Current.CancellationToken);
            }
        }

        Assert.True(File.Exists(this.packageFile));
    }

    [Fact]
    public async Task AddText()
    {
        using (var packer = await this.packerFactory.CreatePackerAsync(
                   this.zipFileCreator,
                   this.cryptoFactory,
                   new FileInfo(this.packageFile),
                   this.rsaPublicKey,
                   TestContext.Current.CancellationToken))
        {
            await packer.AddAsync(
                Guid.NewGuid().ToString(),
                this.inputTexts.First(),
                TestContext.Current.CancellationToken);
        }

        Assert.True(File.Exists(this.packageFile));
    }

    [Fact]
    public async Task AddTexts()
    {
        using (var packer = await this.packerFactory.CreatePackerAsync(
                   this.zipFileCreator,
                   this.cryptoFactory,
                   new FileInfo(this.packageFile),
                   this.rsaPublicKey,
                   TestContext.Current.CancellationToken))
        {
            foreach (var inputText in this.inputTexts)
            {
                await packer.AddAsync(
                    Guid.NewGuid().ToString(),
                    inputText,
                    TestContext.Current.CancellationToken);
            }
        }

        Assert.True(File.Exists(this.packageFile));
    }

    [Fact]
    public async Task Create()
    {
        using var packer = await this.packerFactory.CreatePackerAsync(
            this.zipFileCreator,
            this.cryptoFactory,
            new FileInfo(this.packageFile),
            this.rsaPublicKey,
            TestContext.Current.CancellationToken);

        Assert.IsAssignableFrom<IPacker>(packer);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        if (File.Exists(this.packageFile))
        {
            File.Delete(this.packageFile);
        }

        foreach (var inputFile in this.inputFiles)
        {
            if (File.Exists(inputFile))
            {
                File.Delete(inputFile);
            }
        }
    }
}
