using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iXnet;
using System.Threading;

namespace iXnetManager
{
    class FlashFirmwareState
    {
        private iXnetDevice mDevice;
        public iXnet.iXnetDevice Device
        {
            get { return mDevice; }
            set { mDevice = value; }
        }

        public string SerialNumber
        {
            get { return Device.SerialNumber; }
        }

        public Version FirmwareVersion
        {
            get { return Device.FirmwareVersion; }
        }

        public int OverallProgress { get; set; }

        public string State { get; set; }

        public bool Error { get; set; }

        public FlashFirmwareState(iXnetDevice device)
        {
            mDevice = device;
            OverallProgress = 0;
            State = "Waiting ...";
            Error = false;
        }
    }
}
