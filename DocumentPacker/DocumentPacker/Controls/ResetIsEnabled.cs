namespace DocumentPacker.Controls;

using System.Windows;
using System.Windows.Controls;

public class ResetIsEnabled : ContentControl
{
    static ResetIsEnabled()
    {
        UIElement.IsEnabledProperty.OverrideMetadata(
            typeof(ResetIsEnabled),
            new UIPropertyMetadata(
                true,
                (_, __) => { },
                (_, x) => x));
    }
}
