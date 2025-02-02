namespace DocumentPacker.Tests.Extensions;

using System.Security;
using DocumentPacker.Extensions;

/// <summary>
///     Tests for <see cref="SecureStringExtensions" />.
/// </summary>
public class SecureStringExtensionsTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    [InlineData(111)]
    public void AsByte(int length)
    {
        var expected = new byte[length].FillRandom();

        var secureString = expected.AsSecureString();

        var actual = secureString.AsByte();

        Assert.Equal(
            expected,
            actual);
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("password")]
    public void AsString(string expected)
    {
        var secureString = new SecureString();
        foreach (var character in expected)
        {
            secureString.AppendChar(character);
        }

        var actual = secureString.AsString();

        Assert.Equal(
            expected,
            actual);
    }
}
