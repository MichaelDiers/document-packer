namespace DocumentPacker.Services.Crypto;

/// <summary>
///     Specifies the type of crypto algorithm.
/// </summary>
public enum AlgorithmIdentifier : byte
{
    /// <summary>
    ///     Undefined value.
    /// </summary>
    None = 0,

    /// <summary>
    ///     Use aes.
    /// </summary>
    Aes,

    /// <summary>
    ///     Use rsa.
    /// </summary>
    Rsa
}
