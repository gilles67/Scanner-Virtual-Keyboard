using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WindowsInput
{
    public static class InputState
    {
        //Write On Class Scope
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);

        //You Can Use Below Line To Detect Caps Lock Mode
        public static bool IsNumlocked()
        {
            bool isCapsLock = (((ushort)GetKeyState(0x14 /*VK_CAPITAL*/)) & 0xffff) != 0;
            bool isNumLock = (((ushort)GetKeyState(0x90 /*VK_NUMLOCK*/)) & 0xffff) != 0;
            return isNumLock;
        }
    }
}
