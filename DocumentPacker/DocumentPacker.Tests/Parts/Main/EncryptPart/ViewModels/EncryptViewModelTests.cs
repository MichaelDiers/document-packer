namespace DocumentPacker.Tests.Parts.Main.EncryptPart.ViewModels;

using System.Security;
using DocumentPacker.EventHandling;
using DocumentPacker.Parts.Main.CreateConfigurationPart.ViewModels;
using DocumentPacker.Parts.Main.EncryptPart;
using DocumentPacker.Parts.Main.EncryptPart.ViewModels;
using DocumentPacker.Services;
using DocumentPacker.Tests.Parts.Main.CreateConfigurationPart.ViewModels;
using Libs.Wpf.Commands;
using Libs.Wpf.Controls.CustomMessageBox;
using Libs.Wpf.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

public class EncryptViewModelTests
{
    public async Task<string> ExecuteSaveCommand()
    {
        var messageBoxServiceMock = new Mock<IMessageBoxService>();

        var provider = CustomServiceProviderBuilder.Build(
            EncryptPartServiceCollectionExtensions.TryAddEncryptViewModel,
            CommandsServiceCollectionExtensions.TryAddCommandFactory,
            ServicesServiceCollectionExtensions.TryAddDocumentPackerConfigurationFileService,
            ServicesServiceCollectionExtensions.TryAddEncryptService,
            CommandsServiceCollectionExtensions.TryAddCommandSync,
            services =>
            {
                services.TryAddSingleton(messageBoxServiceMock.Object);
                return services;
            });

        using var viewModel =
            provider.GetRequiredKeyedService<IApplicationViewModel>(ApplicationElementPart.EncryptFeature) as
                IEncryptViewModel;
        Assert.NotNull(viewModel);
        var encryptViewModel = viewModel;

        return "";
    }

    [Fact]
    public async Task SaveCommand()
    {
        var password = new SecureString();
        password.AppendChar('1');

        var outputFolder = Guid.NewGuid().ToString();
        var privateOutputFile = Guid.NewGuid().ToString();
        var publicOutputFile = Guid.NewGuid().ToString();

        try
        {
            using var createConfigurationViewModelTests = new CreateConfigurationViewModelTests();

            var _ = await createConfigurationViewModelTests.ExecuteSaveCommand(
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
}
