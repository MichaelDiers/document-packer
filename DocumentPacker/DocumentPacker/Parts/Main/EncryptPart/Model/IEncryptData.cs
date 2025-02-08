namespace DocumentPacker.Parts.Main.EncryptPart.Model;

using System.IO;

/// <summary>
///     Describes the data that gets packed and encrypted.
/// </summary>
public interface IEncryptData
{
    /// <summary>
    ///     Gets the document packer file.
    /// </summary>
    FileInfo DocumentPackerFile { get; }

    /// <summary>
    ///     Gets the data elements that get packed and encrypted.
    /// </summary>
    IEnumerable<IEncryptDataElement> EncryptDataElements { get; }

    /// <summary>
    ///     Gets the RSA public key pem.
    /// </summary>
    string RsaPublicKeyPem { get; }
}
