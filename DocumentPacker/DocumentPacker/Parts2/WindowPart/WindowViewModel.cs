﻿namespace DocumentPacker.Parts2.WindowPart;

using DocumentPacker.EventHandling;
using DocumentPacker.Mvvm;
using DocumentPacker.Resources;

/// <summary>
///     View model of the <see cref="WindowView" />.
/// </summary>
/// <seealso cref="DocumentPacker.Mvvm.ApplicationViewModel" />
internal class WindowViewModel : ApplicationViewModel
{
    /// <summary>
    ///     The view that is displayed in the window.
    /// </summary>
    private object? view;

    /// <summary>
    ///     The title of the window.
    /// </summary>
    private string? windowTitle = Translation.WindowPartTitle;

    /// <summary>
    ///     Gets or sets the view that is displayed in the window.
    /// </summary>
    [ApplicationPart(ApplicationElementPart.Layout)]
    public object? View
    {
        get => this.view;
        set =>
            this.SetField(
                ref this.view,
                value);
    }

    /// <summary>
    ///     Gets or sets the title of the window.
    /// </summary>
    public string? WindowTitle
    {
        get => this.windowTitle;
        set =>
            this.SetField(
                ref this.windowTitle,
                value);
    }
}
