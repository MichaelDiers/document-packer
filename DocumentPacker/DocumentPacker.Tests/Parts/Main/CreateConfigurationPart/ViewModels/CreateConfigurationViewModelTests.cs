namespace DocumentPacker.Tests.Parts.Main.CreateConfigurationPart.ViewModels;

using DocumentPacker.EventHandling;
using DocumentPacker.Parts.Main.CreateConfigurationPart;
using DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;
using DocumentPacker.Services;
using Libs.Wpf.Commands;
using Libs.Wpf.Commands.CancelWindow;
using Libs.Wpf.Controls.CustomMessageBox;
using Libs.Wpf.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Sreid.Libs.Crypto.Factory;

public class CreateConfigurationViewModelTests : IDisposable
{
    private readonly ICreateConfigurationViewModel createConfigurationViewModel;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CreateConfigurationViewModelTests" /> class.
    /// </summary>
    public CreateConfigurationViewModelTests()
    {
        var messageBoxServiceMock = new Mock<IMessageBoxService>();

        var cancelWindow = new Mock<ICancelWindow>();

        var cancelWindowService = new Mock<ICancelWindowService>();
        cancelWindowService.Setup(service => service.CreateCancelWindow(It.IsAny<object?>()))
            .Returns(cancelWindow.Object);

        var provider = CustomServiceProviderBuilder.Build(
            services => services.AddSingleton(cancelWindowService.Object),
            CreateConfigurationServiceCollectionExtensions.TryAddCreateConfigurationViewModel,
            CommandsServiceCollectionExtensions.TryAddCommandFactory,
            ServicesServiceCollectionExtensions.TryAddDocumentPackerConfigurationFileService,
            CommandsServiceCollectionExtensions.TryAddCommandSync,
            FactoryServiceCollectionExtensions.TryAddFactory,
            services =>
            {
                services.TryAddSingleton(messageBoxServiceMock.Object);
                return services;
            });

        var viewModel =
            provider.GetRequiredKeyedService<IApplicationViewModel>(ApplicationElementPart.CreateConfiguration) as
                ICreateConfigurationViewModel;
        Assert.NotNull(viewModel);
        this.createConfigurationViewModel = viewModel;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        this.createConfigurationViewModel.Dispose();
    }

    public async Task ExecuteSaveCommand(CreateConfigurationViewModelTestData testData)
    {
        this.createConfigurationViewModel.Description.Value = testData.Description;

        this.createConfigurationViewModel.GenerateRsaKeysCommand.Command.Execute(null);

        this.createConfigurationViewModel.ConfigurationItems.Clear();
        foreach (var configurationItem in testData.Items)
        {
            this.createConfigurationViewModel.AddConfigurationItemCommand.Command.Execute(null);
            var item = this.createConfigurationViewModel.ConfigurationItems.Last();
            item.Id.Value = configurationItem.name;
            item.ItemDescription.Value = configurationItem.description;
            Assert.NotNull(item.ConfigurationItemTypes.Value);
            item.ConfigurationItemTypes.SelectedValue =
                item.ConfigurationItemTypes.Value.First(type => type.Value == configurationItem.itemType);
            item.IsRequired.Value = configurationItem.isRequired;
        }

        this.createConfigurationViewModel.Password.Value = testData.Password;

        this.createConfigurationViewModel.OutputFolder.Value = testData.OutputFolder;

        this.createConfigurationViewModel.PrivateOutputFile.Value = testData.PrivateFileName;
        this.createConfigurationViewModel.PublicOutputFile.Value = testData.PublicFileName;

        for (var i = 0; i < 50 && this.createConfigurationViewModel.GenerateRsaKeysCommand.Command.IsActive; i++)
        {
            await Task.Delay(100);
        }

        Assert.False(this.createConfigurationViewModel.GenerateRsaKeysCommand.Command.IsActive);
        Assert.True(this.createConfigurationViewModel.Validate());

        this.createConfigurationViewModel.SaveCommand.Command.Execute(null);
        for (var i = 0; i < 50 && this.createConfigurationViewModel.SaveCommand.Command.IsActive; i++)
        {
            await Task.Delay(100);
        }

        Assert.False(this.createConfigurationViewModel.SaveCommand.Command.IsActive);

        var privateFile = Path.Combine(
            this.createConfigurationViewModel.OutputFolder.Value,
            $"{this.createConfigurationViewModel.PrivateOutputFile.Value}{this.createConfigurationViewModel.PrivateOutputFileExtension.Value}");
        var publicFile = Path.Combine(
            this.createConfigurationViewModel.OutputFolder.Value,
            $"{this.createConfigurationViewModel.PublicOutputFile.Value}{this.createConfigurationViewModel.PublicOutputFileExtension.Value}");

        Assert.True(File.Exists(privateFile));
        Assert.True(File.Exists(publicFile));

        testData.Validate();
    }

    [Fact]
    public async Task SaveConfiguration()
    {
        var testData = new CreateConfigurationViewModelTestData();

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
