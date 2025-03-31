namespace DocumentPacker.Tests.Parts.Main.EncryptPart.ViewModels;

using DocumentPacker.EventHandling;
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

    public async Task ExecuteSaveCommand(EncryptViewModelTestData testData)
    {
        using var createConfigurationViewModelTests = new CreateConfigurationViewModelTests();

        await createConfigurationViewModelTests.ExecuteSaveCommand(testData.ConfigurationViewModelTestData);

        this.encryptViewModel.LoadConfigurationViewModel.ConfigurationFile.Value =
            testData.ConfigurationViewModelTestData.PublicFile;
        this.encryptViewModel.LoadConfigurationViewModel.Password.Value =
            testData.ConfigurationViewModelTestData.Password;
        this.encryptViewModel.LoadConfigurationViewModel.LoadConfigurationCommand.Command.Execute(null);

        for (var i = 0; i < 50 && this.encryptViewModel.EncryptDataViewModel is null; i++)
        {
            await Task.Delay(
                100,
                TestContext.Current.CancellationToken);
        }

        Assert.NotNull(this.encryptViewModel.EncryptDataViewModel);

        foreach (var configurationItem in testData.Items)
        {
            var item = this.encryptViewModel.EncryptDataViewModel?.Items?.SingleOrDefault(
                item => item.Id.Value == configurationItem.name);
            Assert.NotNull(item);
            item.Value.Value = configurationItem.value;
        }

        this.encryptViewModel.OutputFile.Value = testData.OutputFileName;
        this.encryptViewModel.OutputFolder.Value = testData.OutputFolder;

        Assert.True(this.encryptViewModel.SaveCommand.Command.CanExecute(null));

        this.encryptViewModel.SaveCommand.Command.Execute(null);

        for (var i = 0;
             i < 50 && (this.encryptViewModel.SaveCommand.Command.IsActive || !File.Exists(testData.OutputFile));
             i++)
        {
            await Task.Delay(
                100,
                TestContext.Current.CancellationToken);
        }

        testData.Validate();
    }

    [Fact]
    public async Task SaveCommand()
    {
        var testData = new EncryptViewModelTestData();

        try
        {
            await this.ExecuteSaveCommand(testData);
        }
        finally
        {
            testData.Dispose();
        }
    }
}
