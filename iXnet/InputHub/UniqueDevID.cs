using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.InputHub
{
    [Serializable]
    public struct UniqueDevID
    {
        public UInt32 GlobalDevID;
        public UInt32 DevID;

        public override string ToString()
        {
            return String.Format("{0}:{1}", GlobalDevID, DevID);
        }
    }
}
