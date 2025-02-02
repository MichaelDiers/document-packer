namespace DocumentPacker.Tests.Extensions;

using DocumentPacker.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(111)]
    public void AsByte(int length)
    {
        var expected = new byte[length].FillRandom();

        var s = expected.AsString();

        var actual = s.AsByte();

        Assert.Equal(
            expected,
            actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("password")]
    public void AsSecureString(string expected)
    {
        var secureString = expected.AsSecureString();

        var actual = secureString.AsString();

        Assert.Equal(
            expected,
            actual);
    }
}
