using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public class ReportLedBusCrcProperty : BoolProperty
    {
        public override int ID
        {
            get { return 0x11; }
        }

        public override string DisplayName
        {
            get { return "Report LedBus CRC"; }
        }

        public override string Description
        {
            get
            {
                return "Error log will be written if the led bus encounters receive crc errors.";
            }
        }
    }
}
