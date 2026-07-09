using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.LedPort
{
    [Flags]
    public enum LedMode
    {
        LedOff = 0x00,
        LedOn = 0x01,
        LedBlinkSlow = 0x02,
        LedBlinkNormal = 0x03,
        LedBlinkFast = 0x04,
        LedFlagNoClearOnButtonPress = 0x10,
        LedFlagClearIRCalibration = 0x20,
        LedFlagAllFlags = 0xF0,
    }
}
