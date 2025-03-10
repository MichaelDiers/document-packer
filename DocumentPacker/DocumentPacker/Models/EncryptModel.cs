namespace DocumentPacker.Models;

public class EncryptModel(string outputFolder, string outputFile, IEnumerable<EncryptItemModel> items)
{
    public IEnumerable<EncryptItemModel> Items { get; } = items;
    public string OutputFile { get; } = outputFile;
    public string OutputFolder { get; } = outputFolder;
}
