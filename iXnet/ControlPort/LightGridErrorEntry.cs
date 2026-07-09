using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    class LightGridErrorEntry : ErrorEntry
    {
        public LightGridErrorEntry(DateTime timeStamp, byte errorCode)
            : base(timeStamp,errorCode)
        {
            ErrorText = CreateErrorText(errorCode);
        }

        private string CreateErrorText(byte errorCode)
        {
            switch (errorCode)
            {
                case 0x30:
                    return "Lightgrid has too many beams";
                case 0x31:
                    return "Lightgrid reported invalid data";
                default:
                    return String.Format("Unknown light grid error 0x{0:X2}", errorCode);
            }
        }
    }
}
