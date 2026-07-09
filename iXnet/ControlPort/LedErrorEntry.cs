using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public class LedErrorEntry : ErrorEntry
    {
        public int LedID { get; private set; }

        public LedErrorEntry(DateTime timeStamp, byte errorCode, int ledID)
            : base(timeStamp,errorCode)
        {
            LedID = ledID;
            ErrorText = CreateErrorText(errorCode, ledID);
        }

        private string CreateErrorText(byte errorCode, int ledID)
        {
            string errorText;
            switch (errorCode & 0x0F)
            {
                case 0x00:
                    errorText = String.Format("Led {0} has low voltage", ledID);
                    break;
                case 0x01:
                    errorText = String.Format("Led {0} has a hardware failure", ledID);
                    break;
                case 0x02:
                    errorText = String.Format("Ledbus has been truncated before led {0}", ledID);
                    break;
                case 0x03:
                    errorText = String.Format("CRC Preamble error for led {0}",ledID);
                    break;
                case 0x04:
                    errorText = String.Format("CRC Data error for led {0}", ledID);
                    break;
                default:
                    errorText = String.Format("Unknown led bus error 0x{0:X2} at led {1}", errorCode, ledID);
                    break;
            }

            return errorText;
        }
    }
}
