namespace DocumentPacker.Extensions;

using System.Net;
using System.Security;

/// <summary>
///     Extensions for <see cref="SecureString" />.
/// </summary>
public static class SecureStringExtensions
{
    /// <summary>
    ///     Converts to from <see cref="SecureString" /> to <see cref="string" />.
    /// </summary>
    /// <param name="s">The input <see cref="SecureString" />.</param>
    /// <returns>The unsecure <see cref="string" />.</returns>
    public static string ToUnsecureString(this SecureString s)
    {
        return new NetworkCredential(
            string.Empty,
            s).Password;
    }
}
