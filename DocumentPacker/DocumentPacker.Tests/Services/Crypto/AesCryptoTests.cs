namespace DocumentPacker.Tests.Services.Crypto;

using System.Security.Cryptography;
using DocumentPacker.Services;
using DocumentPacker.Services.Crypto;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class AesCryptoTests : IDisposable
{
    public static TheoryData<int, int> AesBitLengthContentByteLengthTestData = new MatrixTheoryData<int, int>(
        [128, 192, 256],
        [8 * 8 - 1 + 200, 8 * 8 + 200, 8 * 8 + 1 + 200]);

    private readonly ICryptoFactory cryptoFactory;
    private readonly string decryptedFile = Guid.NewGuid().ToString();
    private readonly string encryptedFile = Guid.NewGuid().ToString();

    private readonly string inputFile = Guid.NewGuid().ToString();

    public AesCryptoTests()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.TryAddCryptoServices();
        var host = builder.Build();
        this.cryptoFactory = host.Services.GetRequiredService<ICryptoFactory>();
    }

    [Theory]
    [MemberData(nameof(AesCryptoTests.AesBitLengthContentByteLengthTestData))]
    public async Task DecryptAsync_Byte(int aesBits, int contentBytes)
    {
        var key = AesCryptoTests.CreateBytePassword(aesBits);
        var data = AesCryptoTests.CreateByteContent(contentBytes);

        var aes = this.cryptoFactory.CreateAes(key);

        var encrypted = await aes.EncryptAsync(
            data,
            TestContext.Current.CancellationToken);

        var decrypted = await aes.DecryptAsync(
            encrypted,
            TestContext.Current.CancellationToken);

        Assert.Equal(
            data,
            decrypted);
    }

    [Theory]
    [MemberData(nameof(AesCryptoTests.AesBitLengthContentByteLengthTestData))]
    public async Task DecryptAsync_File(int aesBits, int contentBytes)
    {
        var key = AesCryptoTests.CreateStringPassword(aesBits);
        var data = AesCryptoTests.CreateStringContent(contentBytes);
        await File.WriteAllTextAsync(
            this.inputFile,
            data);

        var aes = this.cryptoFactory.CreateAes(key);
        await aes.EncryptAsync(
            new FileInfo(this.inputFile),
            new FileInfo(this.encryptedFile),
            TestContext.Current.CancellationToken);

        Assert.True(File.Exists(this.encryptedFile));

        await aes.DecryptAsync(
            new FileInfo(this.encryptedFile),
            new FileInfo(this.decryptedFile),
            TestContext.Current.CancellationToken);
    }

    [Theory]
    [MemberData(nameof(AesCryptoTests.AesBitLengthContentByteLengthTestData))]
    public async Task DecryptAsync_Stream(int aesBits, int contentBytes)
    {
        var key = AesCryptoTests.CreateBytePassword(aesBits);
        var data = AesCryptoTests.CreateByteContent(contentBytes);

        var aes = this.cryptoFactory.CreateAes(key);

        await using var inputStream = new MemoryStream(data);
        await using var outputStream = new MemoryStream();
        await aes.EncryptAsync(
            inputStream,
            outputStream,
            TestContext.Current.CancellationToken);

        var encrypted = outputStream.ToArray();

        await using var encryptedStream = new MemoryStream(encrypted);
        await using var decryptedStream = new MemoryStream();
        await aes.DecryptAsync(
            encryptedStream,
            decryptedStream,
            TestContext.Current.CancellationToken);

        var decrypted = decryptedStream.ToArray();
        Assert.Equal(
            data,
            decrypted);
    }

    [Theory]
    [MemberData(nameof(AesCryptoTests.AesBitLengthContentByteLengthTestData))]
    public async Task DecryptAsync_String(int aesBits, int contentBytes)
    {
        var key = AesCryptoTests.CreateStringPassword(aesBits);
        var data = AesCryptoTests.CreateStringContent(contentBytes);

        var aes = this.cryptoFactory.CreateAes(key);
        var encrypted = await aes.EncryptAsync(
            data,
            TestContext.Current.CancellationToken);
        var decrypted = await aes.DecryptAsync(
            encrypted,
            TestContext.Current.CancellationToken);

        Assert.Equal(
            data,
            decrypted);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        foreach (var file in new[]
                 {
                     this.inputFile,
                     this.encryptedFile,
                     this.decryptedFile
                 })
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
    }

    [Theory]
    [MemberData(nameof(AesCryptoTests.AesBitLengthContentByteLengthTestData))]
    public async Task EncryptAsync_Byte(int aesBits, int contentBytes)
    {
        var key = AesCryptoTests.CreateBytePassword(aesBits);
        var data = AesCryptoTests.CreateByteContent(contentBytes);

        var aes = this.cryptoFactory.CreateAes(key);
        await aes.EncryptAsync(
            data,
            TestContext.Current.CancellationToken);
    }

    [Theory]
    [MemberData(nameof(AesCryptoTests.AesBitLengthContentByteLengthTestData))]
    public async Task EncryptAsync_File(int aesBits, int contentBytes)
    {
        var key = AesCryptoTests.CreateStringPassword(aesBits);
        var data = AesCryptoTests.CreateStringContent(contentBytes);
        await File.WriteAllTextAsync(
            this.inputFile,
            data);

        var aes = this.cryptoFactory.CreateAes(key);
        await aes.EncryptAsync(
            new FileInfo(this.inputFile),
            new FileInfo(this.encryptedFile),
            TestContext.Current.CancellationToken);

        Assert.True(File.Exists(this.encryptedFile));
    }

    [Theory]
    [MemberData(nameof(AesCryptoTests.AesBitLengthContentByteLengthTestData))]
    public async Task EncryptAsync_Stream(int aesBits, int contentBytes)
    {
        var key = AesCryptoTests.CreateBytePassword(aesBits);
        var data = AesCryptoTests.CreateByteContent(contentBytes);

        var aes = this.cryptoFactory.CreateAes(key);

        await using var input = new MemoryStream(data);
        await using var output = new MemoryStream();
        await aes.EncryptAsync(
            input,
            output,
            TestContext.Current.CancellationToken);

        var encrypted = output.ToArray();
        Assert.NotNull(encrypted);
    }

    [Theory]
    [MemberData(nameof(AesCryptoTests.AesBitLengthContentByteLengthTestData))]
    public async Task EncryptAsync_String(int aesBits, int contentBytes)
    {
        var key = AesCryptoTests.CreateStringPassword(aesBits);
        var data = AesCryptoTests.CreateStringContent(contentBytes);

        var aes = this.cryptoFactory.CreateAes(key);
        await aes.EncryptAsync(
            data,
            TestContext.Current.CancellationToken);
    }

    private static byte[] CreateByteContent(int byteLength)
    {
        var content = new byte[byteLength];
        RandomNumberGenerator.Create().GetBytes(content);
        return content;
    }

    private static byte[] CreateBytePassword(int bits)
    {
        var byteLength = bits / 8;
        var password = new byte[byteLength];
        RandomNumberGenerator.Create().GetBytes(password);
        return password;
    }

    private static string CreateStringContent(int contentBytes)
    {
        return Convert.ToBase64String(AesCryptoTests.CreateByteContent(contentBytes));
    }

    private static string CreateStringPassword(int aesBits)
    {
        return Convert.ToBase64String(AesCryptoTests.CreateBytePassword(aesBits));
    }
}
