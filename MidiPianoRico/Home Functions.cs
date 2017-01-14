using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Sanford.Multimedia.Midi.UI;

namespace MidiPianoRico
{
    partial class Home : Form
    {
        private void SetFolderComboBoxItems(List<string> items)
        {
            SetComboBoxItems(folderComboBox, items);
            if (items.Count > 0)
            {
                folderComboBox.SelectedIndex = 0;
            }
        }

        private void UpdateSongComboBox()
        {
            string path = folderComboBox.SelectedItem.ToString();
            SetSongComboBoxItems(FileHandler.GetFilePaths(path));
        }

        private void SetSongComboBoxItems(List<string> items)
        {
            SetComboBoxItems(songComboBox, items);
            if (items.Count > 0)
            {
                songComboBox.SelectedIndex = 0;
                LoadPages();
            }
        }

        private void SetComboBoxItems(ToolStripComboBox comboBox, List<string> items)
        {
            comboBox.Items.Clear();
            int maxWidth = 25;
            Graphics graphics = Graphics.FromImage(new Bitmap(10, 10));
            foreach (string item in items)
            {
                comboBox.Items.Add(item);
                maxWidth = Math.Max(maxWidth, (int)graphics.MeasureString(item, Font).Width + 25);
            }
            comboBox.Width = maxWidth;
            comboBox.DropDownWidth = maxWidth;
        }

        private void LoadPages()
        {
            if (folderComboBox.Items.Count > 0)
            {
                if (folderComboBox.SelectedItem == null)
                {
                    folderComboBox.SelectedIndex = 0;
                }
                if (songComboBox.Items.Count > 0)
                {
                    if (songComboBox.SelectedItem == null)
                    {
                        songComboBox.SelectedIndex = 0;
                    }
                    pages = FileHandler.LoadPages(folderComboBox.SelectedItem.ToString(), songComboBox.SelectedItem.ToString(), pictureBox.Width);
                    currentPage = 0;
                    ShowPage();
                }
            }
        }

        private void ShowPage()
        {
            pictureBox.Image = pages[currentPage];
        }

        private void NextPage()
        {
            if (currentPage + 1 < pages.Length)
                currentPage++;
            ShowPage();
        }

        private void PreviousPage()
        {
            if (currentPage > 0)
                currentPage--;
            ShowPage();
        }
    }
}
