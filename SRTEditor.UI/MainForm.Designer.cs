using System.Drawing;
using System.Windows.Forms;
using MaterialSkin.Controls;
using LibVLCSharp.WinForms;

namespace MadAngelFilms.SrtEditor.UI;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            DisposePlaybackResources();
            components?.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        mainMenuStrip = new MenuStrip();
        fileMenuItem = new ToolStripMenuItem();
        fileNewMenuItem = new ToolStripMenuItem();
        fileOpenMenuItem = new ToolStripMenuItem();
        fileSaveMenuItem = new ToolStripMenuItem();
        fileExitMenuItem = new ToolStripMenuItem();
        editMenuItem = new ToolStripMenuItem();
        editUndoMenuItem = new ToolStripMenuItem();
        editRedoMenuItem = new ToolStripMenuItem();
        editCutMenuItem = new ToolStripMenuItem();
        editCopyMenuItem = new ToolStripMenuItem();
        editPasteMenuItem = new ToolStripMenuItem();
        editDeleteMenuItem = new ToolStripMenuItem();
        viewMenuItem = new ToolStripMenuItem();
        viewTogglePanelsMenuItem = new ToolStripMenuItem();
        viewThemesMenuItem = new ToolStripMenuItem();
        viewRefreshMenuItem = new ToolStripMenuItem();
        toolsMenuItem = new ToolStripMenuItem();
        toolsOptionsMenuItem = new ToolStripMenuItem();
        toolsSettingsMenuItem = new ToolStripMenuItem();
        toolsPreferencesMenuItem = new ToolStripMenuItem();
        helpMenuItem = new ToolStripMenuItem();
        helpDocumentationMenuItem = new ToolStripMenuItem();
        helpCheckForUpdatesMenuItem = new ToolStripMenuItem();
        helpAboutMenuItem = new ToolStripMenuItem();
        mainSplitContainer = new SplitContainer();
        leftPanelLayout = new TableLayoutPanel();
        primaryActionsLayout = new FlowLayoutPanel();
        drawerNewButton = new MaterialButton();
        drawerOpenButton = new MaterialButton();
        drawerImportButton = new MaterialButton();
        drawerExportButton = new MaterialButton();
        drawerTutorialButton = new MaterialButton();
        mainToolTip = new ToolTip(components);
        subtitleHeaderLabel = new MaterialLabel();
        subtitleActionsLayout = new FlowLayoutPanel();
        combineSelectedButton = new MaterialButton();
        subtitleListView = new MaterialListView();
        rightPanelLayout = new TableLayoutPanel();
        videoFileLabel = new MaterialLabel();
        videoPanel = new Panel();
        videoView = new VideoView();
        videoPlaceholderLabel = new Label();
        playbackTrackBar = new TrackBar();
        playbackControlsLayout = new FlowLayoutPanel();
        frameBackButton = new MaterialButton();
        playButton = new MaterialButton();
        pauseButton = new MaterialButton();
        stopButton = new MaterialButton();
        frameForwardButton = new MaterialButton();
        playbackPositionLabel = new MaterialLabel();
        subtitleTimingLabel = new MaterialLabel();
        subtitleTextBox = new MaterialMultiLineTextBox2();
        mainMenuStrip.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)mainSplitContainer).BeginInit();
        mainSplitContainer.Panel1.SuspendLayout();
        mainSplitContainer.Panel2.SuspendLayout();
        mainSplitContainer.SuspendLayout();
        leftPanelLayout.SuspendLayout();
        subtitleActionsLayout.SuspendLayout();
        rightPanelLayout.SuspendLayout();
        videoPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)videoView).BeginInit();
        ((System.ComponentModel.ISupportInitialize)playbackTrackBar).BeginInit();
        playbackControlsLayout.SuspendLayout();
        SuspendLayout();
        //
        // mainMenuStrip
        //
        mainMenuStrip.AccessibleName = "Main menu";
        mainMenuStrip.Dock = DockStyle.Top;
        mainMenuStrip.Items.AddRange(new ToolStripItem[] { fileMenuItem, editMenuItem, viewMenuItem, toolsMenuItem, helpMenuItem });
        mainMenuStrip.Location = new Point(3, 64);
        mainMenuStrip.Margin = new Padding(0);
        mainMenuStrip.Name = "mainMenuStrip";
        mainMenuStrip.Padding = new Padding(8, 3, 0, 3);
        mainMenuStrip.Size = new Size(1494, 35);
        mainMenuStrip.TabIndex = 0;
        mainMenuStrip.Text = "Main menu";
        // 
        // fileMenuItem
        // 
        fileMenuItem.AccessibleName = "File menu";
        fileMenuItem.DropDownItems.AddRange(new ToolStripItem[] { fileNewMenuItem, fileOpenMenuItem, fileSaveMenuItem, new ToolStripSeparator(), fileExitMenuItem });
        fileMenuItem.Name = "fileMenuItem";
        fileMenuItem.Size = new Size(62, 29);
        fileMenuItem.Text = "&File";
        // 
        // fileNewMenuItem
        // 
        fileNewMenuItem.Name = "fileNewMenuItem";
        fileNewMenuItem.ShortcutKeys = Keys.Control | Keys.N;
        fileNewMenuItem.Size = new Size(270, 34);
        fileNewMenuItem.Text = "&New";
        // 
        // fileOpenMenuItem
        // 
        fileOpenMenuItem.Name = "fileOpenMenuItem";
        fileOpenMenuItem.ShortcutKeys = Keys.Control | Keys.O;
        fileOpenMenuItem.Size = new Size(270, 34);
        fileOpenMenuItem.Text = "&Open…";
        fileOpenMenuItem.Click += FileOpenMenuItem_Click;
        // 
        // fileSaveMenuItem
        // 
        fileSaveMenuItem.Enabled = false;
        fileSaveMenuItem.Name = "fileSaveMenuItem";
        fileSaveMenuItem.ShortcutKeys = Keys.Control | Keys.S;
        fileSaveMenuItem.Size = new Size(270, 34);
        fileSaveMenuItem.Text = "&Save";
        // 
        // fileExitMenuItem
        // 
        fileExitMenuItem.Name = "fileExitMenuItem";
        fileExitMenuItem.ShortcutKeys = Keys.Control | Keys.Q;
        fileExitMenuItem.Size = new Size(270, 34);
        fileExitMenuItem.Text = "E&xit";
        fileExitMenuItem.Click += FileExitMenuItem_Click;
        // 
        // editMenuItem
        // 
        editMenuItem.AccessibleName = "Edit menu";
        editMenuItem.DropDownItems.AddRange(new ToolStripItem[] { editUndoMenuItem, editRedoMenuItem, new ToolStripSeparator(), editCutMenuItem, editCopyMenuItem, editPasteMenuItem, editDeleteMenuItem });
        editMenuItem.Name = "editMenuItem";
        editMenuItem.Size = new Size(65, 29);
        editMenuItem.Text = "&Edit";
        // 
        // editUndoMenuItem
        // 
        editUndoMenuItem.Enabled = false;
        editUndoMenuItem.Name = "editUndoMenuItem";
        editUndoMenuItem.ShortcutKeys = Keys.Control | Keys.Z;
        editUndoMenuItem.Size = new Size(238, 34);
        editUndoMenuItem.Text = "&Undo";
        // 
        // editRedoMenuItem
        // 
        editRedoMenuItem.Enabled = false;
        editRedoMenuItem.Name = "editRedoMenuItem";
        editRedoMenuItem.ShortcutKeys = Keys.Control | Keys.Y;
        editRedoMenuItem.Size = new Size(238, 34);
        editRedoMenuItem.Text = "&Redo";
        // 
        // editCutMenuItem
        // 
        editCutMenuItem.Enabled = false;
        editCutMenuItem.Name = "editCutMenuItem";
        editCutMenuItem.ShortcutKeys = Keys.Control | Keys.X;
        editCutMenuItem.Size = new Size(238, 34);
        editCutMenuItem.Text = "Cu&t";
        // 
        // editCopyMenuItem
        // 
        editCopyMenuItem.Enabled = false;
        editCopyMenuItem.Name = "editCopyMenuItem";
        editCopyMenuItem.ShortcutKeys = Keys.Control | Keys.C;
        editCopyMenuItem.Size = new Size(238, 34);
        editCopyMenuItem.Text = "&Copy";
        // 
        // editPasteMenuItem
        // 
        editPasteMenuItem.Enabled = false;
        editPasteMenuItem.Name = "editPasteMenuItem";
        editPasteMenuItem.ShortcutKeys = Keys.Control | Keys.V;
        editPasteMenuItem.Size = new Size(238, 34);
        editPasteMenuItem.Text = "&Paste";
        // 
        // editDeleteMenuItem
        // 
        editDeleteMenuItem.Enabled = false;
        editDeleteMenuItem.Name = "editDeleteMenuItem";
        editDeleteMenuItem.ShortcutKeys = Keys.Delete;
        editDeleteMenuItem.Size = new Size(238, 34);
        editDeleteMenuItem.Text = "&Delete";
        // 
        // viewMenuItem
        // 
        viewMenuItem.AccessibleName = "View menu";
        viewMenuItem.DropDownItems.AddRange(new ToolStripItem[] { viewTogglePanelsMenuItem, viewThemesMenuItem, viewRefreshMenuItem });
        viewMenuItem.Name = "viewMenuItem";
        viewMenuItem.Size = new Size(67, 29);
        viewMenuItem.Text = "&View";
        // 
        // viewTogglePanelsMenuItem
        // 
        viewTogglePanelsMenuItem.Enabled = false;
        viewTogglePanelsMenuItem.Name = "viewTogglePanelsMenuItem";
        viewTogglePanelsMenuItem.Size = new Size(232, 34);
        viewTogglePanelsMenuItem.Text = "&Toggle Panels";
        // 
        // viewThemesMenuItem
        // 
        viewThemesMenuItem.Enabled = false;
        viewThemesMenuItem.Name = "viewThemesMenuItem";
        viewThemesMenuItem.Size = new Size(232, 34);
        viewThemesMenuItem.Text = "&Themes";
        // 
        // viewRefreshMenuItem
        // 
        viewRefreshMenuItem.Enabled = false;
        viewRefreshMenuItem.Name = "viewRefreshMenuItem";
        viewRefreshMenuItem.Size = new Size(232, 34);
        viewRefreshMenuItem.Text = "&Refresh";
        // 
        // toolsMenuItem
        // 
        toolsMenuItem.AccessibleName = "Tools menu";
        toolsMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolsOptionsMenuItem, toolsSettingsMenuItem, toolsPreferencesMenuItem });
        toolsMenuItem.Name = "toolsMenuItem";
        toolsMenuItem.Size = new Size(74, 29);
        toolsMenuItem.Text = "&Tools";
        // 
        // toolsOptionsMenuItem
        // 
        toolsOptionsMenuItem.Enabled = false;
        toolsOptionsMenuItem.Name = "toolsOptionsMenuItem";
        toolsOptionsMenuItem.Size = new Size(219, 34);
        toolsOptionsMenuItem.Text = "&Options";
        // 
        // toolsSettingsMenuItem
        // 
        toolsSettingsMenuItem.Enabled = false;
        toolsSettingsMenuItem.Name = "toolsSettingsMenuItem";
        toolsSettingsMenuItem.Size = new Size(219, 34);
        toolsSettingsMenuItem.Text = "&Settings";
        // 
        // toolsPreferencesMenuItem
        // 
        toolsPreferencesMenuItem.Enabled = false;
        toolsPreferencesMenuItem.Name = "toolsPreferencesMenuItem";
        toolsPreferencesMenuItem.Size = new Size(219, 34);
        toolsPreferencesMenuItem.Text = "&Preferences";
        // 
        // helpMenuItem
        // 
        helpMenuItem.AccessibleName = "Help menu";
        helpMenuItem.DropDownItems.AddRange(new ToolStripItem[] { helpDocumentationMenuItem, helpCheckForUpdatesMenuItem, helpAboutMenuItem });
        helpMenuItem.Name = "helpMenuItem";
        helpMenuItem.ShortcutKeys = Keys.F1;
        helpMenuItem.Size = new Size(68, 29);
        helpMenuItem.Text = "&Help";
        // 
        // helpDocumentationMenuItem
        // 
        helpDocumentationMenuItem.Enabled = false;
        helpDocumentationMenuItem.Name = "helpDocumentationMenuItem";
        helpDocumentationMenuItem.Size = new Size(295, 34);
        helpDocumentationMenuItem.Text = "&Documentation";
        // 
        // helpCheckForUpdatesMenuItem
        // 
        helpCheckForUpdatesMenuItem.Enabled = false;
        helpCheckForUpdatesMenuItem.Name = "helpCheckForUpdatesMenuItem";
        helpCheckForUpdatesMenuItem.Size = new Size(295, 34);
        helpCheckForUpdatesMenuItem.Text = "Check for &Updates";
        // 
        // helpAboutMenuItem
        // 
        helpAboutMenuItem.Name = "helpAboutMenuItem";
        helpAboutMenuItem.Size = new Size(295, 34);
        helpAboutMenuItem.Text = "&About";
        helpAboutMenuItem.Click += HelpAboutMenuItem_Click;
        // 
        // mainSplitContainer
        // 
        mainSplitContainer.Dock = DockStyle.Fill;
        mainSplitContainer.Location = new Point(3, 99);
        mainSplitContainer.Name = "mainSplitContainer";
        // 
        // mainSplitContainer.Panel1
        // 
        mainSplitContainer.Panel1.Controls.Add(leftPanelLayout);
        mainSplitContainer.Panel1MinSize = 350;
        // 
        // mainSplitContainer.Panel2
        // 
        mainSplitContainer.Panel2.Controls.Add(rightPanelLayout);
        mainSplitContainer.Panel2MinSize = 450;
        mainSplitContainer.Size = new Size(1494, 754);
        mainSplitContainer.SplitterDistance = 500;
        mainSplitContainer.TabIndex = 1;
        // 
        // leftPanelLayout
        // 
        leftPanelLayout.ColumnCount = 1;
        leftPanelLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        leftPanelLayout.Controls.Add(primaryActionsLayout, 0, 0);
        leftPanelLayout.Controls.Add(subtitleHeaderLabel, 0, 1);
        leftPanelLayout.Controls.Add(subtitleActionsLayout, 0, 2);
        leftPanelLayout.Controls.Add(subtitleListView, 0, 3);
        leftPanelLayout.Dock = DockStyle.Fill;
        leftPanelLayout.Location = new Point(0, 0);
        leftPanelLayout.Margin = new Padding(4);
        leftPanelLayout.Name = "leftPanelLayout";
        leftPanelLayout.RowCount = 4;
        leftPanelLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        leftPanelLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        leftPanelLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        leftPanelLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        leftPanelLayout.Size = new Size(500, 754);
        leftPanelLayout.TabIndex = 0;
        //
        // primaryActionsLayout
        //
        primaryActionsLayout.AutoSize = true;
        primaryActionsLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        primaryActionsLayout.Controls.Add(drawerNewButton);
        primaryActionsLayout.Controls.Add(drawerOpenButton);
        primaryActionsLayout.Controls.Add(drawerImportButton);
        primaryActionsLayout.Controls.Add(drawerExportButton);
        primaryActionsLayout.Controls.Add(drawerTutorialButton);
        primaryActionsLayout.Dock = DockStyle.Fill;
        primaryActionsLayout.FlowDirection = FlowDirection.TopDown;
        primaryActionsLayout.Location = new Point(4, 4);
        primaryActionsLayout.Margin = new Padding(4, 4, 4, 12);
        primaryActionsLayout.Name = "primaryActionsLayout";
        primaryActionsLayout.Padding = new Padding(0, 4, 0, 4);
        primaryActionsLayout.Size = new Size(492, 156);
        primaryActionsLayout.TabIndex = 0;
        primaryActionsLayout.WrapContents = false;
        //
        // drawerNewButton
        //
        drawerNewButton.AutoSize = false;
        drawerNewButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        drawerNewButton.Density = MaterialButton.MaterialButtonDensity.Default;
        drawerNewButton.Depth = 0;
        drawerNewButton.Enabled = false;
        drawerNewButton.HighEmphasis = true;
        drawerNewButton.Icon = null;
        drawerNewButton.Margin = new Padding(0, 0, 0, 8);
        drawerNewButton.Name = "drawerNewButton";
        drawerNewButton.NoAccentTextColor = Color.Empty;
        drawerNewButton.Size = new Size(220, 36);
        drawerNewButton.TabIndex = 0;
        drawerNewButton.Text = "New Project";
        drawerNewButton.Type = MaterialButton.MaterialButtonType.Contained;
        drawerNewButton.UseAccentColor = true;
        drawerNewButton.UseVisualStyleBackColor = true;
        mainToolTip.SetToolTip(drawerNewButton, "Create a new subtitle project");
        //
        // drawerOpenButton
        //
        drawerOpenButton.AutoSize = false;
        drawerOpenButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        drawerOpenButton.Density = MaterialButton.MaterialButtonDensity.Default;
        drawerOpenButton.Depth = 0;
        drawerOpenButton.HighEmphasis = true;
        drawerOpenButton.Icon = null;
        drawerOpenButton.Margin = new Padding(0, 0, 0, 8);
        drawerOpenButton.Name = "drawerOpenButton";
        drawerOpenButton.NoAccentTextColor = Color.Empty;
        drawerOpenButton.Size = new Size(220, 36);
        drawerOpenButton.TabIndex = 1;
        drawerOpenButton.Text = "Open Project";
        drawerOpenButton.Type = MaterialButton.MaterialButtonType.Contained;
        drawerOpenButton.UseAccentColor = false;
        drawerOpenButton.UseVisualStyleBackColor = true;
        drawerOpenButton.Click += FileOpenMenuItem_Click;
        mainToolTip.SetToolTip(drawerOpenButton, "Open an existing project");
        //
        // drawerImportButton
        //
        drawerImportButton.AutoSize = false;
        drawerImportButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        drawerImportButton.Density = MaterialButton.MaterialButtonDensity.Default;
        drawerImportButton.Depth = 0;
        drawerImportButton.Enabled = false;
        drawerImportButton.HighEmphasis = true;
        drawerImportButton.Icon = null;
        drawerImportButton.Margin = new Padding(0, 0, 0, 8);
        drawerImportButton.Name = "drawerImportButton";
        drawerImportButton.NoAccentTextColor = Color.Empty;
        drawerImportButton.Size = new Size(220, 36);
        drawerImportButton.TabIndex = 2;
        drawerImportButton.Text = "Import Subtitles";
        drawerImportButton.Type = MaterialButton.MaterialButtonType.Contained;
        drawerImportButton.UseAccentColor = false;
        drawerImportButton.UseVisualStyleBackColor = true;
        mainToolTip.SetToolTip(drawerImportButton, "Import additional subtitle tracks");
        //
        // drawerExportButton
        //
        drawerExportButton.AutoSize = false;
        drawerExportButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        drawerExportButton.Density = MaterialButton.MaterialButtonDensity.Default;
        drawerExportButton.Depth = 0;
        drawerExportButton.Enabled = false;
        drawerExportButton.HighEmphasis = true;
        drawerExportButton.Icon = null;
        drawerExportButton.Margin = new Padding(0, 0, 0, 8);
        drawerExportButton.Name = "drawerExportButton";
        drawerExportButton.NoAccentTextColor = Color.Empty;
        drawerExportButton.Size = new Size(220, 36);
        drawerExportButton.TabIndex = 3;
        drawerExportButton.Text = "Export";
        drawerExportButton.Type = MaterialButton.MaterialButtonType.Contained;
        drawerExportButton.UseAccentColor = false;
        drawerExportButton.UseVisualStyleBackColor = true;
        mainToolTip.SetToolTip(drawerExportButton, "Export using presets");
        //
        // drawerTutorialButton
        //
        drawerTutorialButton.AutoSize = false;
        drawerTutorialButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        drawerTutorialButton.Density = MaterialButton.MaterialButtonDensity.Default;
        drawerTutorialButton.Depth = 0;
        drawerTutorialButton.HighEmphasis = false;
        drawerTutorialButton.Icon = null;
        drawerTutorialButton.Margin = new Padding(0, 0, 0, 8);
        drawerTutorialButton.Name = "drawerTutorialButton";
        drawerTutorialButton.NoAccentTextColor = Color.Empty;
        drawerTutorialButton.Size = new Size(220, 36);
        drawerTutorialButton.TabIndex = 4;
        drawerTutorialButton.Text = "Tutorial";
        drawerTutorialButton.Type = MaterialButton.MaterialButtonType.Text;
        drawerTutorialButton.UseAccentColor = false;
        drawerTutorialButton.UseVisualStyleBackColor = true;
        mainToolTip.SetToolTip(drawerTutorialButton, "Launch guided onboarding");
        // 
        // subtitleHeaderLabel
        // 
        subtitleHeaderLabel.AccessibleName = "Subtitle list label";
        subtitleHeaderLabel.AutoSize = true;
        subtitleHeaderLabel.Depth = 0;
        subtitleHeaderLabel.Dock = DockStyle.Fill;
        subtitleHeaderLabel.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
        subtitleHeaderLabel.Location = new Point(4, 0);
        subtitleHeaderLabel.Margin = new Padding(4, 0, 4, 8);
        subtitleHeaderLabel.MouseState = MaterialSkin.MouseState.HOVER;
        subtitleHeaderLabel.Name = "subtitleHeaderLabel";
        subtitleHeaderLabel.Padding = new Padding(0, 8, 0, 0);
        subtitleHeaderLabel.Size = new Size(492, 32);
        subtitleHeaderLabel.TabIndex = 0;
        subtitleHeaderLabel.Text = "Subtitle Entries";
        // 
        // subtitleListView
        // 
        subtitleListView.AutoSizeTable = false;
        subtitleListView.BackColor = Color.FromArgb(26, 26, 26);
        subtitleListView.BorderStyle = BorderStyle.None;
        subtitleListView.Columns.AddRange(new ColumnHeader[] { sequenceColumnHeader, startColumnHeader, endColumnHeader, textColumnHeader, actionColumnHeader });
        subtitleListView.Depth = 0;
        subtitleListView.Dock = DockStyle.Fill;
        subtitleListView.FullRowSelect = true;
        subtitleListView.HideSelection = false;
        subtitleListView.ForeColor = Color.FromArgb(242, 242, 240);
        subtitleListView.Location = new Point(4, 80);
        subtitleListView.Margin = new Padding(4);
        subtitleListView.MinimumSize = new Size(200, 200);
        subtitleListView.MouseLocation = new Point(-1, -1);
        subtitleListView.MouseState = MaterialSkin.MouseState.OUT;
        subtitleListView.Name = "subtitleListView";
        subtitleListView.OwnerDraw = true;
        subtitleListView.Size = new Size(492, 670);
        subtitleListView.TabIndex = 2;
        subtitleListView.UseCompatibleStateImageBehavior = false;
        subtitleListView.View = View.Details;
        subtitleListView.SelectedIndexChanged += SubtitleListView_SelectedIndexChanged;
        // 
        // sequenceColumnHeader
        // 
        sequenceColumnHeader.Text = "#";
        sequenceColumnHeader.Width = 50;
        // 
        // startColumnHeader
        // 
        startColumnHeader.Text = "Start";
        startColumnHeader.Width = 110;
        // 
        // endColumnHeader
        // 
        endColumnHeader.Text = "End";
        endColumnHeader.Width = 110;
        // 
        // textColumnHeader
        // 
        textColumnHeader.Text = "Text";
        textColumnHeader.Width = 220;
        //
        // actionColumnHeader
        //
        actionColumnHeader.Text = "Actions";
        actionColumnHeader.Width = 90;
        //
        // subtitleActionsLayout
        //
        subtitleActionsLayout.AutoSize = true;
        subtitleActionsLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        subtitleActionsLayout.Controls.Add(combineSelectedButton);
        subtitleActionsLayout.Dock = DockStyle.Fill;
        subtitleActionsLayout.FlowDirection = FlowDirection.LeftToRight;
        subtitleActionsLayout.Location = new Point(4, 40);
        subtitleActionsLayout.Margin = new Padding(4, 0, 4, 4);
        subtitleActionsLayout.Name = "subtitleActionsLayout";
        subtitleActionsLayout.Padding = new Padding(0, 0, 0, 4);
        subtitleActionsLayout.Size = new Size(492, 36);
        subtitleActionsLayout.TabIndex = 1;
        subtitleActionsLayout.WrapContents = false;
        //
        // combineSelectedButton
        //
        combineSelectedButton.AutoSize = false;
        combineSelectedButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        combineSelectedButton.Density = MaterialButton.MaterialButtonDensity.Default;
        combineSelectedButton.Depth = 0;
        combineSelectedButton.Enabled = false;
        combineSelectedButton.HighEmphasis = true;
        combineSelectedButton.Icon = null;
        combineSelectedButton.Margin = new Padding(0, 4, 0, 4);
        combineSelectedButton.Name = "combineSelectedButton";
        combineSelectedButton.NoAccentTextColor = Color.Empty;
        combineSelectedButton.Size = new Size(220, 28);
        combineSelectedButton.TabIndex = 0;
        combineSelectedButton.Text = "Combine Selected";
        combineSelectedButton.Type = MaterialButton.MaterialButtonType.Contained;
        combineSelectedButton.UseAccentColor = false;
        combineSelectedButton.UseVisualStyleBackColor = true;
        // 
        // rightPanelLayout
        // 
        rightPanelLayout.ColumnCount = 1;
        rightPanelLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        rightPanelLayout.Controls.Add(videoFileLabel, 0, 0);
        rightPanelLayout.Controls.Add(videoPanel, 0, 1);
        rightPanelLayout.Controls.Add(playbackTrackBar, 0, 2);
        rightPanelLayout.Controls.Add(playbackControlsLayout, 0, 3);
        rightPanelLayout.Controls.Add(subtitleTimingLabel, 0, 4);
        rightPanelLayout.Controls.Add(subtitleTextBox, 0, 5);
        rightPanelLayout.Dock = DockStyle.Fill;
        rightPanelLayout.Location = new Point(0, 0);
        rightPanelLayout.Name = "rightPanelLayout";
        rightPanelLayout.RowCount = 6;
        rightPanelLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rightPanelLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
        rightPanelLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rightPanelLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rightPanelLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        rightPanelLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
        rightPanelLayout.Size = new Size(990, 754);
        rightPanelLayout.TabIndex = 0;
        // 
        // videoFileLabel
        // 
        videoFileLabel.AutoSize = true;
        videoFileLabel.Depth = 0;
        videoFileLabel.Dock = DockStyle.Fill;
        videoFileLabel.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
        videoFileLabel.Location = new Point(4, 0);
        videoFileLabel.Margin = new Padding(4, 0, 4, 8);
        videoFileLabel.MouseState = MaterialSkin.MouseState.HOVER;
        videoFileLabel.Name = "videoFileLabel";
        videoFileLabel.Padding = new Padding(0, 8, 0, 0);
        videoFileLabel.Size = new Size(982, 32);
        videoFileLabel.TabIndex = 0;
        videoFileLabel.Text = "Video Preview";
        // 
        // videoPanel
        // 
        videoPanel.BackColor = Color.FromArgb(11, 29, 58);
        videoPanel.Controls.Add(videoView);
        videoPanel.Controls.Add(videoPlaceholderLabel);
        videoPanel.Dock = DockStyle.Fill;
        videoPanel.Location = new Point(4, 40);
        videoPanel.Margin = new Padding(4);
        videoPanel.Name = "videoPanel";
        videoPanel.Size = new Size(982, 426);
        videoPanel.TabIndex = 1;
        //
        // videoView
        //
        videoView.BackColor = Color.Black;
        videoView.Dock = DockStyle.Fill;
        videoView.Location = new Point(0, 0);
        videoView.MediaPlayer = null;
        videoView.Name = "videoView";
        videoView.Size = new Size(982, 426);
        videoView.TabIndex = 1;
        videoView.Text = "videoView";
        videoView.Visible = false;
        //
        // videoPlaceholderLabel
        //
        videoPlaceholderLabel.Dock = DockStyle.Fill;
        videoPlaceholderLabel.Font = new Font("Segoe UI", 12F, FontStyle.Italic, GraphicsUnit.Point);
        videoPlaceholderLabel.ForeColor = Color.FromArgb(242, 242, 240);
        videoPlaceholderLabel.Location = new Point(0, 0);
        videoPlaceholderLabel.Name = "videoPlaceholderLabel";
        videoPlaceholderLabel.Size = new Size(982, 426);
        videoPlaceholderLabel.TabIndex = 0;
        videoPlaceholderLabel.Text = "Video playback preview will appear here.";
        videoPlaceholderLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // playbackTrackBar
        // 
        playbackTrackBar.Dock = DockStyle.Top;
        playbackTrackBar.Enabled = false;
        playbackTrackBar.Location = new Point(4, 474);
        playbackTrackBar.Margin = new Padding(4, 4, 4, 0);
        playbackTrackBar.Name = "playbackTrackBar";
        playbackTrackBar.Size = new Size(982, 69);
        playbackTrackBar.TabIndex = 2;
        // 
        // playbackControlsLayout
        // 
        playbackControlsLayout.AutoSize = true;
        playbackControlsLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        playbackControlsLayout.Controls.Add(frameBackButton);
        playbackControlsLayout.Controls.Add(playButton);
        playbackControlsLayout.Controls.Add(pauseButton);
        playbackControlsLayout.Controls.Add(stopButton);
        playbackControlsLayout.Controls.Add(frameForwardButton);
        playbackControlsLayout.Controls.Add(playbackPositionLabel);
        playbackControlsLayout.Dock = DockStyle.Fill;
        playbackControlsLayout.Location = new Point(4, 543);
        playbackControlsLayout.Margin = new Padding(4, 0, 4, 0);
        playbackControlsLayout.Name = "playbackControlsLayout";
        playbackControlsLayout.Padding = new Padding(0, 8, 0, 8);
        playbackControlsLayout.Size = new Size(982, 64);
        playbackControlsLayout.TabIndex = 3;
        // 
        // frameBackButton
        //
        frameBackButton.AutoSize = false;
        frameBackButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        frameBackButton.Density = MaterialButton.MaterialButtonDensity.Default;
        frameBackButton.Depth = 0;
        frameBackButton.Enabled = false;
        frameBackButton.HighEmphasis = true;
        frameBackButton.Icon = null;
        frameBackButton.Margin = new Padding(0, 8, 12, 8);
        frameBackButton.Name = "frameBackButton";
        frameBackButton.NoAccentTextColor = Color.Empty;
        frameBackButton.Size = new Size(120, 36);
        frameBackButton.TabIndex = 0;
        frameBackButton.Text = "Frame -1";
        frameBackButton.Type = MaterialButton.MaterialButtonType.Contained;
        frameBackButton.UseAccentColor = false;
        frameBackButton.UseVisualStyleBackColor = true;
        //
        // playButton
        //
        playButton.AutoSize = false;
        playButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        playButton.Density = MaterialButton.MaterialButtonDensity.Default;
        playButton.Depth = 0;
        playButton.Enabled = false;
        playButton.HighEmphasis = true;
        playButton.Icon = null;
        playButton.Margin = new Padding(0, 8, 12, 8);
        playButton.Name = "playButton";
        playButton.NoAccentTextColor = Color.Empty;
        playButton.Size = new Size(120, 36);
        playButton.TabIndex = 1;
        playButton.Text = "Play";
        playButton.Type = MaterialButton.MaterialButtonType.Contained;
        playButton.UseAccentColor = false;
        playButton.UseVisualStyleBackColor = true;
        // 
        // pauseButton
        // 
        pauseButton.AutoSize = false;
        pauseButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        pauseButton.Density = MaterialButton.MaterialButtonDensity.Default;
        pauseButton.Depth = 0;
        pauseButton.Enabled = false;
        pauseButton.HighEmphasis = true;
        pauseButton.Icon = null;
        pauseButton.Margin = new Padding(0, 8, 12, 8);
        pauseButton.Name = "pauseButton";
        pauseButton.NoAccentTextColor = Color.Empty;
        pauseButton.Size = new Size(120, 36);
        pauseButton.TabIndex = 2;
        pauseButton.Text = "Pause";
        pauseButton.Type = MaterialButton.MaterialButtonType.Contained;
        pauseButton.UseAccentColor = false;
        pauseButton.UseVisualStyleBackColor = true;
        // 
        // stopButton
        // 
        stopButton.AutoSize = false;
        stopButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        stopButton.Density = MaterialButton.MaterialButtonDensity.Default;
        stopButton.Depth = 0;
        stopButton.Enabled = false;
        stopButton.HighEmphasis = true;
        stopButton.Icon = null;
        stopButton.Margin = new Padding(0, 8, 12, 8);
        stopButton.Name = "stopButton";
        stopButton.NoAccentTextColor = Color.Empty;
        stopButton.Size = new Size(120, 36);
        stopButton.TabIndex = 3;
        stopButton.Text = "Stop";
        stopButton.Type = MaterialButton.MaterialButtonType.Contained;
        stopButton.UseAccentColor = false;
        stopButton.UseVisualStyleBackColor = true;
        //
        // frameForwardButton
        //
        frameForwardButton.AutoSize = false;
        frameForwardButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        frameForwardButton.Density = MaterialButton.MaterialButtonDensity.Default;
        frameForwardButton.Depth = 0;
        frameForwardButton.Enabled = false;
        frameForwardButton.HighEmphasis = true;
        frameForwardButton.Icon = null;
        frameForwardButton.Margin = new Padding(0, 8, 12, 8);
        frameForwardButton.Name = "frameForwardButton";
        frameForwardButton.NoAccentTextColor = Color.Empty;
        frameForwardButton.Size = new Size(120, 36);
        frameForwardButton.TabIndex = 4;
        frameForwardButton.Text = "Frame +1";
        frameForwardButton.Type = MaterialButton.MaterialButtonType.Contained;
        frameForwardButton.UseAccentColor = false;
        frameForwardButton.UseVisualStyleBackColor = true;
        //
        // playbackPositionLabel
        //
        playbackPositionLabel.AutoSize = true;
        playbackPositionLabel.Depth = 0;
        playbackPositionLabel.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
        playbackPositionLabel.Location = new Point(492, 8);
        playbackPositionLabel.Margin = new Padding(0, 8, 0, 0);
        playbackPositionLabel.MouseState = MaterialSkin.MouseState.HOVER;
        playbackPositionLabel.Name = "playbackPositionLabel";
        playbackPositionLabel.Size = new Size(170, 19);
        playbackPositionLabel.TabIndex = 5;
        playbackPositionLabel.Text = "00:00:00.000 / --:--:--.--";
        // 
        // subtitleTimingLabel
        // 
        subtitleTimingLabel.AutoSize = true;
        subtitleTimingLabel.Depth = 0;
        subtitleTimingLabel.Dock = DockStyle.Fill;
        subtitleTimingLabel.Font = new Font("Roboto", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
        subtitleTimingLabel.Location = new Point(4, 607);
        subtitleTimingLabel.Margin = new Padding(4, 0, 4, 8);
        subtitleTimingLabel.MouseState = MaterialSkin.MouseState.HOVER;
        subtitleTimingLabel.Name = "subtitleTimingLabel";
        subtitleTimingLabel.Padding = new Padding(0, 8, 0, 0);
        subtitleTimingLabel.Size = new Size(982, 32);
        subtitleTimingLabel.TabIndex = 4;
        subtitleTimingLabel.Text = "Start: --:--:--.---  End: --:--:--.---";
        // 
        // subtitleTextBox
        // 
        subtitleTextBox.AnimateReadOnly = false;
        subtitleTextBox.BackColor = Color.FromArgb(242, 242, 240);
        subtitleTextBox.Depth = 0;
        subtitleTextBox.Dock = DockStyle.Fill;
        subtitleTextBox.Enabled = false;
        subtitleTextBox.Font = new Font("Roboto", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
        subtitleTextBox.HideSelection = true;
        subtitleTextBox.ForeColor = Color.FromArgb(26, 26, 26);
        subtitleTextBox.Location = new Point(4, 647);
        subtitleTextBox.Margin = new Padding(4);
        subtitleTextBox.MaxLength = 1500;
        subtitleTextBox.MouseState = MaterialSkin.MouseState.OUT;
        subtitleTextBox.Name = "subtitleTextBox";
        subtitleTextBox.Size = new Size(982, 103);
        subtitleTextBox.TabIndex = 5;
        subtitleTextBox.Text = string.Empty;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(1500, 856);
        Controls.Add(mainSplitContainer);
        Controls.Add(mainMenuStrip);
        MainMenuStrip = mainMenuStrip;
        MinimumSize = new Size(1200, 700);
        Name = "MainForm";
        Padding = new Padding(3, 64, 3, 3);
        Text = "Mad Angel Films – Subtitle Editor";
        mainMenuStrip.ResumeLayout(false);
        mainMenuStrip.PerformLayout();
        mainSplitContainer.Panel1.ResumeLayout(false);
        mainSplitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)mainSplitContainer).EndInit();
        mainSplitContainer.ResumeLayout(false);
        leftPanelLayout.ResumeLayout(false);
        leftPanelLayout.PerformLayout();
        rightPanelLayout.ResumeLayout(false);
        rightPanelLayout.PerformLayout();
        videoPanel.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)videoView).EndInit();
        ((System.ComponentModel.ISupportInitialize)playbackTrackBar).EndInit();
        subtitleActionsLayout.ResumeLayout(false);
        playbackControlsLayout.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private MenuStrip mainMenuStrip;
    private ToolStripMenuItem fileMenuItem;
    private ToolStripMenuItem fileNewMenuItem;
    private ToolStripMenuItem fileOpenMenuItem;
    private ToolStripMenuItem fileSaveMenuItem;
    private ToolStripMenuItem fileExitMenuItem;
    private ToolStripMenuItem editMenuItem;
    private ToolStripMenuItem editUndoMenuItem;
    private ToolStripMenuItem editRedoMenuItem;
    private ToolStripMenuItem editCutMenuItem;
    private ToolStripMenuItem editCopyMenuItem;
    private ToolStripMenuItem editPasteMenuItem;
    private ToolStripMenuItem editDeleteMenuItem;
    private ToolStripMenuItem viewMenuItem;
    private ToolStripMenuItem viewTogglePanelsMenuItem;
    private ToolStripMenuItem viewThemesMenuItem;
    private ToolStripMenuItem viewRefreshMenuItem;
    private ToolStripMenuItem toolsMenuItem;
    private ToolStripMenuItem toolsOptionsMenuItem;
    private ToolStripMenuItem toolsSettingsMenuItem;
    private ToolStripMenuItem toolsPreferencesMenuItem;
    private ToolStripMenuItem helpMenuItem;
    private ToolStripMenuItem helpDocumentationMenuItem;
    private ToolStripMenuItem helpCheckForUpdatesMenuItem;
    private ToolStripMenuItem helpAboutMenuItem;
    private SplitContainer mainSplitContainer;
    private TableLayoutPanel leftPanelLayout;
    private FlowLayoutPanel primaryActionsLayout;
    private MaterialButton drawerNewButton;
    private MaterialButton drawerOpenButton;
    private MaterialButton drawerImportButton;
    private MaterialButton drawerExportButton;
    private MaterialButton drawerTutorialButton;
    private MaterialLabel subtitleHeaderLabel;
    private FlowLayoutPanel subtitleActionsLayout;
    private MaterialListView subtitleListView;
    private ColumnHeader sequenceColumnHeader = new();
    private ColumnHeader startColumnHeader = new();
    private ColumnHeader endColumnHeader = new();
    private ColumnHeader textColumnHeader = new();
    private ColumnHeader actionColumnHeader = new();
    private MaterialButton combineSelectedButton;
    private TableLayoutPanel rightPanelLayout;
    private MaterialLabel videoFileLabel;
    private Panel videoPanel;
    private VideoView videoView;
    private Label videoPlaceholderLabel;
    private TrackBar playbackTrackBar;
    private FlowLayoutPanel playbackControlsLayout;
    private MaterialButton frameBackButton;
    private MaterialButton playButton;
    private MaterialButton pauseButton;
    private MaterialButton stopButton;
    private MaterialButton frameForwardButton;
    private MaterialLabel playbackPositionLabel;
    private MaterialLabel subtitleTimingLabel;
    private MaterialMultiLineTextBox2 subtitleTextBox;
    private ToolTip mainToolTip;
}
