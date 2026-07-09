using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    [Flags]
    public enum ResetStatus
    {
        StackOverflow = 0x80,
        SpikeDetection = 0x40,
        Software= 0x20,
        Programmer = 0x10,
        Watchdog = 0x08,
        Brownout = 0x04,
        External= 0x02,
        PowerOn = 0x01,
    }
}
