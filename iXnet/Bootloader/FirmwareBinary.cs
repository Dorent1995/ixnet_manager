using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace iXnet.Bootloader
{
    public abstract class FirmwareBinary
    {
        private byte[] mBinaryBytes;
        public byte[] BinaryBytes
        {
            get { return mBinaryBytes; }
            protected set { mBinaryBytes = value; }
        }

        private Version mFirmwareVersion;
        public System.Version FirmwareVersion
        {
            get { return mFirmwareVersion; }
            protected set { mFirmwareVersion = value; }
        }

        private string mDeviceName;
        public string DeviceName
        {
            get { return mDeviceName; }
            protected set { mDeviceName = value; }
        }

        private string mFileName;
        public string FileName
        {
            get { return mFileName; }
        }

        protected FirmwareBinary()
        {
            mFirmwareVersion = new Version(0, 0, 0, 0);
        }

        public void Parse(Stream file)
        {
            OnParseFileFormat(file);
        }

        public void Parse(string filename)
        {
            Stream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
            Parse(file);
            mFileName = filename;
        }

        protected abstract void OnParseFileFormat(Stream file);
    }
}
