using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.Bootloader
{
    public struct BootloaderProperties
    {
        public String SerialNumber { get; set; }
        public Feature Features { get; set; }
        public Version FirmwareVersion { get; set; }
        public MacAddress MacAddress { get; set; }
    }
}
