using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet
{
    class Converter
    {
        public static UInt32 GetUInt32(byte[] buffer, int offset, bool litteEndian)
        {
            if (litteEndian == BitConverter.IsLittleEndian)
            {
                return BitConverter.ToUInt32(buffer, offset);
            }
            else
            {
                byte[] dst = new byte[4];
                Array.Copy(buffer, offset, dst, 0, 4);
                return BitConverter.ToUInt32(dst.Reverse().ToArray(), 0);
            }
        }

        public static UInt64 GetUInt64(byte[] buffer, int offset, bool litteEndian)
        {
            if (litteEndian == BitConverter.IsLittleEndian)
            {
                return BitConverter.ToUInt64(buffer, offset);
            }
            else
            {
                byte[] dst = new byte[8];
                Array.Copy(buffer, offset, dst, 0, 8);
                return BitConverter.ToUInt64(dst.Reverse().ToArray(), 0);
            }
        }

        public static void FillUInt32(UInt32 value, byte[] buffer, int offset, bool littleEndian)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (littleEndian != BitConverter.IsLittleEndian)
                bytes = bytes.Reverse().ToArray();

            Array.Copy(bytes, 0, buffer, offset, bytes.Length);
        }

        public static void FillUInt64(UInt64 value, byte[] buffer, int offset, bool littleEndian)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (littleEndian != BitConverter.IsLittleEndian)
                bytes = bytes.Reverse().ToArray();

            Array.Copy(bytes, 0, buffer, offset, bytes.Length);
        }

        public static UInt16 GetUInt16(byte[] buffer, int offset, bool litteEndian)
        {
            if (litteEndian == BitConverter.IsLittleEndian)
            {
                return BitConverter.ToUInt16(buffer, offset);
            }
            else
            {
                byte[] dst = new byte[2];
                Array.Copy(buffer, offset, dst, 0, 2);
                return BitConverter.ToUInt16(dst.Reverse().ToArray(), 0);
            }
        }

        public static void FillUInt16(UInt16 value, byte[] buffer, int offset, bool littleEndian)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (littleEndian != BitConverter.IsLittleEndian)
                bytes = bytes.Reverse().ToArray();

            Array.Copy(bytes, 0, buffer, offset, bytes.Length);
        }

        public static char[] GetCharArray(byte[] buffer, int offset, int len)
        {
            return System.Text.Encoding.UTF8.GetString(buffer, offset, len).ToCharArray();
        }
    }
}
