namespace DocumentPacker.Tests.Parts.Main.CreateConfigurationPart.ViewModels;

using System.Security;
using System.Windows.Threading;
using DocumentPacker.Commands;
using DocumentPacker.EventHandling;
using DocumentPacker.Parts.Main.CreateConfigurationPart;
using DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;
using DocumentPacker.Services;
using Libs.Wpf.Commands;
using Libs.Wpf.Controls.CustomMessageBox;
using Libs.Wpf.DependencyInjection;
using Libs.Wpf.Threads;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

public class CreateConfigurationViewModelTests : IDisposable
{
    private readonly ICreateConfigurationViewModel createConfigurationViewModel;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CreateConfigurationViewModelTests" /> class.
    /// </summary>
    public CreateConfigurationViewModelTests()
    {
        var messageBoxServiceMock = new Mock<IMessageBoxService>();

        var dispatcherWrapperMock = new Mock<IDispatcherWrapper>();
        dispatcherWrapperMock.Setup(dispatcherWrapper => dispatcherWrapper.Dispatcher)
            .Returns(Dispatcher.CurrentDispatcher);

        var provider = CustomServiceProviderBuilder.Build(
            CreateConfigurationServiceCollectionExtensions.TryAddCreateConfigurationViewModel,
            ServiceCollectionExtensions.TryAddCommandFactory,
            ServicesServiceCollectionExtensions.TryAddRsaService,
            ServicesServiceCollectionExtensions.TryAddDocumentPackerConfigurationFileService,
            CommandsServiceCollectionExtensions.TryAddCommandSync,
            services =>
            {
                services.TryAddSingleton(messageBoxServiceMock.Object);
                return services;
            },
            services =>
            {
                services.TryAddSingleton(dispatcherWrapperMock.Object);
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

    [Fact]
    public async Task SaveConfiguration()
    {
        var password = new SecureString();
        password.AppendChar('1');

        var outputFolder = Guid.NewGuid().ToString();
        var privateOutputFile = Guid.NewGuid().ToString();
        var publicOutputFile = Guid.NewGuid().ToString();

        try
        {
            var _ = await this.ExecuteSaveCommand(
                "description",
                [("name1", "description1", ConfigurationItemType.Text, false)],
                password,
                outputFolder,
                privateOutputFile,
                publicOutputFile);
        }
        finally
        {
            var privateFileInfo = new FileInfo(
                Path.Combine(
                    outputFolder,
                    $"{privateOutputFile}.private.dpc"));
            if (privateFileInfo.Exists)
            {
                privateFileInfo.Delete();
            }

            var publicFileInfo = new FileInfo(
                Path.Combine(
                    outputFolder,
                    $"{publicOutputFile}.public.dpc"));
            if (publicFileInfo.Exists)
            {
                publicFileInfo.Delete();
            }

            if (Directory.Exists(outputFolder))
            {
                Directory.Delete(outputFolder);
            }
        }
    }

    private async Task<(string privateConfigurationFile, string publicConfigurationFile)> ExecuteSaveCommand(
        string description,
        IEnumerable<(string name, string description, ConfigurationItemType itemType, bool isRequired)>
            configurationItems,
        SecureString password,
        string outputFolder,
        string privateOutputFile,
        string publicOutputFile
    )
    {
        this.createConfigurationViewModel.Description.Value = description;

        this.createConfigurationViewModel.GenerateRsaKeysCommand.Command.Execute(null);

        this.createConfigurationViewModel.ConfigurationItems.Clear();
        foreach (var configurationItem in configurationItems)
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

        this.createConfigurationViewModel.Password.Value = password;

        Directory.CreateDirectory(outputFolder);
        this.createConfigurationViewModel.OutputFolder.Value = outputFolder;

        this.createConfigurationViewModel.PrivateOutputFile.Value = privateOutputFile;
        this.createConfigurationViewModel.PublicOutputFile.Value = publicOutputFile;

        for (var i = 0; i < 20 && this.createConfigurationViewModel.GenerateRsaKeysCommand.Command.IsActive; i++)
        {
            DispatcherHelperCore.DoEvents();
            await Task.Delay(250);
        }

        Assert.True(this.createConfigurationViewModel.Validate());

        this.createConfigurationViewModel.SaveCommand.Command.Execute(null);
        for (var i = 0; i < 20 && this.createConfigurationViewModel.SaveCommand.Command.IsActive; i++)
        {
            DispatcherHelperCore.DoEvents();
            await Task.Delay(250);
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

        return (privateFile, publicFile);
    }
}
