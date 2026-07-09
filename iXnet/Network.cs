using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace iXnet
{
    public static class Network
    {
        public static IPAddress[] GetAllUnicastAddresses()
        {
            // This works on both Mono and .NET , but there is a difference: it also
            // includes the LocalLoopBack so we need to filter that one out
            List<IPAddress> Addresses = new List<IPAddress>();
            // Obtain a reference to all network interfaces in the machine
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                foreach (IPAddressInformation uniCast in properties.UnicastAddresses)
                {
                    // Ignore loop-back addresses & IPv6
                    if (!IPAddress.IsLoopback(uniCast.Address) && IsReachableIPV4Address(uniCast.Address))
                        Addresses.Add(uniCast.Address);
                }
            }
            return Addresses.ToArray();
        }

        public static bool IsReachableIPV4Address(IPAddress address)
        {
            // check if its IVP4 and not "link local"
            return address.AddressFamily == AddressFamily.InterNetwork && (!(address.GetAddressBytes()[0] == 169 && address.GetAddressBytes()[1] == 254));
        }
    }
}
