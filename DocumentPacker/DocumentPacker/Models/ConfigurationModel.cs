namespace DocumentPacker.Models;

/// <summary>
///     Describes the data of a document packer configuration.
/// </summary>
public class ConfigurationModel
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ConfigurationModel" /> class.
    /// </summary>
    public ConfigurationModel(
        IEnumerable<ConfigurationItemModel> configurationItems,
        string description,
        string rsaPrivateKey,
        string rsaPublicKey
    )
    {
        this.ConfigurationItems = configurationItems.ToArray();
        this.Description = description;
        this.RsaPrivateKey = rsaPrivateKey;
        this.RsaPublicKey = rsaPublicKey;
    }

    /// <summary>
    ///     Gets the configuration items.
    /// </summary>
    public IEnumerable<ConfigurationItemModel> ConfigurationItems { get; }

    /// <summary>
    ///     Gets the description of the configuration.
    /// </summary>
    public string Description { get; }

    /// <summary>
    ///     Gets the private rsa key in pem format.
    /// </summary>
    public string? RsaPrivateKey { get; set; }

    /// <summary>
    ///     Gets the public rsa key in pem format.
    /// </summary>
    public string RsaPublicKey { get; }
}
