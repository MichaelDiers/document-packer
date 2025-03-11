namespace DocumentPacker.Services.DocumentUnpackerService;

public interface IExecute
{
    /// <summary>
    ///     Start to decompress, decrypt and unpack the files and text of the document packer file.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="Task" /> whose result indicates success.</returns>
    Task ExecuteAsync(CancellationToken cancellationToken);
}
