using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    class ActiveLedCountProperty : IProperty
    {
        public int ID
        {
            get { return 0x89; }
        }

        public int RawLength
        {
            get { return 2; }
        }

        public string DisplayName
        {
            get { return "Active Led Count"; }
        }

        public string Description
        {
            get { return "Number of leds attached to the led bus. For some led types the correct amount cannot be detected and is thus the maximum possible amount."; }
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
