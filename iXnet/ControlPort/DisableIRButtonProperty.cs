using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public class DisableIRButtonProperty : BoolProperty
    {
        public override int ID
        {
            get { return 0x18; }
        }
        public override string DisplayName
        {
            get { return "Disable IR/Button Input"; }
        }
        public override string Description
        {
            get
            {
                return "Disables IR/button hardware for low-cost modules without sensors. Setting is persisted in module EEPROM.";
            }
        }
    }
}
