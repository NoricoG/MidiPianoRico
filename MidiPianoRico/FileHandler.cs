using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace MidiPianoRico
{
    static class FileHandler
    {
        public static Settings LoadSettings()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MidiPianoRico");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path += "/settings.txt";

            if (File.Exists(path))
            {
                List<string> lines = new List<string>();
                using (StreamReader streamReader = new StreamReader(path))
                {
                    string line = "";
                    while ((line = streamReader.ReadLine()) != null)
                        lines.Add(line);
                }
                return new Settings(lines);
            }
            else
            {
                Settings settings = new Settings();
                SaveSettings(settings);
                return settings;
            }
        }

        public static void SaveSettings(Settings settings)
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MidiPianoRico/settings.txt");
            using (StreamWriter streamWriter = new StreamWriter(path))
            {
                foreach (string line in settings.ToLines())
                {
                    streamWriter.WriteLine(line);
                }
            }

        }

        public static List<string> GetFilePaths(string path)
        {
            List<string> paths = new List<string>();
            //MessageBox.Show("The selected path is: " + path);
            string[] found = Directory.GetFiles(path, "*.mscz");
            for (int i = 0; i < found.Length; i++)
            {
                int filenameLength = found[i].Length - path.Length - 6;
                string filename = found[i].Substring(path.Length + 1, filenameLength);
                paths.Add(filename);
            }
            paths.Sort();
            return paths;
        }

        public static Bitmap[] LoadPages(string path, string song, int width)
        {
            string[] found = Directory.GetFiles(path, song + "-?.png");
            Bitmap[] result = new Bitmap[found.Length];
            for (int i = 0; i < found.Length; i++)
            {
                try
                {
                    using (Image temp = Image.FromFile(found[i]))
                    {
                        result[i] = new Bitmap(temp);
                    }
                    float ratio = (float)result[i].Width / width;
                    if (ratio > 1)
                    {
                        result[i] = new Bitmap(result[i], (int)((float)result[i].Width / ratio), (int)((float)result[i].Height / ratio));
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed to load an image");
                    result[i] = new Bitmap(width, 100);
                }
            }
            return result;
        }
    }
}
