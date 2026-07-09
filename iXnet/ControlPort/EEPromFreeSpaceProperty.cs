using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public class EEPromFreeSpaceProperty : UInt32Property
    {
        public override int ID
        {
            get { return 0x8D; }
        }

        public override string DisplayName
        {
            get { return "EEProm Freespace"; }
        }

        public override string Description
        {
            get
            {
                return "Free space in EEProm in bytes.";
            }
        }

        public System.UInt32 EEPromFreeSpace
        {
            get { return TypedValue; }
        }
    }
}
