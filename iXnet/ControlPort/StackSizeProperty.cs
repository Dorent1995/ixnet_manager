using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    class StackSizeProperty  : IProperty
    {
        public int ID
        {
            get { return 0x88; }
        }

        public int RawLength
        {
            get { return 2; }
        }

        public string DisplayName
        {
            get { return "Free Stack Size"; }
        }

        public string Description
        {
            get { return "Free bytes on the stack."; }
        }

        public Type ValueType
        {
            get { return typeof(UInt16); }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        private UInt16 mValue;
        public object Value
        {
            get
            {
                return mValue;
            }
            set
            {
                
            }
        }

        public void SetRawValue(byte[] data)
        {
            mValue = (UInt16)((int)data[0] << 8 | (int)data[1]);
        }

        public byte[] GetRawValue()
        {
            return null;
        }
    }
}
