namespace DocumentPacker.Parts2.Main.EncryptPart.Model;

using System.IO;

/// <summary>
///     Describes the data that gets packed and encrypted.
/// </summary>
internal class EncryptData(
    FileInfo documentPackerFile,
    IEnumerable<IEncryptDataElement> encryptDataElements,
    string rsaPublicKeyPem
) : IEncryptData
{
    /// <summary>
    ///     Gets the document packer file.
    /// </summary>
    public FileInfo DocumentPackerFile { get; } = documentPackerFile;

    /// <summary>
    ///     Gets the data elements that get packed and encrypted.
    /// </summary>
    public IEnumerable<IEncryptDataElement> EncryptDataElements { get; } = encryptDataElements;

    /// <summary>
    ///     Gets the RSA public key pem.
    /// </summary>
    public string RsaPublicKeyPem { get; } = rsaPublicKeyPem;
}
