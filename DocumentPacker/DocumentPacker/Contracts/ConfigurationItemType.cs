namespace DocumentPacker.Contracts;

/// <summary>
///     The available types of configuration items.
/// </summary>
public enum ConfigurationItemType
{
    /// <summary>
    ///     Undefined type.
    /// </summary>
    None = 0,

    /// <summary>
    ///     The file type.
    /// </summary>
    File,

    /// <summary>
    ///     The number type.
    /// </summary>
    Number,

    /// <summary>
    ///     The rsa private key type.
    /// </summary>
    PrivateKey,

    /// <summary>
    ///     The rsa public key type.
    /// </summary>
    PublicKey,

    /// <summary>
    ///     The text type.
    /// </summary>
    Text
}
