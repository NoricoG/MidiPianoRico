using System.Windows.Forms;
using System.Drawing;

/*
TODO:
    Code improvement
    Open, draw and play midi file using alphaTab or MusicXML and LilyPond
*/

namespace MidiPianoRico
{
    partial class Home : Form
    {
        private KeyboardHandler keyboardHandler;
        private ButtonsHandler buttonsHandler;
        public PictureBox pictureBox;
        private Settings settings;

        private ToolStripComboBox folderComboBox, songComboBox;
        private Label folderSwitchingLabel, exitPressedLabel;

        private Bitmap[] pages;
        private int currentPage = 1;
        private bool folderSwitching = false;
        private bool exitPressed = false;
        private bool playerLaunched = false;
        
        public Home()
        {
            Text = "MidiPianoRico";
            //keyboardHandler = new KeyboardHandler(this, 1);
            keyboardHandler = new KeyboardHandler(this, 1);
            buttonsHandler = new ButtonsHandler(this, 2);
            settings = FileHandler.LoadSettings();
            SetSize();
            AddControls();
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

            ToolStripButton addFolderButton = new ToolStripButton();
            addFolderButton.Text = "Add folder";
            addFolderButton.Click += AddFolderButton_Click;
            toolStrip.Items.Add(addFolderButton);

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
