using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    class PowerCycleCountProperty : UInt32Property
    {
        public override int ID
        {
            get { return 0x94; }
        }

        public override string DisplayName
        {
            get { return "Power Cycle Count"; }
        }

        public UInt32 PowerCycleCount
        {
            get { return TypedValue; }
        }
    }
}
