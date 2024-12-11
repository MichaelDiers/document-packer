﻿namespace DocumentPacker.Parts.DocumentPackerPart.Contracts;

using System.ComponentModel;
using DocumentPacker.Parts.Contracts;

/// <summary>
///     The main view model of the application that is the data context of the application window.
/// </summary>
internal interface IDocumentPackerViewModel : INotifyPropertyChanged
{
    /// <summary>
    ///     Gets the icon of the application window.
    /// </summary>
    string Icon { get; }

    /// <summary>
    ///     Gets the version of the application.
    /// </summary>
    string Version { get; }

    /// <summary>
    ///     Gets the view model part.
    /// </summary>
    IPartViewModel ViewModelPart { get; }
}
