using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public class DigitBrightnessProperty : BrightnessProperty
    {
        public override string Description
        {
            get
            {
                return "Brightness of the digits on the led module. The value is in range of [0,255]";
            }
        }

        public DigitBrightnessProperty()
            : base(2, "Digit Brightness")
        {

        }
    }
}
