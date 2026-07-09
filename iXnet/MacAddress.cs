using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet
{
    public struct MacAddress : IEquatable<MacAddress>
    {
        public byte[] Bytes;

        public MacAddress(byte[] bytes)
        {
            Bytes = bytes;
        }

        public override string ToString()
        {
            return Bytes != null ? BitConverter.ToString(Bytes) : String.Empty;
        }

        public bool Equals(MacAddress other)
        {
            return this == other;
        }

        public static bool operator ==(MacAddress a, MacAddress b)
        {
            if (a.Bytes == b.Bytes)
                return true;

            if (a.Bytes == null || b.Bytes == null)
                return false;

            return a.Bytes.SequenceEqual(b.Bytes);
        }

        public static bool operator !=(MacAddress a, MacAddress b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
