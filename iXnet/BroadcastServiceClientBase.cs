using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;

namespace iXnet
{
    public class BroadcastServiceClientBase
    {
        public static int DefaultReplyTimeout= 2000;
        public static int DefaultMaxRetries = 2;
        public static int DefaultRetryDelay = 500;

        private IPAddress mLocalAdapter;
        public IPAddress LocalAdapter
        {
            get { return mLocalAdapter; }
        }

        private IPAddress mBroadcastTarget;
        public IPAddress BroadcastTarget
        {
            get { return mBroadcastTarget; }
        }

        private int mMaxSendRetries = DefaultMaxRetries;
        public int MaxSendRetries
        {
            get { return mMaxSendRetries; }
            set { mMaxSendRetries = value; }
        }

        private int mReplyTimeout = DefaultReplyTimeout;
        public int ReplyTimeout
        {
            get { return mReplyTimeout; }
            set { mReplyTimeout = value; }
        }

        private int mRetryDelay = DefaultRetryDelay;
        public int RetryDelay
        {
            get { return mRetryDelay; }
            set { mRetryDelay = value; }
        }

        protected BroadcastServiceClientBase(IPAddress localAdapter, IPAddress broadCastTarget)
        {
            mLocalAdapter = localAdapter == null ? IPAddress.Any : localAdapter;
            mBroadcastTarget = broadCastTarget == null ? IPAddress.Broadcast : broadCastTarget;
        }

        protected static BroadcastDiscoverResult[] DiscoverDevices(UInt16 clientPort, byte[] discoverPacket, Func<byte[], BroadcastAddress> validateDiscoverResult, IPAddress localAdapter, UInt16 localPort = 0, IPAddress broadcastTarget = null)
        {
            if (broadcastTarget == null)
                broadcastTarget = IPAddress.Broadcast;

            UdpDiscoverResult[] responses= UdpDiscover.SendDiscover(
                localAdapter ?? IPAddress.Any,
                new IPEndPoint(broadcastTarget, clientPort),
                discoverPacket,
                DefaultReplyTimeout,
                localPort);

            return responses.GroupBy(r => validateDiscoverResult(r.Data))
                .Where(group => group.Key.IsValid)
                .Select(group => 
                    {
                        var firstRes = group.First();
                        return new BroadcastDiscoverResult()
                        {
                            LocalAddress = firstRes.LocalAddress,
                            SrcAddress = firstRes.SrcAddress,
                            BroadcastTarget = broadcastTarget,
                            Data = firstRes.Data,
                            BroadcastAddress = group.Key
                        };
                    }
                    ).ToArray();
        }

        protected bool SendPacket(UInt16 clientPort, byte[] packet, out byte[] response, Func<byte[], bool> responseSelector, int timeoutMilli = -1, UInt16 localPort= 0)
        {
            UdpDiscoverResult result;
            int retries = 0;
            if (timeoutMilli < 0)
                timeoutMilli = ReplyTimeout;

            while (!UdpDiscover.SendBroadcastSingleResponse(
                out result,
                LocalAdapter,
                new IPEndPoint(BroadcastTarget, clientPort),
                packet,
                timeoutMilli,
                responseSelector,
                localPort)
                && retries <= MaxSendRetries)
            {
                ++retries;
                Thread.Sleep(RetryDelay);
            }

            if (retries > MaxSendRetries)
            {
                response = null;
                return false;
            }

            response = result.Data;
            return true;
        }
    }
}
