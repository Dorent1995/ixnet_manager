using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    class LightGridModeProperty : IProperty
    {
        public enum LightGridModes
        {
            Mode2D = 0x00,
            Mode1D = 0x01,
            Mode1DMulti = 0x02,
        }

        public int ID
        {
            get { return 0x15; }
        }

        public int RawLength
        {
            get { return 1; }
        }

        public string DisplayName
        {
            get { return "Light Grid Mode"; }
        }

        public string Description
        {
            get { return "Operation mode of the light grid"; }
        }

        public Type ValueType { get { return typeof(LightGridModes); } }

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
                mMode = (LightGridModes)value;
            }
        }

        private LightGridModes mMode;
        public LightGridModes Mode
        {
            get { return mMode; }
            set { mMode = value; }
        }


        public void SetRawValue(byte[] data)
        {
            mMode = (LightGridModes)data[0];
        }

        public byte[] GetRawValue()
        {
            byte[] data = new byte[1];
            data[0] = (byte)mMode;

            return data;
        }
    }
}
