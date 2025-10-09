using System;
using MadAngelFilms.SrtEditor.Core.Models;

namespace MadAngelFilms.SrtEditor.Avalonia.ViewModels;

public sealed class SubtitleEntryViewModel : ViewModelBase
{
    private string _text;

    public SubtitleEntryViewModel(SubtitleEntry entry)
    {
        SequenceNumber = entry.SequenceNumber;
        Start = entry.StartTime;
        End = entry.EndTime;
        _text = entry.Text;
    }

    public int SequenceNumber { get; }

    public TimeSpan Start { get; }

    public TimeSpan End { get; }

    public string StartDisplay => Start.ToString(@"hh\:mm\:ss\.fff");

    public string EndDisplay => End.ToString(@"hh\:mm\:ss\.fff");

    public string Text
    {
        get => _text;
        set => SetProperty(ref _text, value);
    }
}
