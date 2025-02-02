namespace DocumentPacker.Extensions;

using System.Security;

/// <summary>
///     Extensions for <see cref="string" />.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     Converts a <see cref="string" /> to a <see cref="SecureString" />.
    /// </summary>
    /// <param name="s">The <see cref="string" /> that is converted.</param>
    /// <returns>A <see cref="SecureString" />.</returns>
    public static SecureString ToSecureString(this string s)
    {
        var secureString = new SecureString();
        foreach (var character in s)
        {
            secureString.AppendChar(character);
        }

        return secureString;
    }
}
