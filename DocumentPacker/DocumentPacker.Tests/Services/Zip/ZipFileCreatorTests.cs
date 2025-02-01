namespace DocumentPacker.Tests.Services.Zip;

using DocumentPacker.Services;
using DocumentPacker.Services.Zip;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class ZipFileCreatorTests : IDisposable
{
    private readonly IZipFileCreator zipFileCreator;
    private readonly string zipFilePath = Guid.NewGuid().ToString();

    public ZipFileCreatorTests()
    {
        var builder = Host.CreateApplicationBuilder();
        builder.Services.TryAddZipServices();
        var host = builder.Build();
        this.zipFileCreator = host.Services.GetRequiredService<IZipFileCreator>();
    }

    [Fact]
    public void Create()
    {
        using var zipFile = this.zipFileCreator.Create(this.zipFilePath);

        Assert.True(File.Exists(this.zipFilePath));
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        if (File.Exists(this.zipFilePath))
        {
            File.Delete(this.zipFilePath);
        }
    }
}
