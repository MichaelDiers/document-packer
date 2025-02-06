namespace DocumentPacker.Services.PackerStream;

/// <summary>
///     The supported modes of the <see cref="PackerStream" />.
/// </summary>
public enum PackerStreamMode
{
    /// <summary>
    ///     Compress and encrypt the stream data.
    /// </summary>
    Pack,

    /// <summary>
    ///     Decrypt and decompress the stream data.
    /// </summary>
    Unpack
}
