namespace DocumentPacker.Services.PackerStream;

using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

/// <summary>
///     A custom stream that supports <see cref="BrotliStream" /> compression and <see cref="Aes" /> encryption.
/// </summary>
/// <seealso cref="System.IO.Stream" />
public class PackerStream(
    Stream stream,
    PackerStreamMode packerStreamMode,
    byte[] aesKey,
    PaddingMode paddingMode = PaddingMode.PKCS7
) : Stream
{
    /// <summary>
    ///     The brotli stream is used for compression and decompression.
    /// </summary>
    private BrotliStream? brotliStream;

    /// <summary>
    ///     The crypto stream is used for <see cref="Aes" /> encryption and decryption.
    /// </summary>
    private CryptoStream? cryptoStream;

    /// <summary>
    ///     The crypto transform for en- or decoding.
    /// </summary>
    private ICryptoTransform? cryptoTransform;

    /// <summary>
    ///     Indicates whether the <see cref="brotliStream" /> and <see cref="cryptoStream" /> is initialized.
    /// </summary>
    private bool isInitialized;

    /// <summary>When overridden in a derived class, gets a value indicating whether the current stream supports reading.</summary>
    /// <returns>
    ///     <see langword="true" /> if the stream supports reading; otherwise, <see langword="false" />.
    /// </returns>
    public override bool CanRead => packerStreamMode == PackerStreamMode.Unpack;

    /// <summary>When overridden in a derived class, gets a value indicating whether the current stream supports seeking.</summary>
    /// <returns>
    ///     <see langword="true" /> if the stream supports seeking; otherwise, <see langword="false" />.
    /// </returns>
    public override bool CanSeek => false;

    /// <summary>When overridden in a derived class, gets a value indicating whether the current stream supports writing.</summary>
    /// <returns>
    ///     <see langword="true" /> if the stream supports writing; otherwise, <see langword="false" />.
    /// </returns>
    public override bool CanWrite => packerStreamMode == PackerStreamMode.Pack;

    /// <summary>When overridden in a derived class, gets the length in bytes of the stream.</summary>
    /// <exception cref="T:System.NotSupportedException">
    ///     A class derived from <see langword="Stream" /> does not support
    ///     seeking and the length is unknown.
    /// </exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <returns>A long value representing the length of the stream in bytes.</returns>
    public override long Length => throw new NotSupportedException();

    /// <summary>When overridden in a derived class, gets or sets the position within the current stream.</summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support seeking.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <returns>The current position within the stream.</returns>
    public override long Position
    {
        get => throw new NotSupportedException();
        set => throw new NotSupportedException();
    }

    /// <summary>
    ///     When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be
    ///     written to the underlying device.
    /// </summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    public override void Flush()
    {
        this.brotliStream?.Flush();
        this.cryptoStream?.Flush();
    }

    /// <summary>
    ///     When overridden in a derived class, reads a sequence of bytes from the current stream and advances the
    ///     position within the stream by the number of bytes read.
    /// </summary>
    /// <param name="buffer">
    ///     An array of bytes. When this method returns, the buffer contains the specified byte array with the
    ///     values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced
    ///     by the bytes read from the current source.
    /// </param>
    /// <param name="offset">
    ///     The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read
    ///     from the current stream.
    /// </param>
    /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
    /// <exception cref="T:System.ArgumentException">
    ///     The sum of <paramref name="offset" /> and <paramref name="count" /> is
    ///     larger than the buffer length.
    /// </exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///     <paramref name="buffer" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///     <paramref name="offset" /> or <paramref name="count" /> is negative.
    /// </exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <returns>
    ///     The total number of bytes read into the buffer. This can be less than the number of bytes requested if that
    ///     many bytes are not currently available, or zero (0) if <paramref name="count" /> is 0 or the end of the stream has
    ///     been reached.
    /// </returns>
    public override int Read(byte[] buffer, int offset, int count)
    {
        if (!this.CanRead)
        {
            throw new NotSupportedException("Stream is not readable.");
        }

        if (!this.isInitialized)
        {
            this.InitializeUnpackStream();
        }

        return this.brotliStream!.Read(
            buffer,
            offset,
            count);
    }

    /// <summary>When overridden in a derived class, sets the position within the current stream.</summary>
    /// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
    /// <param name="origin">
    ///     A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to
    ///     obtain the new position.
    /// </param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///     The stream does not support seeking, such as if the stream is
    ///     constructed from a pipe or console output.
    /// </exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    /// <returns>The new position within the current stream.</returns>
    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    /// <summary>When overridden in a derived class, sets the length of the current stream.</summary>
    /// <param name="value">The desired length of the current stream in bytes.</param>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.NotSupportedException">
    ///     The stream does not support both writing and seeking, such as if the
    ///     stream is constructed from a pipe or console output.
    /// </exception>
    /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    ///     When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current
    ///     position within this stream by the number of bytes written.
    /// </summary>
    /// <param name="buffer">
    ///     An array of bytes. This method copies <paramref name="count" /> bytes from
    ///     <paramref name="buffer" /> to the current stream.
    /// </param>
    /// <param name="offset">
    ///     The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the
    ///     current stream.
    /// </param>
    /// <param name="count">The number of bytes to be written to the current stream.</param>
    /// <exception cref="T:System.ArgumentException">
    ///     The sum of <paramref name="offset" /> and <paramref name="count" /> is
    ///     greater than the buffer length.
    /// </exception>
    /// <exception cref="T:System.ArgumentNullException">
    ///     <paramref name="buffer" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///     <paramref name="offset" /> or <paramref name="count" /> is negative.
    /// </exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred, such as the specified file cannot be found.</exception>
    /// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
    /// <exception cref="T:System.ObjectDisposedException">
    ///     <see cref="M:System.IO.Stream.Write(System.Byte[],System.Int32,System.Int32)" /> was called after the stream was
    ///     closed.
    /// </exception>
    public override void Write(byte[] buffer, int offset, int count)
    {
        if (!this.CanWrite)
        {
            throw new NotSupportedException();
        }

        if (!this.isInitialized)
        {
            this.InitializePackStream();
        }

        this.brotliStream!.Write(
            buffer,
            offset,
            count);
    }

    /// <summary>
    ///     Releases the unmanaged resources used by the <see cref="T:System.IO.Stream" /> and optionally releases the
    ///     managed resources.
    /// </summary>
    /// <param name="disposing">
    ///     <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only
    ///     unmanaged resources.
    /// </param>
    protected override void Dispose(bool disposing)
    {
        this.brotliStream?.Dispose();
        this.cryptoStream?.Dispose();
        this.cryptoTransform?.Dispose();
        base.Dispose(disposing);
    }

    /// <summary>
    ///     Initialize <see cref="Aes" /> using the given <paramref name="key" /> and an optional <see cref="Aes.IV" />.
    /// </summary>
    /// <param name="key">The aes key.</param>
    /// <param name="iv">If given the <see cref="Aes.IV" /> is used; otherwise a new iv is generated.</param>
    /// <param name="paddingMode">The aes padding mode.</param>
    /// <returns>A new <see cref="Aes" /> instance.</returns>
    private static Aes InitializeAes(byte[] key, byte[]? iv = null, PaddingMode paddingMode = PaddingMode.PKCS7)
    {
        var aes = Aes.Create();
        aes.Padding = paddingMode;
        aes.Key = key;

        if (iv is null)
        {
            aes.GenerateIV();
        }
        else
        {
            aes.IV = iv;
        }

        return aes;
    }

    /// <summary>
    ///     Initializes the pack stream to first compress and then encrypt.
    /// </summary>
    /// <exception cref="InvalidOperationException">Stream is not writeable.</exception>
    private void InitializePackStream()
    {
        if (this.isInitialized)
        {
            return;
        }

        if (!stream.CanWrite)
        {
            throw new InvalidOperationException("Stream is not writeable.");
        }

        // initialize aes and the encryptor
        using var aes = PackerStream.InitializeAes(
            aesKey,
            paddingMode: paddingMode);
        this.cryptoTransform = aes.CreateEncryptor();

        // write the iv length to the stream
        stream.WriteByte((byte) aes.IV.Length);

        // write the iv to the stream
        stream.Write(aes.IV);

        // set the aes encrypt stream
        this.cryptoStream = new CryptoStream(
            stream,
            this.cryptoTransform,
            CryptoStreamMode.Write);

        // compress using brotli
        this.brotliStream = new BrotliStream(
            this.cryptoStream,
            CompressionLevel.Optimal);

        this.isInitialized = true;
    }

    /// <summary>
    ///     Initializes the pack stream to first decrypt and then decompress.
    /// </summary>
    /// <exception cref="InvalidOperationException">Stream is not readable.</exception>
    private void InitializeUnpackStream()
    {
        if (this.isInitialized)
        {
            return;
        }

        if (!stream.CanRead)
        {
            throw new InvalidOperationException("Stream is not readable.");
        }

        // initialize aes and the decryptor
        var ivLength = stream.ReadByte();
        if (ivLength < 1)
        {
            throw new InvalidOperationException("Missing aes header: Length of iv not specified.");
        }

        var iv = new byte[ivLength];
        var actualIvLength = stream.Read(iv);
        if (actualIvLength != iv.Length)
        {
            throw new InvalidOperationException("Incomplete aes header: iv missing or incomplete.");
        }

        using var aes = PackerStream.InitializeAes(
            aesKey,
            iv,
            paddingMode);

        this.cryptoTransform = aes.CreateDecryptor();

        // set the aes encrypt stream
        this.cryptoStream = new CryptoStream(
            stream,
            this.cryptoTransform,
            CryptoStreamMode.Read);

        // compress using brotli
        this.brotliStream = new BrotliStream(
            this.cryptoStream,
            CompressionMode.Decompress);

        this.isInitialized = true;
    }
}
