using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public class ExtIOModeProperty : IProperty
    {
        public enum ExtIOModes
        {
            ButtonOnly = 0x00,
            LightGridButton= 0x01,
        }

        public int ID
        {
            get { return 0x13; }
        }

        public int RawLength
        {
            get { return 1; }
        }

        public string DisplayName
        {
            get { return "ExtIO Mode"; }
        }

        public string Description
        {
            get { return "Operation mode of the extension port."; }
        }

        public Type ValueType { get { return typeof(ExtIOModes); } }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public object Value
        {
            get
            {
                return mMode;
            }
            set
            {
                mMode = (ExtIOModes)value;
            }
        }

        private ExtIOModes mMode;
        public ExtIOModes Mode
        {
            get { return mMode; }
            set { mMode = value; }
        }


        public void SetRawValue(byte[] data)
        {
            mMode = (ExtIOModes)data[0];
        }

        public byte[] GetRawValue()
        {
            byte[] data = new byte[1];
            data[0] = (byte)mMode;

            return data;
        }
    }
}
