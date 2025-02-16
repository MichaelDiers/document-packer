namespace DocumentPacker.Resources;

using System.Globalization;

internal class CultureInfoChangedEventArgs(CultureInfo cultureInfo) : EventArgs
{
    public CultureInfo CultureInfo => cultureInfo;
}
