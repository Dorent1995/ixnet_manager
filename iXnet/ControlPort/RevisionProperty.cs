using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    class RevisionProperty : UInt8Property
    {
        public override int ID
        {
            get { return 0x90; }
        }

        public override string DisplayName
        {
            get { return "HW Revision"; }
        }

        public override string Description
        {
            get
            {
                return "HW Revision of the iXnet module.";
            }
        }
    }
}
