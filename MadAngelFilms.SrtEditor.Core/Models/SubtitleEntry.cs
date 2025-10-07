namespace MadAngelFilms.SrtEditor.Core.Models;

/// <summary>
/// Represents a single subtitle entry.
/// </summary>
public sealed class SubtitleEntry
{
    public SubtitleEntry(int sequenceNumber, TimeSpan startTime, TimeSpan endTime, string text)
    {
        SequenceNumber = sequenceNumber;
        StartTime = startTime;
        EndTime = endTime;
        Text = text;
    }

    public int SequenceNumber { get; }

    public TimeSpan StartTime { get; }

    public TimeSpan EndTime { get; }

    public string Text { get; }
}
