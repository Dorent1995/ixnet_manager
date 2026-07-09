using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public abstract class UInt8Property : IProperty
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
            get { return 1; }
        }

        public virtual Type ValueType
        {
            get { return typeof(byte); }
        }

        private byte mValue;
        protected byte TypedValue
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
                mValue = (byte)value;
            }
        }

        public void SetRawValue(byte[] data)
        {
            mValue = data[0];
        }

        public byte[] GetRawValue()
        {
            byte[] rawValue = new byte[1];
            rawValue[0] = mValue;
            return rawValue;
        }
    }
}
