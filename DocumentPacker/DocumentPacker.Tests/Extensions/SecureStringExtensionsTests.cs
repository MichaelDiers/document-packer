namespace DocumentPacker.Tests.Extensions;

using System.Security;
using DocumentPacker.Extensions;

public class SecureStringExtensionsTests
{
    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("password")]
    public void ToUnsecureString(string expected)
    {
        var secureString = new SecureString();
        foreach (var character in expected)
        {
            secureString.AppendChar(character);
        }

        var actual = secureString.ToUnsecureString();

        Assert.Equal(
            expected,
            actual);
    }
}
