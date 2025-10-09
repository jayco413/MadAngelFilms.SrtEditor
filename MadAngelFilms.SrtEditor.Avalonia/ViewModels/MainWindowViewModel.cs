using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using MadAngelFilms.SrtEditor.Controllers;
using MadAngelFilms.SrtEditor.Core.Models;

namespace MadAngelFilms.SrtEditor.Avalonia.ViewModels;

public sealed class MainWindowViewModel : ViewModelBase
{
    private readonly MainWindowController _controller;
    private bool _isLoading;
    private string _statusMessage = "Select a project folder to begin.";
    private SubtitleEntryViewModel? _selectedSubtitle;
    private string _subtitleText = string.Empty;
    private string? _subtitleFilePath;
    private string? _videoFilePath;
    private double _playbackProgress;
    private string _playbackPositionText = "00:00:00.000 / --:--:--.---";
    private bool _suppressSubtitleTextUpdate;

    public MainWindowViewModel(MainWindowController controller)
    {
        _controller = controller;
        Subtitles = new ObservableCollection<SubtitleEntryViewModel>();
    }

    public ObservableCollection<SubtitleEntryViewModel> Subtitles { get; }

    public bool IsLoading
    {
        get => _isLoading;
        private set => SetProperty(ref _isLoading, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        private set => SetProperty(ref _statusMessage, value);
    }

    public SubtitleEntryViewModel? SelectedSubtitle
    {
        get => _selectedSubtitle;
        set
        {
            if (SetProperty(ref _selectedSubtitle, value))
            {
                _suppressSubtitleTextUpdate = true;
                SubtitleText = value?.Text ?? string.Empty;
                _suppressSubtitleTextUpdate = false;
                OnPropertyChanged(nameof(CanEditSubtitle));
                OnPropertyChanged(nameof(SelectedStartDisplay));
                OnPropertyChanged(nameof(SelectedEndDisplay));
            }
        }
    }

    public string SubtitleText
    {
        get => _subtitleText;
        set
        {
            if (SetProperty(ref _subtitleText, value) && !_suppressSubtitleTextUpdate && SelectedSubtitle is not null)
            {
                SelectedSubtitle.Text = value;
            }
        }
    }

    public string SelectedStartDisplay => SelectedSubtitle?.StartDisplay ?? "--:--:--.---";

    public string SelectedEndDisplay => SelectedSubtitle?.EndDisplay ?? "--:--:--.---";

    public bool CanEditSubtitle => SelectedSubtitle is not null && !IsLoading;

    public string? SubtitleFilePath
    {
        get => _subtitleFilePath;
        private set
        {
            if (SetProperty(ref _subtitleFilePath, value))
            {
                OnPropertyChanged(nameof(SubtitleFileName));
                OnPropertyChanged(nameof(HasProject));
            }
        }
    }

    public string? VideoFilePath
    {
        get => _videoFilePath;
        private set
        {
            if (SetProperty(ref _videoFilePath, value))
            {
                OnPropertyChanged(nameof(VideoFileName));
                OnPropertyChanged(nameof(HasVideo));
            }
        }
    }

    public string SubtitleFileName => SubtitleFilePath is null ? "No subtitle file" : Path.GetFileName(SubtitleFilePath);

    public string VideoFileName => VideoFilePath is null ? "No video file" : Path.GetFileName(VideoFilePath);

    public bool HasVideo => !string.IsNullOrWhiteSpace(VideoFilePath);

    public bool HasProject => HasVideo || !string.IsNullOrWhiteSpace(SubtitleFilePath);

    public double PlaybackProgress
    {
        get => _playbackProgress;
        set => SetProperty(ref _playbackProgress, value);
    }

    public string PlaybackPositionText
    {
        get => _playbackPositionText;
        private set => SetProperty(ref _playbackPositionText, value);
    }

    public async Task LoadProjectAsync(string directoryPath, CancellationToken cancellationToken = default)
    {
        if (IsLoading)
        {
            return;
        }

        IsLoading = true;
        StatusMessage = "Loading project...";

        try
        {
            SubtitleProject project = await _controller.LoadProjectAsync(directoryPath, cancellationToken);
            await UpdateFromProjectAsync(project);
            StatusMessage = $"Loaded {SubtitleFileName} with {Subtitles.Count} subtitles.";
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
            throw;
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void SetPlaybackPosition(TimeSpan current, TimeSpan total)
    {
        double progress = total.TotalMilliseconds <= 0 ? 0 : current.TotalMilliseconds / total.TotalMilliseconds;
        progress = Math.Clamp(progress, 0d, 1d);
        PlaybackProgress = progress;
        PlaybackPositionText = $"{FormatTime(current)} / {FormatTime(total)}";
    }

    public void ResetPlayback()
    {
        PlaybackProgress = 0;
        PlaybackPositionText = "00:00:00.000 / --:--:--.---";
    }

    private static string FormatTime(TimeSpan value)
    {
        if (value == TimeSpan.Zero)
        {
            return "00:00:00.000";
        }

        return value.ToString(@"hh\:mm\:ss\.fff");
    }

    private async Task UpdateFromProjectAsync(SubtitleProject project)
    {
        SubtitleFilePath = project.SubtitleFilePath;
        VideoFilePath = project.VideoFilePath;

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            Subtitles.Clear();
            foreach (SubtitleEntry entry in project.Entries)
            {
                Subtitles.Add(new SubtitleEntryViewModel(entry));
            }

            SelectedSubtitle = Subtitles.FirstOrDefault();
        });
    }
}
