﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Sanford.Multimedia.Midi.UI;
using System.Diagnostics;

namespace MidiPianoRico
{
    partial class Home : Form
    {
        private void FolderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string path = folderComboBox.SelectedItem.ToString();
            SetSongComboBoxItems(FileHandler.GetFilePaths(path));
        }

        private void ShowSongButton_Click(object sender, EventArgs e)
        {
            LoadPages();
        }

        private void AddFolderButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                settings.folderPaths.Add(dialog.SelectedPath);
                settings.folderPaths.Sort();
                FileHandler.SaveSettings(settings);
                SetComboBoxItems(folderComboBox, settings.folderPaths);
            }
            else
            {
                MessageBox.Show("No new folder is selected");
            }
        }

        private void ChangeInputButton_Click(object sender, EventArgs e)
        {
            InputDeviceDialog dialog = new InputDeviceDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                keyboardHandler = new KeyboardHandler(this, dialog.InputDeviceID);
            }
            else
            {
                MessageBox.Show("No new input device is selected");
            }
        }

        public void HandleStopButtonPress()
        {
            if (! exitPressed)
            {
                exitPressed = true;
                exitPressedLabel.Show();
                pictureBox.Hide();
            }
            else
            {
                Close();
            }
        }

        public void HandlePlayButtonPress()
        {
            if (exitPressed)
            {
                exitPressed = false;
                exitPressedLabel.Hide();
                pictureBox.Show();
            }
        }

        public void HandleRecordButtonPress()
        {
           
        }

        public void HandleUpButtonPress()
        {
            PreviousPage();
        }

        public void HandleDownButtonPress()
        {
            NextPage();
        }

        public void HandleLeftButtonPress()
        {
            if (folderSwitching)
            {
                if (folderComboBox.SelectedIndex > 0)
                {
                    folderComboBox.SelectedIndex--;
                }
                //UpdateSongComboBox();
            }
            else
            {
                if (songComboBox.SelectedIndex > 0)
                {
                    songComboBox.SelectedIndex--;
                }
                LoadPages();
            }
        }

        public void HandleRightButtonPress()
        {
            if (folderSwitching)
            {
                if (folderComboBox.SelectedIndex + 1 < folderComboBox.Items.Count)
                {
                    folderComboBox.SelectedIndex++;
                }
                //UpdateSongComboBox();
            }
            else
            {
                if (songComboBox.SelectedIndex + 1 < songComboBox.Items.Count)
                {
                    songComboBox.SelectedIndex++;
                }
                LoadPages();
            }
        }

        public void HandleCenterButtonPress()
        {
            if (folderSwitching)
            {
                folderSwitching = false;
                folderSwitchingLabel.Hide();
                UpdateSongComboBox();
                pictureBox.Show();
            }
            else
            {
                folderSwitching = true;
                folderSwitchingLabel.Show();
                pictureBox.Hide();
            }
        }
    }
}
