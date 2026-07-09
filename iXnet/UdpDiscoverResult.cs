using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace iXnet
{
    public struct UdpDiscoverResult
    {
        public IPAddress LocalAddress { get; set; }
        public IPEndPoint SrcAddress { get; set; }
        public byte[] Data { get; set; }
    }
}
