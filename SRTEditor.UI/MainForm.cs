using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MadAngelFilms.SrtEditor.Controllers;
using MadAngelFilms.SrtEditor.Core.Models;
using MadAngelFilms.SrtEditor.UI.Extensions;
using MaterialSkin;
using MaterialSkin.Controls;

namespace MadAngelFilms.SrtEditor.UI;

public partial class MainForm : MaterialForm
{
    private readonly MainFormController _controller;
    private SubtitleProject? _currentProject;
    private CancellationTokenSource? _loadingCancellation;

    public MainForm(MainFormController controller)
    {
        _controller = controller;
        InitializeComponent();
        ConfigureMaterialSkin();
        ConfigureControls();
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
        subtitleListView.MultiSelect = false;
        subtitleListView.AccessibleName = "Subtitle entries";
        subtitleTextBox.AccessibleName = "Subtitle text";
        subtitleTextBox.Text = string.Empty;
        subtitleTimingLabel.Text = "Start: --:--:--.---  End: --:--:--.---";
        videoFileLabel.Text = "Video Preview";
        videoPlaceholderLabel.Text = "Video playback preview will appear here.";
        AcceptButton = null;
        CancelButton = null;
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
        _currentProject = project;
        subtitleListView.BeginUpdate();
        subtitleListView.Items.Clear();

        foreach (SubtitleEntry entry in project.Entries)
        {
            var item = new ListViewItem(entry.SequenceNumber.ToString(CultureInfo.InvariantCulture))
            {
                Tag = entry
            };
            item.SubItems.Add(entry.StartTime.ToString(@"hh\:mm\:ss\.fff", CultureInfo.InvariantCulture));
            item.SubItems.Add(entry.EndTime.ToString(@"hh\:mm\:ss\.fff", CultureInfo.InvariantCulture));
            item.SubItems.Add(entry.Text);
            subtitleListView.Items.Add(item);
        }

        subtitleListView.EndUpdate();
        subtitleListView.Focus();
        if (subtitleListView.Items.Count > 0)
        {
            subtitleListView.Items[0].Selected = true;
        }

        fileSaveMenuItem.Enabled = true;
        playbackTrackBar.Enabled = true;
        playButton.Enabled = true;
        pauseButton.Enabled = true;
        stopButton.Enabled = true;
        videoFileLabel.Text = $"Video Preview – {Path.GetFileName(project.VideoFilePath)}";
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
        playButton.Enabled = !isLoading && _currentProject is not null;
        pauseButton.Enabled = playButton.Enabled;
        stopButton.Enabled = playButton.Enabled;
        playbackTrackBar.Enabled = playButton.Enabled;
    }

    private void SubtitleListView_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (subtitleListView.SelectedItems.Count == 0)
        {
            ClearSubtitleDetails();
            return;
        }

        SubtitleEntry? entry = subtitleListView.SelectedItems[0].Tag as SubtitleEntry;
        if (entry is null)
        {
            ClearSubtitleDetails();
            return;
        }

        string startText = entry.StartTime.ToString(@"hh\:mm\:ss\.fff", CultureInfo.InvariantCulture);
        string endText = entry.EndTime.ToString(@"hh\:mm\:ss\.fff", CultureInfo.InvariantCulture);
        subtitleTimingLabel.Text = $"Start: {startText}  End: {endText}";
        subtitleTextBox.Text = entry.Text;
        videoPlaceholderLabel.Text =
            $"Playing video from {startText} to {endText}.\n(Playback integration coming soon.)";
    }

    private void ClearSubtitleDetails()
    {
        subtitleTimingLabel.Text = "Start: --:--:--.---  End: --:--:--.---";
        subtitleTextBox.Text = string.Empty;
        videoPlaceholderLabel.Text = "Video playback preview will appear here.";
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
}
