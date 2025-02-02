namespace DocumentPacker.Tests.Extensions;

using DocumentPacker.Extensions;

/// <summary>
///     Tests for <see cref="ByteExtensions" />.
/// </summary>
public class ByteExtensionsTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(11)]
    [InlineData(111)]
    public void AsSecureString(int length)
    {
        var bytes = new byte[length];
        bytes.FillRandom();

        var secureString = bytes.AsSecureString();

        Assert.Equal(
            bytes,
            secureString.AsByte());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(11)]
    [InlineData(111)]
    public void AsString(int length)
    {
        var bytes = new byte[length];
        bytes.FillRandom();

        var s = bytes.AsString();

        Assert.Equal(
            bytes,
            Convert.FromBase64String(s));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(11)]
    [InlineData(111)]
    public void FillRandom(int length)
    {
        var bytes = new byte[length];
        bytes.FillRandom();

        Assert.Equal(
            length,
            bytes.Length);
        Assert.True(length < 50 || bytes.Count(b => b == 0) < length / 2);
    }
}
