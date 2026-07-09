using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public class ClearLedOnButtonPressedProperty : BoolProperty
    {
        public override int ID
        {
            get { return 0x0A; }
        }

        public override string DisplayName
        {
            get { return "Clear Led on Btn"; }
        }

        public override string Description
        {
            get
            {
                return "If a button on a led module will be pressed, it will be immediately cleared without waiting for a response from the server.";
            }
        }
    }
}
