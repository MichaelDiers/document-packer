namespace DocumentPacker.Extensions;

using System.Security;
using System.Security.Cryptography;

/// <summary>
///     Extensions for <see cref="byte" />.
/// </summary>
public static class ByteExtensions
{
    /// <summary>
    ///     The random number generator used by <see cref="FillRandom" />.
    /// </summary>
    private static readonly RandomNumberGenerator RandomNumberGenerator = RandomNumberGenerator.Create();

    /// <summary>
    ///     Converts an array of <see cref="byte" /> to <see cref="SecureString" />.
    /// </summary>
    /// <param name="bytes">The input bytes.</param>
    /// <returns>The output <see cref="SecureString" />.</returns>
    public static SecureString AsSecureString(this byte[] bytes)
    {
        return bytes.AsString().AsSecureString();
    }

    /// <summary>
    ///     Converts the given <paramref name="bytes" /> to <see cref="string" />.
    /// </summary>
    /// <param name="bytes">The input bytes.</param>
    /// <returns>The output <see cref="string" />.</returns>
    public static string AsString(this byte[] bytes)
    {
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    ///     Fills the <paramref name="bytes" /> with random values.
    /// </summary>
    /// <param name="bytes">The values are inserted in place to this array.</param>
    /// <returns>The given <paramref name="bytes" />.</returns>
    public static byte[] FillRandom(this byte[] bytes)
    {
        ByteExtensions.RandomNumberGenerator.GetBytes(bytes);
        return bytes;
    }
}
