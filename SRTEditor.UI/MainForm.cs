using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MadAngelFilms.SrtEditor.Controllers;
using MadAngelFilms.SrtEditor.Core.Models;
using MadAngelFilms.SrtEditor.UI.Extensions;
using MadAngelFilms.SrtEditor.UI.Resources;
using MaterialSkin;
using MaterialSkin.Controls;
using LibVLCSharp.Shared;

namespace MadAngelFilms.SrtEditor.UI;

public partial class MainForm : MaterialForm
{
    private readonly MainFormController _controller;
    private SubtitleProject? _currentProject;
    private List<SubtitleEntry> _subtitleEntries = new();
    private CancellationTokenSource? _loadingCancellation;
    private LibVLC? _libVlc;
    private MediaPlayer? _mediaPlayer;
    private Media? _currentMedia;
    private System.Windows.Forms.Timer? _playbackTimer;
    private bool _isSeeking;
    private bool _suppressSubtitleTextChanges;
    private const int TextColumnIndex = 3;
    private const int ActionColumnIndex = 4;

    public MainForm(MainFormController controller)
    {
        _controller = controller;
        InitializeComponent();
        ConfigureMaterialSkin();
        ConfigureControls();
        InitializeVideoPlayback();
        FormClosed += MainForm_FormClosed;
    }

    private void ConfigureMaterialSkin()
    {
        var manager = MaterialSkinManager.Instance;
        manager.EnforceBackcolorOnAllComponents = true;
        manager.AddFormToManage(this);
        manager.Theme = MaterialSkinManager.Themes.DARK;
        manager.ColorScheme = new ColorScheme(
            Color.FromArgb(0x7C, 0x0A, 0x02),
            Color.FromArgb(0x4A, 0x00, 0x00),
            Color.FromArgb(0x99, 0x2A, 0x1C),
            Color.FromArgb(0xC2, 0x9F, 0x13),
            TextShade.WHITE);
    }

    private void ConfigureControls()
    {
        subtitleListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
        subtitleListView.MultiSelect = true;
        subtitleListView.AccessibleName = "Subtitle entries";
        subtitleTextBox.AccessibleName = "Subtitle text";
        subtitleTextBox.Text = string.Empty;
        subtitleTimingLabel.Text = "Start: --:--:--.---  End: --:--:--.---";
        videoFileLabel.Text = "Video Preview";
        videoPlaceholderLabel.Text = "Video playback preview will appear here.";
        videoPlaceholderLabel.BackColor = Color.FromArgb(11, 29, 58);
        AcceptButton = null;
        CancelButton = null;
        var normalFont = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
        subtitleHeaderLabel.Font = normalFont;
        videoFileLabel.Font = normalFont;
        subtitleTimingLabel.Font = normalFont;
        subtitleListView.Font = normalFont;
        subtitleTextBox.Font = normalFont;
        videoPlaceholderLabel.Font = new Font("Roboto", 16F, FontStyle.Italic, GraphicsUnit.Pixel);
        mainMenuStrip.Font = normalFont;
        mainMenuStrip.Padding = new Padding(8, 8, 8, 8);
        ConfigureIcons();
        playbackTrackBar.Minimum = 0;
        playbackTrackBar.Maximum = 1000;
        playbackTrackBar.Value = 0;
        playbackTrackBar.TickStyle = TickStyle.None;
        playbackTrackBar.MouseDown += PlaybackTrackBar_MouseDown;
        playbackTrackBar.MouseUp += PlaybackTrackBar_MouseUp;
        playbackTrackBar.Scroll += PlaybackTrackBar_Scroll;
        playButton.Click += PlayButton_Click;
        pauseButton.Click += PauseButton_Click;
        stopButton.Click += StopButton_Click;
        combineSelectedButton.Click += CombineSelectedButton_Click;
        videoView.Visible = false;
        videoPlaceholderLabel.BringToFront();
        UpdatePlaybackControlsEnabledState(isFormLoading: false);
        subtitleListView.MouseClick += SubtitleListView_MouseClick;
        subtitleTextBox.TextChanged += SubtitleTextBox_TextChanged;
    }

    private void ConfigureIcons()
    {
        mainMenuStrip.ImageScalingSize = new Size(20, 20);

        SetMenuIcon(fileMenuItem, "FileMenu");
        SetMenuIcon(fileNewMenuItem, "FileNew");
        SetMenuIcon(fileOpenMenuItem, "FileOpen");
        SetMenuIcon(fileSaveMenuItem, "FileSave");
        SetMenuIcon(fileExitMenuItem, "FileExit");

        SetMenuIcon(editMenuItem, "EditMenu");
        SetMenuIcon(editUndoMenuItem, "EditUndo");
        SetMenuIcon(editRedoMenuItem, "EditRedo");
        SetMenuIcon(editCutMenuItem, "EditCut");
        SetMenuIcon(editCopyMenuItem, "EditCopy");
        SetMenuIcon(editPasteMenuItem, "EditPaste");
        SetMenuIcon(editDeleteMenuItem, "EditDelete");

        SetMenuIcon(viewMenuItem, "ViewMenu");
        SetMenuIcon(viewTogglePanelsMenuItem, "ViewTogglePanels");
        SetMenuIcon(viewThemesMenuItem, "ViewThemes");
        SetMenuIcon(viewRefreshMenuItem, "ViewRefresh");

        SetMenuIcon(toolsMenuItem, "ToolsMenu");
        SetMenuIcon(toolsOptionsMenuItem, "ToolsOptions");
        SetMenuIcon(toolsSettingsMenuItem, "ToolsSettings");
        SetMenuIcon(toolsPreferencesMenuItem, "ToolsPreferences");

        SetMenuIcon(helpMenuItem, "HelpMenu");
        SetMenuIcon(helpDocumentationMenuItem, "HelpDocumentation");
        SetMenuIcon(helpCheckForUpdatesMenuItem, "HelpCheckForUpdates");
        SetMenuIcon(helpAboutMenuItem, "HelpAbout");

        playButton.Icon = IconProvider.GetIcon("PlaybackPlay", 28);
        pauseButton.Icon = IconProvider.GetIcon("PlaybackPause", 28);
        stopButton.Icon = IconProvider.GetIcon("PlaybackStop", 28);
    }

    private static void SetMenuIcon(ToolStripMenuItem menuItem, string iconName)
    {
        menuItem.Image = IconProvider.GetIcon(iconName, 20);
        menuItem.ImageScaling = ToolStripItemImageScaling.None;
    }

    private async void FileOpenMenuItem_Click(object? sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog
        {
            Description = "Select a project folder that contains an .srt and .mkv file.",
            ShowNewFolderButton = false,
            UseDescriptionForTitle = true
        };

        if (dialog.ShowDialog(this) == DialogResult.OK)
        {
            await LoadProjectAsync(dialog.SelectedPath).ConfigureAwait(false);
        }
    }

    private void InitializeVideoPlayback()
    {
        LibVLCSharp.Shared.Core.Initialize();
        _libVlc = new LibVLC();
        _mediaPlayer = new MediaPlayer(_libVlc);
        _mediaPlayer.EndReached += MediaPlayer_EndReached;
        videoView.MediaPlayer = _mediaPlayer;
        _playbackTimer = new System.Windows.Forms.Timer
        {
            Interval = 200
        };
        _playbackTimer.Tick += PlaybackTimer_Tick;
    }

    private async Task LoadProjectAsync(string directoryPath)
    {
        CancelLoading();
        var cancellationSource = new CancellationTokenSource();
        _loadingCancellation = cancellationSource;
        SetLoadingState(isLoading: true);

        try
        {
            SubtitleProject project = await _controller.LoadProjectAsync(directoryPath, cancellationSource.Token).ConfigureAwait(false);
            if (cancellationSource.IsCancellationRequested)
            {
                return;
            }

            await this.InvokeAsync(() => ApplyProject(project)).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await this.InvokeAsync(() =>
            {
                MessageBox.Show(
                    this,
                    ex.Message,
                    "Load Project",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }).ConfigureAwait(false);
        }
        finally
        {
            await this.InvokeAsync(() => SetLoadingState(isLoading: false)).ConfigureAwait(false);
            cancellationSource.Dispose();
            if (ReferenceEquals(_loadingCancellation, cancellationSource))
            {
                _loadingCancellation = null;
            }
        }
    }

    private void ApplyProject(SubtitleProject project)
    {
        ResetVideoState();
        _currentProject = project;
        _subtitleEntries = project.Entries
            .Select(entry => new SubtitleEntry(entry.SequenceNumber, entry.StartTime, entry.EndTime, entry.Text))
            .ToList();
        RefreshSubtitleList(selectedIndex: 0);
        fileSaveMenuItem.Enabled = true;
        videoFileLabel.Text = $"Video Preview – {Path.GetFileName(project.VideoFilePath)}";
        LoadVideo(project.VideoFilePath);
    }

    private void RefreshSubtitleList(int? selectedIndex = null)
    {
        subtitleListView.BeginUpdate();
        subtitleListView.Items.Clear();

        foreach (SubtitleEntry entry in _subtitleEntries)
        {
            var item = new ListViewItem(entry.SequenceNumber.ToString(CultureInfo.InvariantCulture))
            {
                Tag = entry
            };
            item.SubItems.Add(entry.StartTime.ToString(@"hh\:mm\:ss\.fff", CultureInfo.InvariantCulture));
            item.SubItems.Add(entry.EndTime.ToString(@"hh\:mm\:ss\.fff", CultureInfo.InvariantCulture));
            item.SubItems.Add(entry.Text);
            item.SubItems.Add("Delete");
            subtitleListView.Items.Add(item);
        }

        subtitleListView.EndUpdate();

        if (subtitleListView.Items.Count == 0)
        {
            ClearSubtitleDetails();
            UpdateActionButtonsState();
            return;
        }

        int targetIndex = selectedIndex ?? 0;
        targetIndex = Math.Clamp(targetIndex, 0, subtitleListView.Items.Count - 1);
        subtitleListView.Items[targetIndex].Selected = true;
        subtitleListView.Items[targetIndex].Focused = true;
        subtitleListView.EnsureVisible(targetIndex);
        subtitleListView.Focus();
        UpdateActionButtonsState();
    }

    private void LoadVideo(string videoPath)
    {
        if (_mediaPlayer is null || _libVlc is null)
        {
            return;
        }

        StopPlaybackInternal(resetPosition: true);
        _currentMedia?.Dispose();
        _currentMedia = new Media(_libVlc, videoPath, FromType.FromPath);
        _mediaPlayer.Media = _currentMedia;
        SetTrackBarValueSafely(playbackTrackBar.Minimum);
        videoPlaceholderLabel.Visible = false;
        videoView.Visible = true;
        UpdatePlaybackControlsEnabledState(isFormLoading: false);
    }

    private void CancelLoading()
    {
        if (_loadingCancellation is { IsCancellationRequested: false })
        {
            _loadingCancellation.Cancel();
        }
    }

    private void SetLoadingState(bool isLoading)
    {
        UseWaitCursor = isLoading;
        mainMenuStrip.Enabled = !isLoading;
        subtitleListView.Enabled = !isLoading;
        if (isLoading)
        {
            StopPlaybackInternal(resetPosition: true);
            combineSelectedButton.Enabled = false;
            subtitleTextBox.Enabled = false;
        }

        UpdatePlaybackControlsEnabledState(isLoading);
    }

    private void SubtitleListView_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (subtitleListView.SelectedItems.Count != 1)
        {
            ClearSubtitleDetails();
            UpdateActionButtonsState();
            return;
        }

        ListViewItem selectedItem = subtitleListView.SelectedItems[0];
        if (selectedItem.Tag is not SubtitleEntry entry)
        {
            ClearSubtitleDetails();
            UpdateActionButtonsState();
            return;
        }

        string startText = entry.StartTime.ToString(@"hh\:mm\:ss\.fff", CultureInfo.InvariantCulture);
        string endText = entry.EndTime.ToString(@"hh\:mm\:ss\.fff", CultureInfo.InvariantCulture);
        subtitleTimingLabel.Text = $"Start: {startText}  End: {endText}";
        _suppressSubtitleTextChanges = true;
        subtitleTextBox.Text = entry.Text;
        subtitleTextBox.Enabled = true;
        _suppressSubtitleTextChanges = false;
        if (_mediaPlayer is MediaPlayer player && player.Media is not null && player.IsSeekable)
        {
            long startMilliseconds = (long)entry.StartTime.TotalMilliseconds;
            player.Time = startMilliseconds;
            UpdateTrackBarFromMediaTime(startMilliseconds, player.Length);
        }
        UpdateActionButtonsState();
    }

    private void ClearSubtitleDetails()
    {
        subtitleTimingLabel.Text = "Start: --:--:--.---  End: --:--:--.---";
        _suppressSubtitleTextChanges = true;
        subtitleTextBox.Text = string.Empty;
        _suppressSubtitleTextChanges = false;
        subtitleTextBox.Enabled = false;
        if (_currentMedia is null)
        {
            videoPlaceholderLabel.Text = "Video playback preview will appear here.";
            videoPlaceholderLabel.Visible = true;
            videoView.Visible = false;
        }
    }

    private void UpdateActionButtonsState()
    {
        if (!subtitleListView.Enabled)
        {
            combineSelectedButton.Enabled = false;
            return;
        }

        if (subtitleListView.SelectedIndices.Count != 2)
        {
            combineSelectedButton.Enabled = false;
            return;
        }

        int[] indices = subtitleListView.SelectedIndices.Cast<int>().OrderBy(index => index).ToArray();
        bool canCombine = indices.Length == 2 && indices[1] - indices[0] == 1;
        combineSelectedButton.Enabled = canCombine;
    }

    private void SubtitleListView_MouseClick(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left)
        {
            return;
        }

        ListViewHitTestInfo hit = subtitleListView.HitTest(e.Location);
        if (hit.Item is null || hit.SubItem is null)
        {
            return;
        }

        int subItemIndex = hit.Item.SubItems.IndexOf(hit.SubItem);
        if (subItemIndex == ActionColumnIndex)
        {
            DeleteEntryAtIndex(hit.Item.Index);
        }
    }

    private void DeleteEntryAtIndex(int index)
    {
        if (index < 0 || index >= _subtitleEntries.Count)
        {
            return;
        }

        _subtitleEntries.RemoveAt(index);
        ReindexEntries();
        UpdateCurrentProjectEntries();
        int nextIndex = Math.Min(index, _subtitleEntries.Count - 1);
        RefreshSubtitleList(nextIndex >= 0 ? nextIndex : (int?)null);
    }

    private void ReindexEntries()
    {
        _subtitleEntries = _subtitleEntries
            .Select((entry, position) => new SubtitleEntry(position + 1, entry.StartTime, entry.EndTime, entry.Text))
            .ToList();
    }

    private void UpdateCurrentProjectEntries()
    {
        if (_currentProject is null)
        {
            return;
        }

        _currentProject = new SubtitleProject(
            _currentProject.SubtitleFilePath,
            _currentProject.VideoFilePath,
            _subtitleEntries);
    }

    private void CombineSelectedButton_Click(object? sender, EventArgs e)
    {
        if (subtitleListView.SelectedIndices.Count != 2)
        {
            return;
        }

        int[] indices = subtitleListView.SelectedIndices.Cast<int>().OrderBy(index => index).ToArray();
        if (indices.Length != 2 || indices[1] - indices[0] != 1)
        {
            MessageBox.Show(
                this,
                "Please select two adjacent subtitle entries to combine.",
                "Combine Subtitles",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        SubtitleEntry firstEntry = _subtitleEntries[indices[0]];
        SubtitleEntry secondEntry = _subtitleEntries[indices[1]];
        string combinedText = string.Join(
            Environment.NewLine,
            new[] { firstEntry.Text, secondEntry.Text }
                .Where(text => !string.IsNullOrWhiteSpace(text)));

        var combinedEntry = new SubtitleEntry(
            firstEntry.SequenceNumber,
            firstEntry.StartTime,
            secondEntry.EndTime,
            combinedText);

        _subtitleEntries[indices[0]] = combinedEntry;
        _subtitleEntries.RemoveAt(indices[1]);
        ReindexEntries();
        UpdateCurrentProjectEntries();
        RefreshSubtitleList(indices[0]);
    }

    private void SubtitleTextBox_TextChanged(object? sender, EventArgs e)
    {
        if (_suppressSubtitleTextChanges || subtitleListView.SelectedItems.Count != 1)
        {
            return;
        }

        int selectedIndex = subtitleListView.SelectedItems[0].Index;
        if (selectedIndex < 0 || selectedIndex >= _subtitleEntries.Count)
        {
            return;
        }

        SubtitleEntry currentEntry = _subtitleEntries[selectedIndex];
        if (subtitleTextBox.Text == currentEntry.Text)
        {
            return;
        }

        var updatedEntry = new SubtitleEntry(
            currentEntry.SequenceNumber,
            currentEntry.StartTime,
            currentEntry.EndTime,
            subtitleTextBox.Text);
        _subtitleEntries[selectedIndex] = updatedEntry;
        subtitleListView.SelectedItems[0].SubItems[TextColumnIndex].Text = subtitleTextBox.Text;
        subtitleListView.SelectedItems[0].Tag = updatedEntry;
        UpdateCurrentProjectEntries();
    }

    private void FileExitMenuItem_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void HelpAboutMenuItem_Click(object? sender, EventArgs e)
    {
        const string message = "Mad Angel Films Subtitle Editor\nVersion 0.1";
        MessageBox.Show(this, message, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void PlayButton_Click(object? sender, EventArgs e)
    {
        if (_mediaPlayer is null || _currentMedia is null)
        {
            return;
        }

        if (_mediaPlayer.Play())
        {
            videoPlaceholderLabel.Visible = false;
            videoView.Visible = true;
            _playbackTimer?.Start();
        }
    }

    private void PauseButton_Click(object? sender, EventArgs e)
    {
        if (_mediaPlayer is null)
        {
            return;
        }

        _mediaPlayer.Pause();
        _playbackTimer?.Stop();
    }

    private void StopButton_Click(object? sender, EventArgs e)
    {
        StopPlaybackInternal(resetPosition: true);
    }

    private void PlaybackTrackBar_MouseDown(object? sender, MouseEventArgs e)
    {
        _isSeeking = true;
    }

    private void PlaybackTrackBar_MouseUp(object? sender, MouseEventArgs e)
    {
        _isSeeking = false;
        SeekToTrackBarPosition();
    }

    private void PlaybackTrackBar_Scroll(object? sender, EventArgs e)
    {
        if (_isSeeking)
        {
            return;
        }

        SeekToTrackBarPosition();
    }

    private void SeekToTrackBarPosition()
    {
        if (_mediaPlayer is not MediaPlayer player || !player.IsSeekable || _currentMedia is null)
        {
            return;
        }

        float target = playbackTrackBar.Value / (float)playbackTrackBar.Maximum;
        target = Math.Clamp(target, 0f, 1f);
        player.Position = target;
    }

    private void PlaybackTimer_Tick(object? sender, EventArgs e)
    {
        if (_mediaPlayer is null || _isSeeking)
        {
            return;
        }

        long total = _mediaPlayer.Length;
        long current = _mediaPlayer.Time;
        UpdateTrackBarFromMediaTime(current, total);
    }

    private void MediaPlayer_EndReached(object? sender, EventArgs e)
    {
        _ = this.InvokeAsync(() => StopPlaybackInternal(resetPosition: true));
    }

    private void StopPlaybackInternal(bool resetPosition)
    {
        _playbackTimer?.Stop();

        if (_mediaPlayer is null)
        {
            return;
        }

        if (_mediaPlayer.IsPlaying)
        {
            _mediaPlayer.Stop();
        }

        if (resetPosition)
        {
            SetTrackBarValueSafely(playbackTrackBar.Minimum);
        }
    }

    private void ResetVideoState()
    {
        StopPlaybackInternal(resetPosition: true);

        if (_mediaPlayer is not null)
        {
            _mediaPlayer.Media = null;
        }

        _currentMedia?.Dispose();
        _currentMedia = null;
        SetTrackBarValueSafely(playbackTrackBar.Minimum);
        videoView.Visible = false;
        videoPlaceholderLabel.Visible = true;
        videoPlaceholderLabel.Text = "Video playback preview will appear here.";
        UpdatePlaybackControlsEnabledState(isFormLoading: false);
    }

    private void UpdatePlaybackControlsEnabledState(bool isFormLoading)
    {
        bool hasMedia = !isFormLoading && _currentMedia is not null;
        playButton.Enabled = hasMedia;
        pauseButton.Enabled = hasMedia;
        stopButton.Enabled = hasMedia;
        playbackTrackBar.Enabled = hasMedia;
    }

    private void UpdateTrackBarFromMediaTime(long currentMilliseconds, long totalMilliseconds)
    {
        if (totalMilliseconds <= 0)
        {
            return;
        }

        float position = Math.Clamp((float)currentMilliseconds / totalMilliseconds, 0f, 1f);
        int targetValue = (int)Math.Round(position * playbackTrackBar.Maximum, MidpointRounding.AwayFromZero);
        targetValue = Math.Clamp(targetValue, playbackTrackBar.Minimum, playbackTrackBar.Maximum);

        SetTrackBarValueSafely(targetValue);
    }

    private void MainForm_FormClosed(object? sender, FormClosedEventArgs e)
    {
        DisposePlaybackResources();
    }

    private void DisposePlaybackResources()
    {
        StopPlaybackInternal(resetPosition: false);

        if (_playbackTimer is not null)
        {
            _playbackTimer.Stop();
            _playbackTimer.Tick -= PlaybackTimer_Tick;
            _playbackTimer.Dispose();
            _playbackTimer = null;
        }

        if (_mediaPlayer is not null)
        {
            _mediaPlayer.EndReached -= MediaPlayer_EndReached;
            _mediaPlayer.Dispose();
            _mediaPlayer = null;
        }

        _currentMedia?.Dispose();
        _currentMedia = null;

        _libVlc?.Dispose();
        _libVlc = null;
    }

    private void SetTrackBarValueSafely(int value)
    {
        int clampedValue = Math.Clamp(value, playbackTrackBar.Minimum, playbackTrackBar.Maximum);
        bool previousSeeking = _isSeeking;
        _isSeeking = true;
        if (playbackTrackBar.Value != clampedValue)
        {
            playbackTrackBar.Value = clampedValue;
        }
        _isSeeking = previousSeeking;
    }
}
