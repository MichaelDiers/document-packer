namespace DocumentPacker.Extensions;

using System.Security;

/// <summary>
///     Extensions for <see cref="string" />.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     Converts a <see cref="string" /> to an array of <see cref="byte" />.
    /// </summary>
    /// <param name="s">The <see cref="string" /> that is converted.</param>
    /// <returns>An array of <see cref="byte" />.</returns>
    public static byte[] AsByte(this string s)
    {
        return Convert.FromBase64String(s);
    }

    /// <summary>
    ///     Converts a <see cref="string" /> to a <see cref="SecureString" />.
    /// </summary>
    /// <param name="s">The <see cref="string" /> that is converted.</param>
    /// <returns>A <see cref="SecureString" />.</returns>
    public static SecureString AsSecureString(this string s)
    {
        var secureString = new SecureString();
        foreach (var character in s)
        {
            secureString.AppendChar(character);
        }

        return secureString;
    }
}
