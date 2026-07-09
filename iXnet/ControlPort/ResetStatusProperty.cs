using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    class ResetStatusProperty : IProperty
    {
        public int ID
        {
            get { return 0x87; }
        }

        public int RawLength
        {
            get { return 1; }
        }

        public string DisplayName
        {
            get { return "Reset Status"; }
        }

        public string Description
        {
            get { return "Reset conditions."; }
        }


        public Type ValueType
        {
            get { return typeof(ResetStatus); }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        private ResetStatus mResetStatus;
        public object Value
        {
            get
            {
                return mResetStatus;
            }
            set
            {
                
            }
        }

        public void SetRawValue(byte[] data)
        {
            mResetStatus= (ResetStatus)data[0];
        }

        public byte[] GetRawValue()
        {
            return null;
        }
    }
}
