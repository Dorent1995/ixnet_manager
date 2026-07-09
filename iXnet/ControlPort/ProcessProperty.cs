using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    class ProcessProperty : UInt8Property
    {
        private int mID;
        public override int ID
        {
            get { return mID; }
        }

        private string mDisplayName;
        public override string DisplayName
        {
            get { return mDisplayName; }
        }

        private Type mEnumType;
        public override Type ValueType
        {
            get
            {
                return mEnumType;
            }
        }

        public override object Value
        {
            get
            {
                return Enum.ToObject(mEnumType, TypedValue);
            }
            set
            {
                TypedValue = (byte)Convert.ChangeType(value, typeof(byte));
            }
        }

        protected ProcessProperty(int processPropID, string propName, Type enumValuesType)
        {
            mID = processPropID | 0x40;
            mDisplayName = String.Format("PRC {0}", propName);
            mEnumType = enumValuesType;
        }
    }
}
