using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public class SupplyVoltageProperty : UInt16Property
    {
        public override int ID
        {
            get { return 0x8F; }
        }

        public override string DisplayName
        {
            get { return "Supply Voltage"; }
        }

        public override string Description
        {
            get
            {
                return "Voltage at the supply connector.";
            }
        }

        public override object Value
        {
            get
            {
                return String.Format("{0} V", ((float)SupplyVoltageMV / 1000.0f));
            }
            set
            {
                
            }
        }

        UInt16 SupplyVoltageMV
        {
            get { return TypedValue; }
        }
    }
}
