using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public class DebugDisplayProperty : BoolProperty
    {
        public override int ID
        {
            get { return 0x17; }
        }
        public override string DisplayName
        {
            get { return "Debug Display"; }
        }
        public override string Description
        {
            get
            {
                return "Enables debug display mode on modules. RGB LED and 7-segment show sensor values instead of stream data.";
            }
        }
    }
}
