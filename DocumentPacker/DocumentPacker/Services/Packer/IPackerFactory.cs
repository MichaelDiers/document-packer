namespace DocumentPacker.Services.Packer;

using System.IO;
using System.Security;
using DocumentPacker.Services.Crypto;
using DocumentPacker.Services.Zip;

/// <summary>
///     A factory for creating a new packer.
/// </summary>
public interface IPackerFactory
{
    /// <summary>
    ///     Creates the packer and writes all data to the given file.
    /// </summary>
    /// <param name="cryptoFactory">A factory to create crypto algorithms.</param>
    /// <param name="fileInfo">The file information of the resulting package file.</param>
    /// <param name="zipFileCreator">A factory to create zip files.</param>
    /// <param name="rsaPublicKey">The rsa public key.</param>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>An <see cref="IPacker" /> that is used to add data.</returns>
    Task<IPacker> CreatePackerAsync(
        IZipFileCreator zipFileCreator,
        ICryptoFactory cryptoFactory,
        FileInfo fileInfo,
        SecureString rsaPublicKey,
        CancellationToken cancellationToken
    );
}
