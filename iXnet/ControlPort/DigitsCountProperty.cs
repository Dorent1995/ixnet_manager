using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    class DigitsCountProperty : IProperty
    {
        public int ID
        {
            get { return 0x86; }
        }

        public int RawLength
        {
            get { return 1; }
        }

        public string DisplayName
        {
            get { return "Digits per Led"; }
        }

        public string Description
        {
            get { return "Number of seven segment digits on each led module on the led bus."; }
        }

        public Type ValueType
        {
            get { return typeof(byte); }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public int NumDigits
        {
            get { return (int)mDigitsCount; }
        }

        private byte mDigitsCount;
        public object Value
        {
            get
            {
                return mDigitsCount;
            }
            set
            {

            }
        }

        public void SetRawValue(byte[] data)
        {
            mDigitsCount = data[0];
        }

        public byte[] GetRawValue()
        {
            return null;
        }
    }
}
