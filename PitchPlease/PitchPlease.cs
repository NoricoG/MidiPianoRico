using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace PitchPlease
{
    class PitchPlease : Form
    {
        int[] noteY;
        int currentNote;
        int baseX, baseY, halfWidth, halfDistance;
        int keyWidth, keyboardMargin, bottom, keyboardTop;
        ReadingImproverKeyboardHandler readingImproverKeyboardHandler;
        string lastNote = "";
        bool lastCorrect = false;

        public PitchPlease()
        {
            Rectangle workingArea = Screen.GetWorkingArea(this);
            Size = new Size(workingArea.Width, workingArea.Height);
            WindowState = FormWindowState.Maximized;

            Text = "Pitch Please";
            DoubleBuffered = true;

            lastNote = "X";
            halfWidth = 200;
            halfDistance = 25;
            baseX = ClientSize.Width / 2;
            baseY = 30 * halfDistance;

            keyWidth = 50;
            keyboardMargin = (ClientSize.Width - 36 * keyWidth) / 2;
            //MessageBox.Show("36 keyWidth=" + (36 * keyWidth) + "  ClientSize.Width=" + ClientSize.Width + "   keyboardMargin=" + keyboardMargin);
            bottom = ClientSize.Height;
            keyboardTop = bottom - 200;

            //      0,  1,  2,  3,  4,  5,  6
            // 0:  C2, D2, E2, F2, G2, A2, B2
            // 7:  C3, D3, E3, F3, G3, A3, B3
            // 14: C4, D4, E4, F4, G4, A4, B4
            // 21: C5, D5, E5, F5, G5, A5, B5
            // 28: C6

            noteY = new int[29];
            for (int i = 0; i < 29; i++)
            {
                noteY[i] = baseY - i * halfDistance - halfDistance;
            }
            NextNote();


            Paint += PitchPlease_Paint;
            MouseClick += PitchPlease_MouseClick;
            KeyDown += PitchPlease_KeyDown;
            readingImproverKeyboardHandler = new ReadingImproverKeyboardHandler(this, 0);
        }

        private void PitchPlease_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.C: if (currentNote % 7 == 0) NextNote(); break;
                case Keys.D: if (currentNote % 7 == 1) NextNote(); break;
                case Keys.E: if (currentNote % 7 == 2) NextNote(); break;
                case Keys.F: if (currentNote % 7 == 3) NextNote(); break;
                case Keys.G: if (currentNote % 7 == 4) NextNote(); break;
                case Keys.A: if (currentNote % 7 == 5) NextNote(); break;
                case Keys.B: if (currentNote % 7 == 6) NextNote(); break;
            }
        }

        private void PitchPlease_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Y > keyboardTop)
            {
                int key = (e.X - keyboardMargin) / keyWidth;
                if (key == currentNote)
                {
                    NextNote();
                }
            }
        }

        private void NextNote()
        {
            //currentNote = new Random().Next(30);
            currentNote = 2 + 2 * new Random().Next(13);
            Invalidate();
        }

        private void PitchPlease_Paint(object sender, PaintEventArgs e)
        {
            Graphics gr = e.Graphics;

            //Draw bars
            for (int i = 0; i < 5; i++)
            {
                int y = baseY - (i + 2) * halfDistance * 2;
                gr.DrawLine(Pens.Black, baseX - halfWidth, y, baseX + halfWidth, y);
                y -= 12 * halfDistance;
                gr.DrawLine(Pens.Black, baseX - halfWidth, y, baseX + halfWidth, y);
            }

            //Draw Keyboard
            gr.DrawLine(Pens.Black, keyboardMargin, keyboardTop, ClientSize.Width - keyboardMargin, keyboardTop);
            int x = keyboardMargin - keyWidth;
            for (int i = 0; i < 37; i++)
            {
                x += keyWidth;
                gr.DrawLine(Pens.Black, x, bottom, x, keyboardTop);
            }
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (j != 2)
                    {
                        int x1 = keyboardMargin + i * 7 * keyWidth + keyWidth * 3 / 4 + j * keyWidth;
                        gr.FillRectangle(Brushes.Black, x1, keyboardTop, keyWidth / 2, 100);
                    }
                }
            }

            //Draw note
            if (currentNote <= 2)
            {
                //E2
                int y = baseY - 2 * halfDistance;
                gr.DrawLine(Pens.Black, baseX - 4 * halfDistance, y, baseX + 4 * halfDistance, y);
                if (currentNote == 0)
                {
                    //C2
                    y = baseY;
                    gr.DrawLine(Pens.Black, baseX - 4 * halfDistance, y, baseX + 4 * halfDistance, y);
                }
            }
            else if (currentNote == 14)
            {
                //C3
                int y = baseY - 14 * halfDistance;
                gr.DrawLine(Pens.Black, baseX - 4 * halfDistance, y, baseX + 4 * halfDistance, y);
            }
            else if (currentNote >= 26)
            {
                //A5
                int y = baseY - 26 * halfDistance;
                gr.DrawLine(Pens.Black, baseX - 4 * halfDistance, y, baseX + 4 * halfDistance, y);
                if (currentNote == 28)
                {
                    //C6
                    y = baseY - 28 * halfDistance;
                    gr.DrawLine(Pens.Black, baseX - 4 * halfDistance, y, baseX + 4 * halfDistance, y);
                }
            }

            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            gr.FillEllipse(Brushes.Black, baseX - halfDistance * 1.5f, noteY[currentNote], halfDistance * 3, halfDistance * 2);

            //Draw last note
            if (lastCorrect)
            {
                gr.DrawString(lastNote, new Font(FontFamily.GenericSansSerif, 30), Brushes.Green, baseX - 22, noteY[currentNote] + 2);
            }
            else
            {
                gr.DrawString(lastNote, new Font(FontFamily.GenericSansSerif, 20), Brushes.Red, baseX - 12, noteY[currentNote] + 6);
            }
        }

        public void NoteOn(int key)
        {
            switch (key % 7)
            {
                case 0: lastNote = "C"; break;
                case 1: lastNote = "D"; break;
                case 2: lastNote = "E"; break;
                case 3: lastNote = "F"; break;
                case 4: lastNote = "G"; break;
                case 5: lastNote = "A"; break;
                case 6: lastNote = "B"; break;
            }
            lastCorrect = key == currentNote;
            Invalidate();
        }

        public void NoteOff(int key)
        {
            lastNote = "";
            Invalidate();
            if (key == currentNote)
                NextNote();
        }

    }
}
