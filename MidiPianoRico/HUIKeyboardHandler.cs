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


namespace MidiPianoRico
{
    abstract class InputHandler
    {
        protected const int SysExBufferSize = 128;
        protected InputDevice inDevice = null;
        protected SynchronizationContext context;

        protected Home home;

        public InputHandler(Home home, int inputDeviceID)
        {
            this.home = home;

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
        }

        public void Close()
        {
            inDevice.Close();
        }

        protected abstract void HandleChannelMessageReceived(object sender, ChannelMessageEventArgs e);
    }

    class KeyboardHandler : InputHandler
    {
        private OutputDevice outDevice;

        public KeyboardHandler(Home home, int inputDeviceID, int outputDeviceID = 0) : base(home, inputDeviceID)
        {
            outDevice = new OutputDevice(outputDeviceID);
        }

        protected override void HandleChannelMessageReceived(object sender, ChannelMessageEventArgs e)
        {
            context.Post(delegate (object dummy)
            {
                outDevice.Send(e.Message);
            }, null);
        }
    }

    class ButtonsHandler : InputHandler
    {
        public ButtonsHandler(Home home, int inputDeviceID) : base(home, inputDeviceID) { }

        protected override void HandleChannelMessageReceived(object sender, ChannelMessageEventArgs e)
        {
            context.Post(delegate (object dummy)
            {
                if (e.Message.Data2 == 127)
                {
                    switch (e.Message.Data1)
                    {
                        /*
                        5D =  93 = A  6 = Stop
                        5E =  94 = Bb 6 = Play
                        5F =  95 = B  6 = Record
                        60 =  96 = C  7 = Up
                        61 =  97 = C# 7 = Down
                        62 =  98 = D  7 = Left
                        63 =  99 = Eb 7 = Right
                        64 = 100 = E  7 = Center
                        */
                        case 93:
                            home.HandleStopButtonPress();
                            break;
                        case 94:
                            home.HandlePlayButtonPress();
                            break;
                        case 95:
                            home.HandleRecordButtonPress();
                            break;
                        case 96:
                            home.HandleUpButtonPress();
                            break;
                        case 97:
                            home.HandleDownButtonPress();
                            break;
                        case 98:
                            home.HandleLeftButtonPress();
                            break;
                        case 99:
                            home.HandleRightButtonPress();
                            break;
                        case 100:
                            home.HandleCenterButtonPress();
                            break;
                    }
                }
            }, null);
        }
    }
}
