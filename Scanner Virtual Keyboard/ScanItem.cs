using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Scanner_Virtual_Keyboard
{
    public class ScanItem
    {
        public string Label { get; protected set; }
        public string Date { get; protected set; }
        public string Time { get; protected set; }
        private Configuration.KeyboardConfiguration keyboard;

        public ScanItem(string line, Configuration.KeyboardConfiguration Keyboard)
        {
            Label = line;
            Date = DateTime.Now.ToShortDateString();
            Time = DateTime.Now.ToLongTimeString();
            keyboard = Keyboard;
        }

        public void Keystroke()
        {
            bool numstate = WindowsInput.InputState.IsNumlocked();
            //if numlock is disable, enable it
            if (!numstate)
                WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMLOCK);

            foreach (char key in Label)
            {
                switch (key)
                {
                    case '0':
                        WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMPAD0);
                        break;
                    case '1':
                        WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMPAD1);
                        break;
                    case '2':
                        WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMPAD2);
                        break;
                    case '3':
                        WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMPAD3);
                        break;
                    case '4':
                        WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMPAD4);
                        break;
                    case '5':
                        WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMPAD5);
                        break;
                    case '6':
                        WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMPAD6);
                        break;
                    case '7':
                        WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMPAD7);
                        break;
                    case '8':
                        WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMPAD8);
                        break;
                    case '9':
                        WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMPAD9);
                        break;
                    default:
                        WindowsInput.InputSimulator.SimulateTextEntry(key.ToString());
                        break;

                }
                if (keyboard.delayKeystorke > 0)
                    Thread.Sleep(keyboard.delayKeystorke);
            }

            //If numlock was disable, disable it now.
            if (!numstate)
                WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.NUMLOCK);

            if(keyboard.appendReturn == true)
                WindowsInput.InputSimulator.SimulateKeyPressEx(WindowsInput.VirtualKeyCode.RETURN);

            if (keyboard.logKeystorke == true)
                Logstroke();
        }

        public void Logstroke()
        {
            StreamWriter sw = File.AppendText(Configuration.Configuration.GetConfigurationFilename("Keystroke.log"));
            sw.WriteLine("{0}\t{1}\t{2}", Date, Time, Label);
            sw.Close();
        }
    }
}
