using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    class UnknownProperty : IProperty
    {
        private int mID;
        public int ID
        {
            get { return mID; }
        }

        private int mRawLength;
        public int RawLength
        {
            get { return mRawLength; }
        }

        public string DisplayName
        {
            get { return String.Format("ID 0x{0:X2}", mID); }
        }

        public string Description
        {
            get { return "Unknown property."; }
        }

        public Type ValueType
        {
            get { return typeof(String); }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        private string mValue;
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
           
        }

        public byte[] GetRawValue()
        {
            return null;
        }

        public UnknownProperty(int id, int rawLength, string value)
        {
            mID = id;
            mRawLength = rawLength;
            mValue = value;
        }
    }
}
