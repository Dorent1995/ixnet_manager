using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public abstract class BoolProperty : IProperty
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

        public Type ValueType
        {
            get { return typeof(bool); }
        }

        private bool mValue;
        protected bool TypedValue
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
                mValue = (bool)value;
            }
        }

        public void SetRawValue(byte[] data)
        {
            mValue = data[0] != 0;
        }

        public byte[] GetRawValue()
        {
            byte[] data = new byte[1];
            data[0] = (byte)(mValue ? 1 : 0);
            return data;
        }
    }
}
