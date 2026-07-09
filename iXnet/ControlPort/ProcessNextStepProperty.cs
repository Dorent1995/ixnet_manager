using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    class ProcessNextStepProperty : ProcessProperty
    {
        enum Values
        {
            No = 0,
            Every = 1,
            Any = 2,
        }

        public ProcessNextStepProperty()
            : base(0, "Led Next Step", typeof(Values))
        {}
    }
}
