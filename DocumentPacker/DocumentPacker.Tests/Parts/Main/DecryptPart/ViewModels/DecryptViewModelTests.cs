namespace DocumentPacker.Tests.Parts.Main.DecryptPart.ViewModels;

using DocumentPacker.EventHandling;
using DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;
using DocumentPacker.Parts.Main.DecryptPart;
using DocumentPacker.Services;
using DocumentPacker.Tests.Parts.Main.EncryptPart.ViewModels;
using Libs.Wpf.Commands;
using Libs.Wpf.Commands.CancelWindow;
using Libs.Wpf.Controls.CustomMessageBox;
using Libs.Wpf.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class DecryptViewModelTests : IDisposable
{
    private readonly IDecryptViewModel decryptViewModel;

    public DecryptViewModelTests()
    {
        var messageBoxServiceMock = new Mock<IMessageBoxService>();

        var cancelWindow = new Mock<ICancelWindow>();

        var cancelWindowService = new Mock<ICancelWindowService>();
        cancelWindowService.Setup(service => service.CreateCancelWindow(It.IsAny<object?>()))
            .Returns(cancelWindow.Object);

        var provider = CustomServiceProviderBuilder.Build(
            services => services.AddSingleton(messageBoxServiceMock.Object),
            services => services.AddSingleton(cancelWindowService.Object),
            DecryptPartServiceCollectionExtensions.TryAddDecryptViewModel,
            CommandsServiceCollectionExtensions.TryAddCommandFactory,
            ServicesServiceCollectionExtensions.TryAddDocumentPackerConfigurationFileService,
            ServicesServiceCollectionExtensions.TryAddDecryptService,
            CommandsServiceCollectionExtensions.TryAddCommandSync,
            ServicesServiceCollectionExtensions.TryAddDocumentUnpackerService);

        var viewModel =
            provider.GetRequiredKeyedService<IApplicationViewModel>(ApplicationElementPart.DecryptFeature) as
                IDecryptViewModel;
        Assert.NotNull(viewModel);
        this.decryptViewModel = viewModel;
    }

    [Fact]
    public async Task DecryptCommand()
    {
        var testData = new DecryptViewModelTestData();

        try
        {
            await this.ExecuteDecryptCommand(testData);
        }
        finally
        {
            testData.Dispose();
        }
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        this.decryptViewModel.Dispose();
    }

    private async Task ExecuteDecryptCommand(DecryptViewModelTestData testData)
    {
        await new EncryptViewModelTests().ExecuteSaveCommand(testData.EncryptViewModelTestData);

        testData.EncryptViewModelTestData.Validate();

        this.decryptViewModel.LoadConfigurationViewModel.ConfigurationFile.Value =
            testData.EncryptViewModelTestData.ConfigurationViewModelTestData.PrivateFile;
        this.decryptViewModel.LoadConfigurationViewModel.Password.Value =
            testData.EncryptViewModelTestData.ConfigurationViewModelTestData.Password;

        Assert.True(this.decryptViewModel.LoadConfigurationViewModel.LoadConfigurationCommand.Command.CanExecute(null));
        this.decryptViewModel.LoadConfigurationViewModel.LoadConfigurationCommand.Command.Execute(null);

        for (var i = 0;
             i < 50 && this.decryptViewModel.LoadConfigurationViewModel.LoadConfigurationCommand.Command.IsActive;
             i++)
        {
            await Task.Delay(
                100,
                TestContext.Current.CancellationToken);
        }

        Assert.False(string.IsNullOrWhiteSpace(this.decryptViewModel.PrivateRsaKey.Value));

        this.decryptViewModel.EncryptedFile.Value = testData.EncryptViewModelTestData.OutputFile;
        this.decryptViewModel.OutputFolder.Value = testData.OutputFolder;

        Assert.True(this.decryptViewModel.DecryptCommand.Command.CanExecute(null));
        this.decryptViewModel.DecryptCommand.Command.Execute(null);

        for (var i = 0; i < 50 && this.decryptViewModel.DecryptCommand.Command.IsActive; i++)
        {
            await Task.Delay(
                100,
                TestContext.Current.CancellationToken);
        }

        Assert.False(this.decryptViewModel.DecryptCommand.Command.IsActive);

        foreach (var data in testData.EncryptViewModelTestData.Items)
        {
            var file = Path.Join(
                testData.OutputFolder,
                data.itemType == ConfigurationItemType.Text
                    ? $"{data.name}.txt"
                    : $"{data.name}{Path.GetExtension(data.value)}");
            Assert.True(File.Exists(file));

            var content = await File.ReadAllTextAsync(
                file,
                TestContext.Current.CancellationToken);
            switch (data.itemType)
            {
                case ConfigurationItemType.File:
                    Assert.Equal(
                        data.fileContent,
                        content);
                    break;
                case ConfigurationItemType.Text:
                    Assert.Equal(
                        $"{data.description}{Environment.NewLine}{Environment.NewLine}{data.value}",
                        content);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        testData.Validate();
    }
}
