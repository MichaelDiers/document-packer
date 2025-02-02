namespace DocumentPacker.Tests.Services.Crypto;

using System.Security.Cryptography;
using DocumentPacker.Services;
using DocumentPacker.Services.Crypto;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class AesCryptoTests
{
    public static TheoryData<int, int> AesBitLengthContentByteLengthTestData = new MatrixTheoryData<int, int>(
        [128, 192, 256],
        [8 * 8 - 1 + 200, 8 * 8 + 200, 8 * 8 + 1 + 200]);

    private readonly ICryptoFactory cryptoFactory;

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
        var key = this.CreateBytePassword(aesBits);
        var data = this.CreateByteContent(contentBytes);

        var aes = this.cryptoFactory.CreateAes(key);

        var (header, encrypted) = await aes.EncryptAsync(
            data,
            CancellationToken.None);

        var decrypted = await aes.DecryptAsync(
            header,
            encrypted,
            CancellationToken.None);

        Assert.Equal(
            data,
            decrypted);
    }

    [Theory]
    [MemberData(nameof(AesCryptoTests.AesBitLengthContentByteLengthTestData))]
    public async Task EncryptAsync_Byte(int aesBits, int contentBytes)
    {
        var key = this.CreateBytePassword(aesBits);
        var data = this.CreateByteContent(contentBytes);

        var aes = this.cryptoFactory.CreateAes(key);
        await aes.EncryptAsync(
            data,
            CancellationToken.None);
    }

    [Theory]
    [MemberData(nameof(AesCryptoTests.AesBitLengthContentByteLengthTestData))]
    public async Task EncryptAsync_String(int aesBits, int contentBytes)
    {
        var key = this.CreateStringPassword(aesBits);
        var data = this.CreateStringContent(contentBytes);

        var aes = this.cryptoFactory.CreateAes(key);
        await aes.EncryptAsync(
            data,
            CancellationToken.None);
    }

    private byte[] CreateByteContent(int byteLength)
    {
        var content = new byte[byteLength];
        RandomNumberGenerator.Create().GetBytes(content);
        return content;
    }

    private byte[] CreateBytePassword(int bits)
    {
        var byteLength = bits / 8;
        var password = new byte[byteLength];
        RandomNumberGenerator.Create().GetBytes(password);
        return password;
    }

    private string CreateStringContent(int contentBytes)
    {
        return Convert.ToBase64String(this.CreateByteContent(contentBytes));
    }

    private string CreateStringPassword(int aesBits)
    {
        return Convert.ToBase64String(this.CreateBytePassword(aesBits));
    }
}
