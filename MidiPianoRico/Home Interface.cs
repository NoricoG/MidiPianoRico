using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Sanford.Multimedia.Midi.UI;
using System.Diagnostics;

/*
TODO:
    Open picture in folder
    Show note help
    Remember latest category and song
    Handle multiple pages
    
Maybe:
    Output with low latency witouth VMPK
    Open, draw and play midi file
*/

namespace MidiPianoRico
{
    partial class Home : Form
    {
        private HUIKeyboardHandler hUIKeyboardHandler;
        public PictureBox pictureBox;
        private Settings settings;

        private ToolStripComboBox folderComboBox, songComboBox, metronomeComboBox;
        private Label folderSwitchingLabel, exitPressedLabel;
        private ToolStripButton metronomeButton;

        private Bitmap[] pages;
        private int currentPage = 1;
        private bool folderSwitching = false;
        private bool exitPressed = false;
        private bool playerLaunched = false;
        private Timer metronomeTimer;
        
        public Home()
        {
            Text = "MidiPianoRico";
            hUIKeyboardHandler = new HUIKeyboardHandler(this, 1);
            settings = FileHandler.LoadSettings();
            Load += Home_Load;
            SetSize();
            AddControls();

            metronomeTimer = new Timer();
            metronomeTimer.Tick += MetronomeTimer_Tick;
        }



        private void SetSize()
        {
            Rectangle screenSize = Screen.GetWorkingArea(this);
            this.Size = new Size(screenSize.Width, screenSize.Height);
            WindowState = FormWindowState.Maximized;
        }

        private void AddControls()
        {
            ToolStrip toolStrip = new ToolStrip();
            Controls.Add(toolStrip);

            ToolStripLabel folderLabel = new ToolStripLabel();
            folderLabel.Text = "Folder";
            toolStrip.Items.Add(folderLabel);

            folderComboBox = new ToolStripComboBox();
            folderComboBox.AutoSize = false;
            SetComboBoxItems(folderComboBox, settings.folderPaths);
            folderComboBox.SelectedIndexChanged += FolderComboBox_SelectedIndexChanged;
            toolStrip.Items.Add(folderComboBox);

            ToolStripLabel songLabel = new ToolStripLabel();
            songLabel.Text = "Song";
            toolStrip.Items.Add(songLabel);

            songComboBox = new ToolStripComboBox();
            songComboBox.AutoSize = false;
            songComboBox.Width = 25;
            toolStrip.Items.Add(songComboBox);

            ToolStripButton showSongButton = new ToolStripButton();
            showSongButton.Text = "Show song";
            showSongButton.Click += ShowSongButton_Click;
            toolStrip.Items.Add(showSongButton);

            toolStrip.Items.Add(new ToolStripSeparator());

            ToolStripLabel metronomeLabel = new ToolStripLabel();
            songLabel.Text = "Metronome";
            toolStrip.Items.Add(metronomeLabel);

            metronomeComboBox = new ToolStripComboBox();
            metronomeComboBox.Items.Add("30");
            metronomeComboBox.Items.Add("40");
            metronomeComboBox.Items.Add("50");
            metronomeComboBox.Items.Add("60");
            metronomeComboBox.Items.Add("70");
            metronomeComboBox.Items.Add("80");
            metronomeComboBox.Items.Add("90");
            metronomeComboBox.Items.Add("100");
            toolStrip.Items.Add(metronomeComboBox);

            metronomeButton = new ToolStripButton();
            metronomeButton.Text = "Start";
            metronomeButton.Click += MetronomeButton_Click;
            toolStrip.Items.Add(metronomeButton);

            toolStrip.Items.Add(new ToolStripSeparator());

            ToolStripButton launchPlayerButton = new ToolStripButton();
            launchPlayerButton.Text = "Launch player";
            launchPlayerButton.Click += LaunchPlayerButton_Click;
            toolStrip.Items.Add(launchPlayerButton);

            ToolStripButton addFolderButton = new ToolStripButton();
            addFolderButton.Text = "Add folder";
            addFolderButton.Click += AddFolderButton_Click;
            toolStrip.Items.Add(addFolderButton);

            ToolStripButton changePlayerButton = new ToolStripButton();
            changePlayerButton.Text = "Change player";
            changePlayerButton.Click += ChangePlayerButton_Click;
            toolStrip.Items.Add(changePlayerButton);

            ToolStripButton changeInputButton = new ToolStripButton();
            changeInputButton.Text = "Change input";
            changeInputButton.Click += ChangeInputButton_Click;
            toolStrip.Items.Add(changeInputButton);

            pictureBox = new PictureBox();
            pictureBox.BackColor = Color.White;
            pictureBox.Size = new Size(Size.Width, Size.Height - toolStrip.Height); //1920x1200 -> 1920x1145
            pictureBox.Location = new Point(0, toolStrip.Height);
            Controls.Add(pictureBox);
            //MessageBox.Show(pictureBox.Width + " " + pictureBox.Height);

            folderSwitchingLabel = new Label();
            folderSwitchingLabel.Text = ("Press left or right to change a folder and select to confirm");
            folderSwitchingLabel.TextAlign = ContentAlignment.MiddleCenter;
            folderSwitchingLabel.Size = Size;
            folderSwitchingLabel.Hide();
            Controls.Add(folderSwitchingLabel);

            exitPressedLabel = new Label();
            exitPressedLabel.Text = ("Press stop to exit or play to continue");
            exitPressedLabel.TextAlign = ContentAlignment.MiddleCenter;
            exitPressedLabel.Size = Size;
            exitPressedLabel.Hide();
            Controls.Add(exitPressedLabel);

            if (settings.folderPaths.Count > 0)
            {
                SetFolderComboBoxItems(settings.folderPaths);
                UpdateSongComboBox();
            }

            LoadPages();
        }
    }
}
