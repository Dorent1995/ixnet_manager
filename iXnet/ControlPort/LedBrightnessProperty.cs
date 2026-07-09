using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public class LedBrightnessProperty : BrightnessProperty
    {
        public override string Description
        {
            get
            {
                return "Maximum brightness of the RGB Led on led module. The value is in range of [0,255].";
            }
        }

        public LedBrightnessProperty()
            : base(3, "Led Brightness")
        {

        }
    }
}
