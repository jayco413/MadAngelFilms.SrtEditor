using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using LibVLCSharp.Avalonia;
using LibVLCSharp.Shared;
using MadAngelFilms.SrtEditor.Avalonia.ViewModels;

namespace MadAngelFilms.SrtEditor.Avalonia.Views;

public partial class MainWindow : AppWindow
{
    private static readonly object LibVlcInitializationLock = new();
    private static bool _isLibVlcInitialized;

    private readonly LibVLC _libVlc;
    private readonly MediaPlayer _mediaPlayer;
    private readonly DispatcherTimer _playbackTimer;
    private bool _isSeeking;
    private MainWindowViewModel? _attachedViewModel;
    private readonly VideoView _videoView;
    private readonly TextBlock _videoPlaceholder;

    public MainWindow()
    {
        InitializeComponent();
        _videoView = this.FindControl<VideoView>("VideoView")
                      ?? throw new InvalidOperationException("VideoView control could not be located in the visual tree.");
        _videoPlaceholder = this.FindControl<TextBlock>("VideoPlaceholder")
                             ?? throw new InvalidOperationException("VideoPlaceholder control could not be located in the visual tree.");
        InitializeLibVlc();
        _libVlc = new LibVLC();
        _mediaPlayer = new MediaPlayer(_libVlc);
        _videoView.MediaPlayer = _mediaPlayer;
        _mediaPlayer.EndReached += MediaPlayerOnEndReached;
        _mediaPlayer.TimeChanged += MediaPlayerOnTimeChanged;

        _playbackTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(250)
        };
        _playbackTimer.Tick += OnPlaybackTimerTick;
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
        if (StorageProvider is null)
        {
            await ShowMessageAsync("Folder picker unavailable", "This platform does not expose a folder picker.");
            return;
        }

        var options = new FolderPickerOpenOptions
        {
            AllowMultiple = false,
            Title = "Select a project folder that contains an .srt and .mkv file."
        };

        IReadOnlyList<IStorageFolder> folders;
        try
        {
            folders = await StorageProvider.OpenFolderPickerAsync(options);
        }
        catch (NotSupportedException)
        {
            await ShowMessageAsync("Folder picker unavailable", "This platform does not support selecting folders.");
            return;
        }

        IStorageFolder? folder = folders.FirstOrDefault();
        if (folder is null)
        {
            return;
        }

        string? folderPath = folder.TryGetLocalPath();
        if (string.IsNullOrWhiteSpace(folderPath))
        {
            await ShowMessageAsync("Folder picker unavailable", "The selected folder does not provide a local file system path.");
            return;
        }

        try
        {
            await ViewModel.LoadProjectAsync(folderPath);
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
        _videoPlaceholder.IsVisible = !hasVideo;
        _videoView.IsVisible = hasVideo;

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

    private void OnPlaybackTimerTick(object? sender, EventArgs e)
    {
        UpdatePlaybackPosition();
    }

    protected override void OnClosed(EventArgs e)
    {
        _playbackTimer.Stop();
        _playbackTimer.Tick -= OnPlaybackTimerTick;
        if (_attachedViewModel is not null)
        {
            _attachedViewModel.PropertyChanged -= ViewModelOnPropertyChanged;
            _attachedViewModel = null;
        }
        _mediaPlayer.EndReached -= MediaPlayerOnEndReached;
        _mediaPlayer.TimeChanged -= MediaPlayerOnTimeChanged;
        _videoView.MediaPlayer = null;
        _mediaPlayer.Dispose();
        _libVlc.Dispose();
        base.OnClosed(e);
    }

    private static void InitializeLibVlc()
    {
        if (_isLibVlcInitialized)
        {
            return;
        }

        lock (LibVlcInitializationLock)
        {
            if (_isLibVlcInitialized)
            {
                return;
            }

            string? libVlcDirectory = TryGetLibVlcDirectory();
            if (!string.IsNullOrEmpty(libVlcDirectory))
            {
                LibVLCSharp.Shared.Core.Initialize(libVlcDirectory);
            }
            else
            {
                LibVLCSharp.Shared.Core.Initialize();
            }

            _isLibVlcInitialized = true;
        }
    }

    private static string? TryGetLibVlcDirectory()
    {
        string baseDirectory = AppContext.BaseDirectory;
        string libraryName = GetLibVlcLibraryName();

        foreach (string candidate in GetLibVlcCandidatePaths(baseDirectory))
        {
            if (File.Exists(Path.Combine(candidate, libraryName)))
            {
                return candidate;
            }
        }

        return null;
    }

    private static IEnumerable<string> GetLibVlcCandidatePaths(string baseDirectory)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            yield return Path.Combine(baseDirectory, "libvlc", "win-x64");
            yield return Path.Combine(baseDirectory, "runtimes", "win-x64", "native");
            yield return Path.Combine(baseDirectory, "libvlc", "win-x86");
            yield return Path.Combine(baseDirectory, "runtimes", "win-x86", "native");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            yield return Path.Combine(baseDirectory, "libvlc", "osx-x64");
            yield return Path.Combine(baseDirectory, "runtimes", "osx-x64", "native");
            yield return Path.Combine(baseDirectory, "libvlc", "osx-arm64");
            yield return Path.Combine(baseDirectory, "runtimes", "osx-arm64", "native");
        }
        else
        {
            yield return Path.Combine(baseDirectory, "libvlc", "linux-x64");
            yield return Path.Combine(baseDirectory, "runtimes", "linux-x64", "native");
            yield return Path.Combine(baseDirectory, "libvlc", "linux-arm64");
            yield return Path.Combine(baseDirectory, "runtimes", "linux-arm64", "native");
        }

        yield return baseDirectory;
    }

    private static string GetLibVlcLibraryName()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "libvlc.dll";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return "libvlc.dylib";
        }

        return "libvlc.so";
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void OnExitClicked(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
