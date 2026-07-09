using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet
{
    static class Helper
    {
        public static string GetStringFromAscii(byte[] data, int offset)
        {
            if (data == null)
                return "";

            Encoding enc = Encoding.ASCII;
            int inx = Array.FindIndex(data, offset, (x) => x == 0);//search for 0
            if (inx >= 0)
                return (enc.GetString(data, offset, inx - offset));
            else
                return (enc.GetString(data, offset, data.Length - offset));
        }

        public static string GetStringFromAscii(byte[] data, int offset, int numBytes)
        {
            return GetStringFromAscii(GetSubArray(data, offset, numBytes),0);
        }

        public static byte[] GetSubArray(byte[] packet, int offset, int numBytes)
        {
            byte[] res = new byte[numBytes];
            Array.Copy(packet, offset, res, 0, numBytes);
            return res;
        }

        public static void PutStringIntoAscii(byte[] dst, int dstOffset, int dstMaxBytes, string src)
        {
            byte[] bytes= System.Text.Encoding.ASCII.GetBytes(src);

            Array.Copy(bytes, 0, dst, dstOffset, Math.Min(bytes.Length, dstMaxBytes));
        }
    }
}
