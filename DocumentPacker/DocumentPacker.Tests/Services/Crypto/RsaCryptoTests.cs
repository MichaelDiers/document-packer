namespace DocumentPacker.Tests.Services.Crypto;

using System.Security.Cryptography;
using DocumentPacker.Extensions;
using DocumentPacker.Services;
using DocumentPacker.Services.Crypto;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class RsaCryptoTests : IDisposable
{
    public static TheoryData<int, int> RsaKeySizeDataByteLengthTestData = new MatrixTheoryData<int, int>(
        [2048, 4096],
        [8 * 8 - 1, 8 * 8, 8 * 8 + 1]);

    private readonly ICryptoFactory cryptoFactory;
    private readonly string decryptedTempFile = Guid.NewGuid().ToString();
    private readonly string encryptedTempFile = Guid.NewGuid().ToString();
    private readonly string inputTempFile = Guid.NewGuid().ToString();

    public RsaCryptoTests()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.TryAddCryptoServices();
        var host = builder.Build();
        this.cryptoFactory = host.Services.GetRequiredService<ICryptoFactory>();
    }

    [Theory]
    [MemberData(nameof(RsaCryptoTests.RsaKeySizeDataByteLengthTestData))]
    public async Task DecryptAsync_Byte(int keySize, int dataBytes)
    {
        var keyCreator = RSA.Create(keySize);
        var publicKey = keyCreator.ExportRSAPublicKeyPem();
        var privateKey = keyCreator.ExportRSAPrivateKeyPem();

        var data = new byte[dataBytes].FillRandom();

        var encrypted = await this.cryptoFactory.CreateRsa(
                null,
                publicKey)
            .EncryptAsync(
                data,
                TestContext.Current.CancellationToken);

        var decrypted = await this.cryptoFactory.CreateRsa(
                privateKey,
                null)
            .DecryptAsync(
                encrypted,
                TestContext.Current.CancellationToken);

        Assert.Equal(
            data,
            decrypted);
    }

    [Theory]
    [MemberData(nameof(RsaCryptoTests.RsaKeySizeDataByteLengthTestData))]
    public async Task DecryptAsync_File(int keySize, int dataBytes)
    {
        var keyCreator = RSA.Create(keySize);
        var publicKey = keyCreator.ExportRSAPublicKeyPem();
        var privateKey = keyCreator.ExportRSAPrivateKeyPem();

        var data = new byte[dataBytes].FillRandom();
        await File.WriteAllBytesAsync(
            this.inputTempFile,
            data,
            TestContext.Current.CancellationToken);

        await this.cryptoFactory.CreateRsa(
                null,
                publicKey)
            .EncryptAsync(
                new FileInfo(this.inputTempFile),
                new FileInfo(this.encryptedTempFile),
                TestContext.Current.CancellationToken);

        await this.cryptoFactory.CreateRsa(
                privateKey,
                null)
            .DecryptAsync(
                new FileInfo(this.encryptedTempFile),
                new FileInfo(this.decryptedTempFile),
                TestContext.Current.CancellationToken);

        Assert.Equal(
            data,
            await File.ReadAllBytesAsync(
                this.decryptedTempFile,
                TestContext.Current.CancellationToken));
    }

    [Theory]
    [MemberData(nameof(RsaCryptoTests.RsaKeySizeDataByteLengthTestData))]
    public async Task DecryptAsync_Stream(int keySize, int dataBytes)
    {
        var keyCreator = RSA.Create(keySize);
        var publicKey = keyCreator.ExportRSAPublicKeyPem();
        var privateKey = keyCreator.ExportRSAPrivateKeyPem();

        var data = new byte[dataBytes].FillRandom();

        await using var inputStream = new MemoryStream(data);
        await using var encryptedStream = new MemoryStream();

        await this.cryptoFactory.CreateRsa(
                null,
                publicKey)
            .EncryptAsync(
                inputStream,
                encryptedStream,
                TestContext.Current.CancellationToken);
        var encrypted = encryptedStream.ToArray();

        await using var encryptedInputStream = new MemoryStream(encrypted);
        await using var decryptedStream = new MemoryStream();
        await this.cryptoFactory.CreateRsa(
                privateKey,
                null)
            .DecryptAsync(
                encryptedInputStream,
                decryptedStream,
                TestContext.Current.CancellationToken);

        var decrypted = decryptedStream.ToArray();
        Assert.Equal(
            data,
            decrypted);
    }

    [Theory]
    [MemberData(nameof(RsaCryptoTests.RsaKeySizeDataByteLengthTestData))]
    public async Task DecryptAsync_String(int keySize, int dataBytes)
    {
        var keyCreator = RSA.Create(keySize);
        var publicKey = keyCreator.ExportRSAPublicKeyPem();
        var privateKey = keyCreator.ExportRSAPrivateKeyPem();

        var data = new byte[dataBytes].FillRandom().AsString();

        var encrypted = await this.cryptoFactory.CreateRsa(
                null,
                publicKey)
            .EncryptAsync(
                data,
                TestContext.Current.CancellationToken);

        var decrypted = await this.cryptoFactory.CreateRsa(
                privateKey,
                null)
            .DecryptAsync(
                encrypted,
                TestContext.Current.CancellationToken);

        Assert.Equal(
            data,
            decrypted);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        if (File.Exists(this.inputTempFile))
        {
            File.Delete(this.inputTempFile);
        }

        if (File.Exists(this.encryptedTempFile))
        {
            File.Delete(this.encryptedTempFile);
        }

        if (File.Exists(this.decryptedTempFile))
        {
            File.Delete(this.decryptedTempFile);
        }
    }

    [Theory]
    [MemberData(nameof(RsaCryptoTests.RsaKeySizeDataByteLengthTestData))]
    public async Task EncryptAsync_Byte(int keySize, int dataBytes)
    {
        var keyCreator = RSA.Create(keySize);
        var publicKey = keyCreator.ExportRSAPublicKeyPem();

        var data = new byte[dataBytes].FillRandom();

        var rsa = this.cryptoFactory.CreateRsa(
            null,
            publicKey);
        _ = await rsa.EncryptAsync(
            data,
            TestContext.Current.CancellationToken);
    }

    [Theory]
    [MemberData(nameof(RsaCryptoTests.RsaKeySizeDataByteLengthTestData))]
    public async Task EncryptAsync_File(int keySize, int dataBytes)
    {
        var keyCreator = RSA.Create(keySize);
        var publicKey = keyCreator.ExportRSAPublicKeyPem();

        var data = new byte[dataBytes].FillRandom();
        await File.WriteAllBytesAsync(
            this.inputTempFile,
            data,
            TestContext.Current.CancellationToken);

        await this.cryptoFactory.CreateRsa(
                null,
                publicKey)
            .EncryptAsync(
                new FileInfo(this.inputTempFile),
                new FileInfo(this.encryptedTempFile),
                TestContext.Current.CancellationToken);
    }

    [Theory]
    [MemberData(nameof(RsaCryptoTests.RsaKeySizeDataByteLengthTestData))]
    public async Task EncryptAsync_Stream(int keySize, int dataBytes)
    {
        var keyCreator = RSA.Create(keySize);
        var publicKey = keyCreator.ExportRSAPublicKeyPem();

        var data = new byte[dataBytes].FillRandom();

        await using var inputStream = new MemoryStream(data);
        await using var encryptedStream = new MemoryStream();

        await this.cryptoFactory.CreateRsa(
                null,
                publicKey)
            .EncryptAsync(
                inputStream,
                encryptedStream,
                TestContext.Current.CancellationToken);
        _ = encryptedStream.ToArray();
    }

    [Theory]
    [MemberData(nameof(RsaCryptoTests.RsaKeySizeDataByteLengthTestData))]
    public async Task EncryptAsync_String(int keySize, int dataBytes)
    {
        var keyCreator = RSA.Create(keySize);
        var publicKey = keyCreator.ExportRSAPublicKeyPem();

        var data = new byte[dataBytes].FillRandom().AsString();

        _ = await this.cryptoFactory.CreateRsa(
                null,
                publicKey)
            .EncryptAsync(
                data,
                TestContext.Current.CancellationToken);
    }
}
