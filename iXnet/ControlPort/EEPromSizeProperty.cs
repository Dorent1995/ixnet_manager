using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public class EEPromSizeProperty : UInt32Property
    {
        public override int ID
        {
            get { return 0x8C; }
        }

        public override string DisplayName
        {
            get { return "EEProm Size"; }
        }

        public override string Description
        {
            get
            {
                return "Size of the EEProm in bytes.";
            }
        }

        public System.UInt32 EEPromSize
        {
            get { return TypedValue; }
        }
    }
}
