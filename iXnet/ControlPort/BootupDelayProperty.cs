using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    class BootupDelayProperty : IProperty
    {
        public enum BootupDelay
        {
            No = 0x00,
            Low = 0x01,
            Mid = 0x02,
            High = 0x03
        };

        public int ID
        {
            get { return 0x16; }
        }

        public int RawLength
        {
            get { return 1; }
        }

        public string DisplayName
        {
            get { return "Bootup Delay"; }
        }

        public string Description
        {
            get { return "Additional boot delay which gives external devices time to reach a well defined state.\nNo=0s,Low=3s,Mid=6s,High=9s."; }
        }

        public Type ValueType
        {
            get { return typeof(BootupDelay); }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public object Value
        {
            get
            {
                return mBootDelay;
            }
            set
            {
                mBootDelay = (BootupDelay)value;
            }
        }

        private BootupDelay mBootDelay;
        public BootupDelay BootDelay
        {
            get { return mBootDelay; }
            set { mBootDelay = value; }
        }

        public void SetRawValue(byte[] data)
        {
            mBootDelay = (BootupDelay)data[0];
        }

        public byte[] GetRawValue()
        {
            byte[] data = new byte[1];
            data[0] = (byte)mBootDelay;

            return data;
        }
    }
}
