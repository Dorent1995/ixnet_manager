using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace iXnet.ControlPort
{
    public class LedTypeProperty : IProperty
    {
        public enum LedType
        {
            RGB,
            RGBQuantity,
            RGBQuantityButton,
            //RGBQuantityButtonDbg,
            RGBWS2812B,
            //RGBCS8812B,
            Unknown,
        }

        public int ID
        {
            get { return 1; }
        }

        public int RawLength
        {
            get { return 1; }
        }

        public string DisplayName
        {
            get { return "Led Type"; }
        }

        public string Description
        {
            get { return "Type of the led modules on the led bus."; }
        }

        public Type ValueType { get { return typeof(LedType); } }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public object Value
        {
            get
            {
                return mLedType;
            }
            set
            {
                mLedType = (LedType)value;
            }
        }

        private LedType mLedType;
        public LedType LedMode
        {
            get { return mLedType; }
            set { mLedType = value; }
        }


        public void SetRawValue(byte[] data)
        {
            switch (data[0])
            {
                case 0:
                    mLedType = LedType.RGB;
                    break;
                case 1:
                    mLedType = LedType.RGBQuantity;
                    break;
                case 2:
                    mLedType = LedType.RGBQuantityButton;
                    break;
                //case 3:
                //    mLedType = LedType.RGBQuantityButtonDbg;
                //    break;
                case 4:
                    mLedType = LedType.RGBWS2812B;
                    break;
                //case 5:
                //    mLedType = LedType.RGBCS8812B;
                //    break;
                default:
                    mLedType = LedType.Unknown;
                    break;
            }
        }

        public byte[] GetRawValue()
        {
            byte[] data = new byte[1];
            switch(mLedType)
            {
                case LedType.RGB:
                    data[0] = 0;
                    break;
                case LedType.RGBQuantity:
                    data[0] = 1;
                    break;
                case LedType.RGBQuantityButton:
                    data[0] = 2;
                    break;
                //case LedType.RGBQuantityButtonDbg:
                //    data[0] = 3;
                //    break;
                case LedType.RGBWS2812B:
                    data[0] = 4;
                    break;
                //case LedType.RGBCS8812B:
                //    data[0] = 5;
                //    break;
                default:
                    data[0] = 0xFF;
                    break;
            }

            return data;
        }
    }
}
