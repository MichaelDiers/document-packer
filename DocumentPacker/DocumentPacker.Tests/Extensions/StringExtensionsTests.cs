namespace DocumentPacker.Tests.Extensions;

using DocumentPacker.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("password")]
    public void ToSecureString(string expected)
    {
        var secureString = expected.ToSecureString();

        var actual = secureString.ToUnsecureString();

        Assert.Equal(
            expected,
            actual);
    }
}
