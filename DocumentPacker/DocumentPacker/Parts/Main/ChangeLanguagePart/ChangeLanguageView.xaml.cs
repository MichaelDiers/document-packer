namespace DocumentPacker.Parts.Main.ChangeLanguagePart;

using DocumentPacker.EventHandling;

/// <summary>
///     Interaction logic for ChangeLanguageView.xaml
/// </summary>
[DataContext(ApplicationElementPart.ChangeLanguage)]
public partial class ChangeLanguageView : IApplicationView
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ChangeLanguageView" /> class.
    /// </summary>
    public ChangeLanguageView()
    {
        this.InitializeComponent();
    }
}
