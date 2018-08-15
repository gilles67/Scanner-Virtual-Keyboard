using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsInput
{
    /// <summary>
    /// Specifies various aspects of a MapVirtualKey MapType. This member can have one of the following values.
    /// </summary>
    public enum MapType : uint // UInt32
    {
        /// <summary>
        /// MAPVK_VK_TO_VSC = 0x0000 (uCode is a virtual-key code and is translated into a scan code. If it is a virtual-key code that does not distinguish between left- and right-hand keys, the left-hand scan code is returned. If there is no translation, the function returns 0.)
        /// </summary>
        MAPVK_VK_TO_VSC = 0x0000,

        /// <summary>
        /// MAPVK_VSC_TO_VK = 0x0001 (uCode is a scan code and is translated into a virtual-key code that does not distinguish between left- and right-hand keys. If there is no translation, the function returns 0.)
        /// </summary>
        MAPVK_VSC_TO_VK = 0x0001,

        /// <summary>
        /// MAPVK_VK_TO_CHAR = 0x0002 (uCode is a virtual-key code and is translated into an unshifted character value in the low-order word of the return value. Dead keys (diacritics) are indicated by setting the top bit of the return value. If there is no translation, the function returns 0.)
        /// </summary>
        MAPVK_VK_TO_CHAR = 0x0002,

        /// <summary>
        /// MAPVK_VSC_TO_VK_EX = 0x0003 (uCode is a scan code and is translated into a virtual-key code that distinguishes between left- and right-hand keys. If there is no translation, the function returns 0.)
        /// </summary>
        MAPVK_VSC_TO_VK_EX = 0x0003
    }
}
