using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public class LedCountProperty : IProperty
    {
        public int ID
        {
            get { return 0x85; }
        }

        public int RawLength
        {
            get { return 2; }
        }

        public string DisplayName
        {
            get { return "Max Led Count"; }
        }

        public string Description
        {
            get { return "Maximum number of leds that is possible on the led bus with the current selected led type."; }
        }

        public Type ValueType
        {
            get { return typeof(UInt16); }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public int LedCount
        {
            get { return mLedCount; }
        }

        private UInt16 mLedCount;
        public object Value
        {
            get
            {
                return mLedCount;
            }
            set
            {
                
            }
        }

        public void SetRawValue(byte[] data)
        {
            mLedCount = (UInt16)((int)data[0] << 8 | (int)data[1]);
        }

        public byte[] GetRawValue()
        {
            return null;
        }
    }
}
