using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public class LightGridDedicatedDeviceProperty : BoolProperty
    {
        public override int ID
        {
            get { return 0x12; }
        }

        public override string DisplayName
        {
            get { return "Light Grid Dedicated Device"; }
        }

        public override string Description
        {
            get
            {
                return "Create an extra input device for the light grid instead to use the led bus button device.";
            }
        }
    }
}
