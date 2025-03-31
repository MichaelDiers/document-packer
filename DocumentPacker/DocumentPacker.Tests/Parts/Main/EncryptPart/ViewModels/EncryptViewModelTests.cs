namespace DocumentPacker.Tests.Parts.Main.EncryptPart.ViewModels;

using System.Security;
using DocumentPacker.EventHandling;
using DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;
using DocumentPacker.Parts.Main.EncryptPart;
using DocumentPacker.Parts.Main.EncryptPart.ViewModels;
using DocumentPacker.Services;
using DocumentPacker.Tests.Parts.Main.CreateConfigurationPart.ViewModels;
using Libs.Wpf.Commands;
using Libs.Wpf.Commands.CancelWindow;
using Libs.Wpf.Controls.CustomMessageBox;
using Libs.Wpf.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class EncryptViewModelTests : IDisposable
{
    private readonly IEncryptViewModel encryptViewModel;

    public EncryptViewModelTests()
    {
        var messageBoxServiceMock = new Mock<IMessageBoxService>();

        var cancelWindow = new Mock<ICancelWindow>();

        var cancelWindowService = new Mock<ICancelWindowService>();
        cancelWindowService.Setup(service => service.CreateCancelWindow(It.IsAny<object?>()))
            .Returns(cancelWindow.Object);

        var provider = CustomServiceProviderBuilder.Build(
            services => services.AddSingleton(messageBoxServiceMock.Object),
            services => services.AddSingleton(cancelWindowService.Object),
            EncryptPartServiceCollectionExtensions.TryAddEncryptViewModel,
            CommandsServiceCollectionExtensions.TryAddCommandFactory,
            ServicesServiceCollectionExtensions.TryAddDocumentPackerConfigurationFileService,
            ServicesServiceCollectionExtensions.TryAddEncryptService,
            CommandsServiceCollectionExtensions.TryAddCommandSync,
            ServicesServiceCollectionExtensions.TryAddDocumentPackerService);

        var viewModel =
            provider.GetRequiredKeyedService<IApplicationViewModel>(ApplicationElementPart.EncryptFeature) as
                IEncryptViewModel;
        Assert.NotNull(viewModel);
        this.encryptViewModel = viewModel;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        this.encryptViewModel.Dispose();
    }

    public async Task<(string privateConfigurationFile, string publicConfigurationFile, string encryptedFile)>
        ExecuteSaveCommand(
            string description,
            ICollection<(string name, string description, ConfigurationItemType itemType, bool isRequired, string value
                )> configurationItems,
            SecureString password,
            string configurationFileOutputFolder,
            string configurationFilePrivateOutputFile,
            string configurationFilePublicOutputFile,
            string encryptedFileOutputFolder,
            string encryptedFileOutputFileName
        )
    {
        using var createConfigurationViewModelTests = new CreateConfigurationViewModelTests();

        var configurationFiles = await createConfigurationViewModelTests.ExecuteSaveCommand(
            description,
            configurationItems.Select(item => (item.name, item.description, item.itemType, item.isRequired)).ToArray(),
            password,
            configurationFileOutputFolder,
            configurationFilePrivateOutputFile,
            configurationFilePublicOutputFile);

        this.encryptViewModel.LoadConfigurationViewModel.ConfigurationFile.Value =
            configurationFiles.publicConfigurationFile;
        this.encryptViewModel.LoadConfigurationViewModel.Password.Value = password;
        this.encryptViewModel.LoadConfigurationViewModel.LoadConfigurationCommand.Command.Execute(null);

        for (var i = 0; i < 50 && this.encryptViewModel.EncryptDataViewModel is null; i++)
        {
            await Task.Delay(
                100,
                TestContext.Current.CancellationToken);
        }

        Assert.NotNull(this.encryptViewModel.EncryptDataViewModel);

        foreach (var configurationItem in configurationItems)
        {
            var item = this.encryptViewModel.EncryptDataViewModel?.Items?.SingleOrDefault(
                item => item.Id.Value == configurationItem.name);
            Assert.NotNull(item);
        }

        this.encryptViewModel.OutputFile.Value = encryptedFileOutputFileName;
        this.encryptViewModel.OutputFolder.Value = encryptedFileOutputFolder;

        Assert.True(this.encryptViewModel.SaveCommand.Command.CanExecute(null));

        this.encryptViewModel.SaveCommand.Command.Execute(null);

        var outputFile = Path.Join(
            encryptedFileOutputFolder,
            $"{encryptedFileOutputFileName}{this.encryptViewModel.OutputFileExtension.Value}");

        for (var i = 0; i < 50 && (this.encryptViewModel.SaveCommand.Command.IsActive || !File.Exists(outputFile)); i++)
        {
            await Task.Delay(
                100,
                TestContext.Current.CancellationToken);
        }

        return (configurationFiles.privateConfigurationFile, configurationFiles.publicConfigurationFile, outputFile);
    }

    [Fact]
    public async Task SaveCommand()
    {
        var password = new SecureString();
        password.AppendChar('1');

        var outputFolder = Guid.NewGuid().ToString();
        var privateOutputFile = Guid.NewGuid().ToString();
        var publicOutputFile = Guid.NewGuid().ToString();

        var encryptedFileOutputFolder = Guid.NewGuid().ToString();
        var encryptedFileOutputFileName = Guid.NewGuid().ToString();

        var testFiles = Enumerable.Range(
                0,
                2)
            .Select(
                i =>
                {
                    var file = Guid.NewGuid().ToString();
                    File.WriteAllText(
                        file,
                        $"Lorem ipsum: {i}");
                    return file;
                })
            .ToArray();
        (string privateConfigurationFile, string publicConfigurationFile, string encryptedFile)? outputFiles = null;
        try
        {
            outputFiles = await this.ExecuteSaveCommand(
                "description",
                [
                    ("name1", "description1", ConfigurationItemType.Text, false, "text1"),
                    ("name2", "description2", ConfigurationItemType.Text, true, "text2"),
                    ("name3", "description3", ConfigurationItemType.File, false, testFiles[0]),
                    ("name4", "description4", ConfigurationItemType.File, true, testFiles[1])
                ],
                password,
                outputFolder,
                privateOutputFile,
                publicOutputFile,
                encryptedFileOutputFolder,
                encryptedFileOutputFileName);
        }
        finally
        {
            if (outputFiles is not null)
            {
                EncryptViewModelTests.DeleteFile(outputFiles.Value.privateConfigurationFile);
                EncryptViewModelTests.DeleteFile(outputFiles.Value.publicConfigurationFile);
                EncryptViewModelTests.DeleteFile(outputFiles.Value.encryptedFile);
            }

            foreach (var testFile in testFiles)
            {
                EncryptViewModelTests.DeleteFile(testFile);
            }

            EncryptViewModelTests.DeleteDirectory(outputFolder);

            EncryptViewModelTests.DeleteDirectory(encryptedFileOutputFolder);
        }
    }

    private static void DeleteDirectory(string directory)
    {
        try
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory);
            }
        }
        catch
        {
            // ignore
        }
    }

    private static void DeleteFile(string file)
    {
        try
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
        catch
        {
            // ignore
        }
    }
}
