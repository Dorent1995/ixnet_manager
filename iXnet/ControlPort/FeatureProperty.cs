using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public class FeatureProperty : IProperty
    {
        public int ID
        {
            get { return 0x84; }
        }

        public int RawLength
        {
            get { return 1; }
        }

        public string DisplayName
        {
            get { return "Features"; }
        }

        public string Description
        {
            get { return "Program features enabled in the iXnet module."; }
        }

        public Type ValueType
        {
            get { return typeof(Feature); }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        private Feature mValue;
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

        public Feature Features
        {
            get { return mValue; }
        }

        public void SetRawValue(byte[] data)
        {
            mValue = (Feature)data[0];
        }

        public byte[] GetRawValue()
        {
            byte[] data = new byte[1];
            data[0] = (byte)mValue;
            return data;
        }
    }
}
