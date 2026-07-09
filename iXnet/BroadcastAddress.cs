using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet
{
    public struct BroadcastAddress : IEquatable<BroadcastAddress>
    {
        public static BroadcastAddress Invalid = new BroadcastAddress();

        private byte[] mAddress;
        public byte[] Address
        {
            get { return mAddress; }
            set { mAddress = value; }
        }

        public bool IsValid
        {
            get { return mAddress != null; }
        }

        public BroadcastAddress(byte[] address)
        {
            mAddress = address;
        }

        public static BroadcastAddress ReadFromPacket(byte[] packet, int offset, int len)
        {
            byte[] addr = new byte[len];
            Array.Copy(packet, offset, addr, 0, len);
            return new BroadcastAddress(addr);
        }

        public bool Equals(BroadcastAddress other)
        {
            if (other.mAddress == mAddress)
                return true;

            if (mAddress == null || other.mAddress == null)
                return false;

            return mAddress.SequenceEqual(other.mAddress);
        }
    }
}
