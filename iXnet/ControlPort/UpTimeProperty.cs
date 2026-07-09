using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    class UpTimeProperty : IProperty
    {
        public int ID
        {
            get { return 0x8B; }
        }

        public int RawLength
        {
            get { return 4; }
        }

        public string DisplayName
        {
            get { return "Up Time"; }
        }

        public string Description
        {
            get { return "Time the iXnet device was up since the last reset."; }
        }

        public Type ValueType
        {
            get { return typeof(TimeSpan); }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        private TimeSpan mUpTime;
        public System.TimeSpan UpTime
        {
            get { return mUpTime; }
        }

        public object Value
        {
            get
            {
                return mUpTime;
            }
            set
            {
                mUpTime = (TimeSpan)value;
            }
        }

        public void SetRawValue(byte[] data)
        {
            UInt32 seconds = (UInt32)data[0] << 24 | (UInt32)data[1] << 16 | (UInt32)data[2] << 8 | (UInt32)data[3];

            mUpTime = new TimeSpan(0, 0, (int)seconds);
        }

        public byte[] GetRawValue()
        {
            return null;
        }

    }
}
