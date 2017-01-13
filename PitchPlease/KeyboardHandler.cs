using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Sanford.Multimedia;
using Sanford.Multimedia.Midi;
using Sanford.Multimedia.Midi.UI;
using System.Windows.Forms;

namespace PitchPlease
{
    class ReadingImproverKeyboardHandler
    {
        private const int SysExBufferSize = 128;
        private InputDevice inDevice = null;
        private OutputDevice outDevice = null;
        private SynchronizationContext context;

        private PitchPlease pitchPlease;

        public ReadingImproverKeyboardHandler(PitchPlease pitchPlease, int inputDeviceID)
        {
            this.pitchPlease = pitchPlease;

            if (InputDevice.DeviceCount == 0)
            {
                MessageBox.Show("No MIDI input devices available.");
            }
            else
            {
                try
                {
                    context = SynchronizationContext.Current;
                    inDevice = new InputDevice(inputDeviceID);
                    inDevice.ChannelMessageReceived += HandleChannelMessageReceived;
                    inDevice.StartRecording();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error: " + ex);
                }
            }

            if (OutputDevice.DeviceCount == 0)
            {
                MessageBox.Show("No MIDI output devices available.");
            }
            else
            {
                try
                {
                    context = SynchronizationContext.Current;
                    outDevice = new OutputDevice(0);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error: " + ex);
                }
            }
        }

        private void HandleChannelMessageReceived(object sender, ChannelMessageEventArgs e)
        {
            context.Post(delegate (object dummy)
            {
                int midiNote = e.Message.Data1;
                int note = -1;
                switch (midiNote)
                {
                    case 36: note = 0; break; //C2
                    case 38: note = 1; break; //D2
                    case 40: note = 2; break; //E2
                    case 41: note = 3; break; //F2
                    case 43: note = 4; break; //G2
                    case 45: note = 5; break; //A2
                    case 47: note = 6; break; //B2
                    case 48: note = 7; break; //C3
                    case 50: note = 8; break; //D3
                    case 52: note = 9; break; //E3
                    case 53: note = 10; break; //F3
                    case 55: note = 11; break; //G3
                    case 57: note = 12; break; //A3
                    case 59: note = 13; break; //B3
                    case 60: note = 14; break; //C4
                    case 62: note = 15; break; //D4
                    case 64: note = 16; break; //E4
                    case 65: note = 17; break; //F4
                    case 67: note = 18; break; //G4
                    case 69: note = 19; break; //A4
                    case 71: note = 20; break; //B4
                    case 72: note = 21; break; //C5
                    case 74: note = 22; break; //D5
                    case 76: note = 23; break; //E5
                    case 77: note = 24; break; //F5
                    case 79: note = 25; break; //G5
                    case 81: note = 26; break; //A5
                    case 83: note = 27; break; //B5
                    case 84: note = 28; break; //C6
                }
                if (note != -1)
                {
                    if ((e.Message.Command == ChannelCommand.NoteOn && e.Message.Data2 == 0) || e.Message.Command == ChannelCommand.NoteOff)
                    {
                        pitchPlease.NoteOff(note);
                    }
                    else if (e.Message.Command == ChannelCommand.NoteOn)
                    {
                        pitchPlease.NoteOn(note);
                        outDevice.Send(e.Message);
                    }
                }
            }, null);
        }
    }
}
