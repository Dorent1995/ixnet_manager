using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    [Flags]
    public enum FactoryResetFlags
    {
        Local = 0x01,
        Log = 0x02,
        ProcessData = 0x04,
        PowerCount = 0x08,
    }
}
