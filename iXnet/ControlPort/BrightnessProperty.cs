using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public class BrightnessProperty : IProperty
    {
        private int mID;
        public int ID { get { return mID; } }

        public int RawLength
        {
            get { return 1; }
        }

        private string mDisplayName;
        public string DisplayName
        {
            get { return mDisplayName; }
        }

        public virtual string Description
        {
            get { return String.Empty; }
        }

        public Type ValueType { get { return typeof(byte); } }

        public bool IsReadOnly
        {
            get { return false; }
        }

        private byte mValue;
        public object Value
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

        public byte Brightness
        {
            get { return mValue; }
            set { mValue = value; }
        }

        protected BrightnessProperty(int id, string displayName)
        {
            mID = id;
            mDisplayName = displayName;
        }

        public void SetRawValue(byte[] data)
        {
            mValue = data[0];
        }

        public byte[] GetRawValue()
        {
            byte[] data = new byte[1];
            data[0] = mValue;
            return data;
        }
    }
}
