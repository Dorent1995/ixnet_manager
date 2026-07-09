using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace iXnet.ControlPort
{
    public struct InterfaceProperties
    {
        public IPAddress Address;
        public IPAddress Netmask;
        public IPAddress Gateway;
    }
}
