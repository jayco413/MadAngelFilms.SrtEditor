using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using LibVLCSharp.Shared;
using MadAngelFilms.SrtEditor.Avalonia.ViewModels;

namespace MadAngelFilms.SrtEditor.Avalonia.Views;

public partial class MainWindow : AppWindow
{
    private readonly LibVLC _libVlc;
    private readonly MediaPlayer _mediaPlayer;
    private readonly DispatcherTimer _playbackTimer;
    private bool _isSeeking;
    private MainWindowViewModel? _attachedViewModel;

    public MainWindow()
    {
        InitializeComponent();
        LibVLCSharp.Shared.Core.Initialize();
        _libVlc = new LibVLC();
        _mediaPlayer = new MediaPlayer(_libVlc);
        VideoView.MediaPlayer = _mediaPlayer;
        _mediaPlayer.EndReached += MediaPlayerOnEndReached;
        _mediaPlayer.TimeChanged += MediaPlayerOnTimeChanged;

        _playbackTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(250)
        };
        _playbackTimer.Tick += (_, _) => UpdatePlaybackPosition();
    }

    private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext!;

    protected override void OnDataContextChanged(EventArgs e)
    {
        if (_attachedViewModel is not null)
        {
            _attachedViewModel.PropertyChanged -= ViewModelOnPropertyChanged;
        }

        _attachedViewModel = DataContext as MainWindowViewModel;
        if (_attachedViewModel is not null)
        {
            _attachedViewModel.PropertyChanged += ViewModelOnPropertyChanged;
            LoadVideo(_attachedViewModel.VideoFilePath);
        }

        base.OnDataContextChanged(e);
    }

    private async void OnOpenProjectClicked(object? sender, RoutedEventArgs e)
    {
        var dialog = new OpenFolderDialog
        {
            Title = "Select a project folder that contains an .srt and .mkv file."
        };

        string? folder = await dialog.ShowAsync(this);
        if (string.IsNullOrWhiteSpace(folder))
        {
            return;
        }

        try
        {
            await ViewModel.LoadProjectAsync(folder);
            LoadVideo(ViewModel.VideoFilePath);
        }
        catch (Exception ex)
        {
            await ShowMessageAsync("Unable to load project", ex.Message);
            ViewModel.ResetPlayback();
        }
    }

    private void LoadVideo(string? videoPath)
    {
        _mediaPlayer.Stop();
        _playbackTimer.Stop();
        ViewModel.ResetPlayback();
        bool hasVideo = !string.IsNullOrWhiteSpace(videoPath) && File.Exists(videoPath);
        VideoPlaceholder.IsVisible = !hasVideo;
        VideoView.IsVisible = hasVideo;

        if (!hasVideo)
        {
            return;
        }

        _mediaPlayer.Media?.Dispose();
        _mediaPlayer.Media = new Media(_libVlc, new Uri(videoPath!, UriKind.Absolute));
        _mediaPlayer.Media.Parse(MediaParseOptions.ParseLocal);
    }

    private async Task ShowMessageAsync(string title, string message)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            PrimaryButtonText = "OK"
        };

        await dialog.ShowAsync(this);
    }

    private void OnPlayClicked(object? sender, RoutedEventArgs e)
    {
        if (!_mediaPlayer.IsPlaying)
        {
            _mediaPlayer.Play();
            _playbackTimer.Start();
        }
    }

    private void OnPauseClicked(object? sender, RoutedEventArgs e)
    {
        if (_mediaPlayer.IsPlaying)
        {
            _mediaPlayer.Pause();
        }
        _playbackTimer.Stop();
    }

    private void OnStopClicked(object? sender, RoutedEventArgs e)
    {
        _mediaPlayer.Stop();
        _playbackTimer.Stop();
        ViewModel.ResetPlayback();
    }

    private void OnStepBackClicked(object? sender, RoutedEventArgs e)
    {
        if (_mediaPlayer.Length <= 0)
        {
            return;
        }

        long newTime = Math.Max(0, _mediaPlayer.Time - 42);
        _mediaPlayer.Time = newTime;
        UpdatePlaybackPosition();
    }

    private void OnStepForwardClicked(object? sender, RoutedEventArgs e)
    {
        if (_mediaPlayer.Length <= 0)
        {
            return;
        }

        long newTime = Math.Min(_mediaPlayer.Length, _mediaPlayer.Time + 42);
        _mediaPlayer.Time = newTime;
        UpdatePlaybackPosition();
    }

    private void OnSeekStarted(object? sender, PointerPressedEventArgs e)
    {
        _isSeeking = true;
        _playbackTimer.Stop();
    }

    private void OnSeekCompleted(object? sender, PointerReleasedEventArgs e)
    {
        if (_mediaPlayer.Length > 0)
        {
            double progress = ViewModel.PlaybackProgress;
            _mediaPlayer.Position = (float)progress;
            UpdatePlaybackPosition();
        }

        _isSeeking = false;
        if (_mediaPlayer.IsPlaying)
        {
            _playbackTimer.Start();
        }
    }

    private void OnSeekMoved(object? sender, PointerEventArgs e)
    {
        if (_isSeeking && _mediaPlayer.Length > 0)
        {
            double progress = ViewModel.PlaybackProgress;
            TimeSpan total = TimeSpan.FromMilliseconds(_mediaPlayer.Length);
            TimeSpan current = TimeSpan.FromMilliseconds(total.TotalMilliseconds * progress);
            ViewModel.SetPlaybackPosition(current, total);
        }
    }

    private void UpdatePlaybackPosition()
    {
        if (_mediaPlayer.Length <= 0)
        {
            ViewModel.ResetPlayback();
            return;
        }

        if (!_isSeeking)
        {
            double progress = _mediaPlayer.Position;
            ViewModel.PlaybackProgress = progress;
        }

        TimeSpan current = TimeSpan.FromMilliseconds(_mediaPlayer.Time);
        TimeSpan total = TimeSpan.FromMilliseconds(_mediaPlayer.Length);
        ViewModel.SetPlaybackPosition(current, total);
    }

    private void MediaPlayerOnTimeChanged(object? sender, MediaPlayerTimeChangedEventArgs e)
    {
        if (_isSeeking)
        {
            return;
        }

        Dispatcher.UIThread.Post(UpdatePlaybackPosition);
    }

    private void MediaPlayerOnEndReached(object? sender, EventArgs e)
    {
        Dispatcher.UIThread.Post(() =>
        {
            _mediaPlayer.Stop();
            _playbackTimer.Stop();
            ViewModel.ResetPlayback();
        });
    }

    private void ViewModelOnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainWindowViewModel.VideoFilePath))
        {
            LoadVideo(ViewModel.VideoFilePath);
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void OnExitClicked(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        _playbackTimer.Stop();
        if (_attachedViewModel is not null)
        {
            _attachedViewModel.PropertyChanged -= ViewModelOnPropertyChanged;
            _attachedViewModel = null;
        }
        _mediaPlayer.EndReached -= MediaPlayerOnEndReached;
        _mediaPlayer.TimeChanged -= MediaPlayerOnTimeChanged;
        _mediaPlayer.Dispose();
        _libVlc.Dispose();
    }
}
