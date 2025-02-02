namespace DocumentPacker.Extensions;

using System.Net;
using System.Security;

/// <summary>
///     Extensions for <see cref="SecureString" />.
/// </summary>
public static class SecureStringExtensions
{
    /// <summary>
    ///     Converts to from <see cref="SecureString" /> to an array of <see cref="byte" />.
    /// </summary>
    /// <param name="s">The input <see cref="SecureString" />.</param>
    /// <returns>The array of <see cref="byte" />.</returns>
    public static byte[] AsByte(this SecureString s)
    {
        return s.AsString().AsByte();
    }

    /// <summary>
    ///     Converts to from <see cref="SecureString" /> to <see cref="string" />.
    /// </summary>
    /// <param name="s">The input <see cref="SecureString" />.</param>
    /// <returns>The unsecure <see cref="string" />.</returns>
    public static string AsString(this SecureString s)
    {
        return new NetworkCredential(
            string.Empty,
            s).Password;
    }
}
