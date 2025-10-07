using System.Collections.Generic;

namespace MadAngelFilms.SrtEditor.Core.Models;

/// <summary>
/// Represents a subtitle project pairing subtitle entries with a video file.
/// </summary>
public sealed class SubtitleProject
{
    public SubtitleProject(string subtitleFilePath, string videoFilePath, IReadOnlyList<SubtitleEntry> entries)
    {
        SubtitleFilePath = subtitleFilePath;
        VideoFilePath = videoFilePath;
        Entries = entries;
    }

    public string SubtitleFilePath { get; }

    public string VideoFilePath { get; }

    public IReadOnlyList<SubtitleEntry> Entries { get; }
}
