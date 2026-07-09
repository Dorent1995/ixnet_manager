using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace iXnet
{
    public static class UdpDiscover
    {
        public static UdpDiscoverResult[] SendDiscover(IPAddress localAdapter, IPEndPoint dstAddress, byte[] discoverPacket, int discoverTimeout, int localPort = 0)
        {
            UdpClient[] clients = null;
            try
            {
                clients = CreateUdpClient(localAdapter, localPort);
            }
            catch
            {
                return new UdpDiscoverResult[0];
            }


            try
            {
                foreach (UdpClient client in clients)
                {
                    try
                    {
                        client.EnableBroadcast = true;
                        client.Send(discoverPacket, discoverPacket.Length, dstAddress);
                    }
                    catch
                    {
                    }
                }

                if (discoverTimeout > 0)
                    Thread.Sleep(discoverTimeout);

                List<UdpDiscoverResult> results = new List<UdpDiscoverResult>();
                foreach (UdpClient client in clients)
                {
                    while (client.Available > 0)
                    {
                        IPEndPoint from = null;
                        UdpDiscoverResult result = new UdpDiscoverResult();
                        result.Data = client.Receive(ref from);
                        result.SrcAddress = from;
                        result.LocalAddress = ((IPEndPoint)client.Client.LocalEndPoint).Address;

                        results.Add(result);
                    }
                }

                return results.ToArray();
            }
            catch
            {
                return new UdpDiscoverResult[0];
            }
            finally
            {
                foreach (UdpClient client in clients)
                {
                    ((IDisposable)client).Dispose();
                }
            }
        }

        public static bool SendBroadcastSingleResponse(out UdpDiscoverResult response, IPAddress localAdapter, IPEndPoint dstAddress, byte[] packet, int receiveTimeoutMilli, Func<byte[], bool> responseSelector, int localPort = 0)
        {
            response = new UdpDiscoverResult();
            UdpClient[] clients = null;
            try
            {
                clients = CreateUdpClient(localAdapter, localPort);
            }
            catch
            {
                return false;
            }


            try
            {
                foreach (UdpClient client in clients)
                {
                    try
                    {
                        client.EnableBroadcast = true;
                        client.Send(packet, packet.Length, dstAddress);
                    }
                    catch
                    {
                    }
                }

                DateTime start = DateTime.Now;
                TimeSpan timeout = new TimeSpan(0, 0, 0, 0, receiveTimeoutMilli);
                do
                {
                    foreach (UdpClient client in clients)
                    {
                        if (client.Available > 0)
                        {
                            IPEndPoint from = null;
                            response.Data = client.Receive(ref from);
                            response.SrcAddress = from;
                            response.LocalAddress = ((IPEndPoint)client.Client.LocalEndPoint).Address;

                            if (responseSelector(response.Data))
                                return true;
                        }
                    }
                    Thread.Sleep(10);
                } while (DateTime.Now - start < timeout);
            }
            catch
            {
            }
            finally
            {
                foreach (UdpClient client in clients)
                {
                    ((IDisposable)client).Dispose();
                }
            }

            return false;
        }


        private static UdpClient[] CreateUdpClient(IPAddress localAdapter, int localPort = 0)
        {
            if (localAdapter.Equals(IPAddress.Any))
                return Network.GetAllUnicastAddresses().Select(ip => new UdpClient(new IPEndPoint(ip, localPort))).ToArray();
            else
                return new UdpClient[] { new UdpClient(new IPEndPoint(localAdapter, localPort)) };
        }
    }
}
