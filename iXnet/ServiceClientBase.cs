using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Collections.Concurrent;
using System.Threading;

namespace iXnet
{
    public class ServiceClientBase : IDisposable
    {
        public static int DefaultReplyTimeout = 2000;
        public static int DefaultMaxRetries = 2;
        public static int DefaultRetryDelay = 500;

        struct UdpPacket
        {
            public byte[] Data;
            public IPEndPoint Src;
        }

        private UdpClient mConn;
        private BlockingCollection<UdpPacket> mInPackets;
        private UInt16 mLocalPort;
        

        private IPAddress mLocalAddress;
        public System.Net.IPAddress LocalAddress
        {
            get { return mLocalAddress; }
        }

        private IPAddress mRemoteAddress;
        public System.Net.IPAddress RemoteAddress
        {
            get { return mRemoteAddress; }
        }

        private UInt16 mRemotePort;
        public System.UInt16 RemotePort
        {
            get { return mRemotePort; }
        }

        public IPEndPoint RemoteEndPoint
        {
            get { return new IPEndPoint(RemoteAddress, RemotePort); }
        }


        private int mReplyTimeout = DefaultReplyTimeout;
        public int ReplyTimeout
        {
            get { return mReplyTimeout; }
            set { mReplyTimeout = value; }
        }

        private int mMaxRetries = DefaultMaxRetries;
        public int MaxRetries
        {
            get { return mMaxRetries; }
            set { mMaxRetries = value; }
        }

        private int mRetryDelay = DefaultRetryDelay;
        public int RetryDelay
        {
            get { return mRetryDelay; }
            set { mRetryDelay = value; }
        }

        private SendReport mLastSendReport = SendReport.Ok;
        public SendReport LastSendReport
        {
            get { return mLastSendReport; }
        }

        public bool IsDisposed
        {
            get { return mConn == null; }
        }

        protected ServiceClientBase(IPAddress localAdapter, IPAddress remoteAddress, UInt16 remotePort)
            : this(localAdapter,remoteAddress,remotePort,0)
        {
        }

        protected ServiceClientBase(IPAddress localAdapter, IPAddress remoteAddress, UInt16 remotePort, UInt16 localPort)
        {
            mLocalAddress= localAdapter;
            mRemoteAddress= remoteAddress;
            mRemotePort= remotePort;
            mLocalPort = localPort;

            SetupUdpClient();
        }

        protected bool SendUdpPacket(byte[] packet, IPEndPoint dst= null)
        {
            mLastSendReport.Error = SendError.NoError;
            byte packetID = packet[0];

            int sendBytes = SendUdpPacketNative(packet, dst);
            if (sendBytes == -2)    // send failed because socket inoperable
            {
                if (ReinitUdpClient())
                {
                    sendBytes = SendUdpPacketNative(packet, dst);
                }
            }

            if (sendBytes == packet.Length)
                return true;

            mLastSendReport.Error = SendError.PacketTransmitFailed;
            return false;
        }

        protected bool SendUdpPacket(out byte[] response, byte[] packet, IPEndPoint dst = null)
        {
            response = null;
            mLastSendReport.Retries = 0;
            while (mLastSendReport.Retries <= MaxRetries)
            {
                if (!SendUdpPacket(packet, dst))
                    return false;

                response = ReceiveResponsePacket(packet, dst);
                if (response != null)
                    return true;

                ++mLastSendReport.Retries;
                mLastSendReport.Error = SendError.RetriesNeeded;
                Thread.Sleep(RetryDelay);
            }

            mLastSendReport.Error = SendError.MaxRetryReached;
            return false;
        }

        protected virtual bool OnValidateResponsePacket(byte[] responsePacket, byte[] requestPacket)
        {
            return responsePacket[0] == requestPacket[0];
        }

        private void SetupUdpClient()
        {
            mConn = new UdpClient(new IPEndPoint(mLocalAddress, mLocalPort));
            mConn.Connect(new IPEndPoint(mRemoteAddress, mRemotePort));

            mInPackets = new BlockingCollection<UdpPacket>(new ConcurrentQueue<UdpPacket>());
            BeginReceiveUdpPacket();
        }

        private void DisposeUdpClient()
        {
            if (mConn != null)
            {
                ((IDisposable)mConn).Dispose();
                mConn = null;
            }

            if (mInPackets != null)
            {
                mInPackets.Dispose();
                mInPackets = null;
            }
        }

        private bool ReinitUdpClient()
        {
            DisposeUdpClient();

            try
            {
                SetupUdpClient();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        private int SendUdpPacketNative(byte[] packet, IPEndPoint dst)
        {
            try
            {
                if (dst == null)
                    return mConn.Send(packet, packet.Length);
                else
                    return mConn.Send(packet, packet.Length, dst);
            }
            catch (System.Exception)
            {
                return -2;
            }
        }

        private byte[] ReceiveResponsePacket(byte[] requestPacket, IPEndPoint src)
        {
            byte packetID = requestPacket[0];

            do
            {
                UdpPacket recvPacket;
                if (!mInPackets.TryTake(out recvPacket, ReplyTimeout))
                    return null;

                if (src == null || recvPacket.Src.Address == src.Address)
                    if (OnValidateResponsePacket(recvPacket.Data, requestPacket))
                        return recvPacket.Data;
            } while (true);
        }

        private void BeginReceiveUdpPacket()
        {
            mConn.BeginReceive(new AsyncCallback(ReceiveUdpPacket), null);
        }

        private void ReceiveUdpPacket(IAsyncResult result)
        {
            try
            {
                if (mConn == null)
                    return;

                IPEndPoint srcEndpoint = null;
                byte[] packet = mConn.EndReceive(result, ref srcEndpoint);
                if (packet != null && packet.Length > 0)
                    mInPackets.Add(new UdpPacket { Data = packet, Src = srcEndpoint });

                BeginReceiveUdpPacket();
            }
            catch
            {
                // do nothing, socket closed
                return;
            }
        }

        public virtual void Dispose()
        {
            DisposeUdpClient();
        }
    }
}
