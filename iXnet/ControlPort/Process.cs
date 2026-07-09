using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.ControlPort
{
    public class Process
    {
        private int mAddress;
        public int Address
        {
            get { return mAddress; }
        }

        private string mName;
        public string Name
        {
            get { return mName; }
        }

        private byte mID;
        public byte ID
        {
            get { return mID; }
        }

        private byte mOptionFlags;
        public byte OptionFlags
        {
            get { return mOptionFlags; }
        }

        private int mByteSize;
        public int ByteSize
        {
            get { return mByteSize; }
        }

        public Process(int address, string name, byte id, byte optionFlags, int byteSize)
        {
            mAddress = address;
            mName = name;
            mID = id;
            mOptionFlags = optionFlags;
            mByteSize = byteSize;
        }

    }
}
