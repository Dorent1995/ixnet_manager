using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet
{
    public enum SendError
    {
        NoError,
        PacketTransmitFailed,
        MaxRetryReached,
        RetriesNeeded,
    }
}
