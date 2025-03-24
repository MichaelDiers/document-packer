//
//    The MIT License (MIT)
//
//    Copyright (c) 2016-2025 jbe2277
//
//    Source: https://github.com/jbe2277/waf/blob/master/src/System.Waf/System.Waf/System.Waf.Wpf/Applications/DispatcherHelperCore.cs

namespace DocumentPacker.Tests.Parts.Main.CreateConfigurationPart.ViewModels;

using System.Windows.Threading;

internal static class DispatcherHelperCore
{
    internal static void DoEvents()
    {
        var frame = new DispatcherFrame();
        Dispatcher.CurrentDispatcher.BeginInvoke(
            DispatcherPriority.Background,
            new DispatcherOperationCallback(DispatcherHelperCore.ExitFrame),
            frame);
        Dispatcher.PushFrame(frame);
    }

    private static object? ExitFrame(object frame)
    {
        ((DispatcherFrame) frame).Continue = false;
        return null;
    }
}
