namespace DocumentPacker.Parts2.Header.NavigationBarPart;

using System.Collections.ObjectModel;
using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;

/// <summary>
///     The view model of the <see cref="NavigationBarView" />. Displays the navigation bar links.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationViewModel" />
internal class NavigationBarViewModel : ApplicationViewModel
{
    /// <summary>
    ///     The menu items of the navigation bar.
    /// </summary>
    private ObservableCollection<object> menuItems = [];

    /// <summary>
    ///     Gets or sets the menu items of the navigation bar.
    /// </summary>
    [ApplicationPart(ApplicationElementPart.ChangeLanguageLink)]
    [ApplicationPart(ApplicationElementPart.BackLink)]
    public ObservableCollection<object> MenuItems
    {
        get => this.menuItems;
        set =>
            this.SetField(
                ref this.menuItems,
                value);
    }
}
