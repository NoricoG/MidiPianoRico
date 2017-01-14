using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MidiPianoRico
{
    class Settings
    {
        public int inputID = 1;
        public List<string> folderPaths = new List<string>();

        public Settings()
        {
            
        }

        public Settings(List<string> lines)
        {
            try
            {
                inputID = int.Parse(lines[0]);
                if (lines.Count > 1)
                {
                    for (int i = 1; i < lines.Count; i++)
                    {
                        if (Directory.Exists(lines[i]))
                        {
                            folderPaths.Add(lines[i]);
                        }
                        else
                        {
                            MessageBox.Show("The folder \"" + lines[i] + "\" doesn't exist anymore");
                        }
                    }
                    FileHandler.SaveSettings(this);
                }
            }
            catch
            {
                MessageBox.Show("The settings file is corrupt. Default settings are loaded");
            }
        }
        
        public List<string> ToLines()
        {
            List<string> lines = new List<string>();
            lines.Add(inputID.ToString());
            if (folderPaths.Count > 0)
            {
                for (int i = 0; i < folderPaths.Count; i++)
                {
                    lines.Add(folderPaths[i]);
                }
            }
            return lines;
        }
    }
}
