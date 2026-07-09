using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet
{
    [Flags]
    public enum Feature
    {
        None = 0x00,
        LedBus = 0x01,
        Input = 0x02,
        Process = 0x04,
    }
}
