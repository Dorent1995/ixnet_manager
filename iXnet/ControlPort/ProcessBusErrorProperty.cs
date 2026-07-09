using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    class ProcessBusErrorProperty : ProcessProperty
    {
        enum Values
        {
            Off = 0,
            On = 1,
        }

        public ProcessBusErrorProperty()
            : base(1,"Bus Error Detect",typeof(Values))
        {

        }
    }
}
