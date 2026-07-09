using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public abstract class UInt32Property : IProperty
    {
        public abstract int ID { get; }
        public abstract string DisplayName { get; }

        public virtual string Description
        {
            get { return String.Empty; }
        }

        public virtual bool IsReadOnly
        {
            get { return (ID & 0x80) == 0x80; }
        }

        public int RawLength
        {
            get { return 4; }
        }

        public Type ValueType
        {
            get { return typeof(UInt32); }
        }

        private UInt32 mValue;
        protected UInt32 TypedValue
        {
            get { return mValue; }
            set { mValue = value; }
        }

        public object Value
        {
            get
            {
                return mValue;
            }
            set
            {
                mValue = (UInt32)value;
            }
        }

        public void SetRawValue(byte[] data)
        {
            mValue = (UInt32)data[0] << 24 | (UInt32)data[1] << 16 | (UInt32)data[2] << 8 | (UInt32)data[3];
        }

        public byte[] GetRawValue()
        {
            byte[] rawValue = new byte[4];
            Converter.FillUInt32(mValue, rawValue, 0, false);

            return rawValue;
        }
    }
}
