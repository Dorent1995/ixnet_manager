using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public abstract class UInt16Property : IProperty
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
            get { return 2; }
        }

        public Type ValueType
        {
            get { return typeof(UInt16); }
        }

        private UInt16 mValue;
        protected UInt16 TypedValue
        {
            get { return mValue; }
            set { mValue = value; }
        }

        virtual public object Value
        {
            get
            {
                return mValue;
            }
            set
            {
                mValue = (UInt16)value;
            }
        }

        public void SetRawValue(byte[] data)
        {
            mValue = (UInt16)((UInt16)data[0] << 8 | (UInt16)data[1]);
        }

        public byte[] GetRawValue()
        {
            byte[] rawValue = new byte[2];
            Converter.FillUInt16(mValue, rawValue, 0, false);
            return rawValue;
        }
    }
}
