namespace DocumentPacker.ViewModels.SubViewModels;

using System.Windows.Input;
using DocumentPacker.Contracts;
using DocumentPacker.Contracts.ViewModels.SubViewModels;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///     View model for the startup view.
/// </summary>
/// <param name="viewHandler">Handler for requesting specific sub views.</param>
/// <seealso cref="DocumentPacker.ViewModels.SubViewModels.SubViewModel" />
/// <seealso cref="DocumentPacker.Contracts.ViewModels.SubViewModels.IStartUpViewModel" />
internal class StartUpViewModel(IViewHandler viewHandler) : SubViewModel(SubViewId.StartUp), IStartUpViewModel
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="StartUpViewModel" /> class.
    /// </summary>
    public StartUpViewModel()
        : this(App.ServiceProvider.GetRequiredService<IViewHandler>())
    {
    }

    /// <summary>
    ///     Gets the select next view command.
    /// </summary>
    public ICommand SelectNextViewCommand =>
        new TaskCommand(
            _ => true,
            (nextView, _) =>
            {
                if (!Enum.TryParse<SubViewId>(
                        nextView?.ToString(),
                        out var parsedNextView) ||
                    parsedNextView == SubViewId.None)
                {
                    throw new ArgumentException(
                        $"Cannot parse to {nameof(SubViewModel.SubViewId)}.",
                        nameof(nextView));
                }

                viewHandler.RequestView(parsedNextView);
                return Task.CompletedTask;
            });
}
