using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public class ProcessEnableProperty : BoolProperty
    {
        public override int ID
        {
            get { return 0x0E; }
        }

        public override string DisplayName
        {
            get { return "Process Enabled"; }
        }

        public override string Description
        {
            get
            {
                return "Sets if the integrated process is enabled. Changing this property requires a reset of the iXnet device.";
            }
        }
    }
}
