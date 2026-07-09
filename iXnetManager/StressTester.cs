using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iXnet;
using System.Threading;

namespace iXnetManager
{
    class StressTester : IDisposable
    {
        private iXnetDevice mDevice;
        private Thread mRunThread;
        private bool mCancelRequest;

        private uint mSendPackets;
        public uint SendPackets
        {
            get { return mSendPackets; }
            private set { mSendPackets = value; }
        }

        private uint mFailedPackets;
        public uint FailedPackets
        {
            get { return mFailedPackets; }
            private set { mFailedPackets = value; }
        }

        private uint mRetries;
        public uint Retries
        {
            get { return mRetries; }
            private set { mRetries = value; }
        }

        private uint mPacketsPerSecond;
        public uint PacketsPerSecond
        {
            get { return mPacketsPerSecond; }
            private set { mPacketsPerSecond = value; }
        }

        private bool mIsRunning;
        public bool IsRunning
        {
            get { return mIsRunning; }
            private set { mIsRunning = value; }
        }

        public int ReplyTimeout { get; set; }
        public int MaxRetries { get; set; }
        public int RetryDelay { get; set; }

        public StressTester(iXnetDevice device)
        {
            mDevice = device;
        }

        public void ResetStats()
        {
            SendPackets = 0;
            FailedPackets = 0;
            Retries = 0;
            PacketsPerSecond = 0;
        }

        public void Start()
        {
            if (IsRunning)
                return;

            ResetStats();

            mRunThread = new Thread(new ThreadStart(Run));
            IsRunning = true;
            mRunThread.Start();
        }

        public void Stop()
        {
            if (!IsRunning)
                return;

            mCancelRequest = true;
            mRunThread.Join();
            mRunThread = null;

            IsRunning = false;
        }

        private void Run()
        {
            if (mDevice.LedPort.LedCount == 0)
                return;

            int oldReplyTimeout = mDevice.LedPort.ReplyTimeout;
            int oldMaxRetries = mDevice.LedPort.MaxRetries;
            int oldReplyDelay = mDevice.LedPort.RetryDelay;

            mDevice.LedPort.RetryDelay = RetryDelay;
            mDevice.LedPort.ReplyTimeout = ReplyTimeout;
            mDevice.LedPort.MaxRetries = MaxRetries;

            const int updateSecs = 1;
            uint lastPacketCount = 0;
            DateTime nextMessureTime = DateTime.Now + new TimeSpan(0, 0, updateSecs);

            while (!mCancelRequest)
            {
                mDevice.LedPort.SetLedState(0, iXnet.LedPort.LedMode.LedOn, iXnet.LedPort.LedColor.Red, null);

                switch (mDevice.LedPort.LastSendReport.Error)
                {
                    case SendError.PacketTransmitFailed:
                        ++FailedPackets;
                        break;
                    case SendError.MaxRetryReached:
                        ++FailedPackets;
                        Retries += (uint)mDevice.LedPort.LastSendReport.Retries;
                        break;
                    case SendError.RetriesNeeded:
                        Retries += (uint)mDevice.LedPort.LastSendReport.Retries;
                        break;
                    case SendError.NoError:
                        ++SendPackets;
                        break;
                }

                if (nextMessureTime < DateTime.Now)
                {
                    uint packetDiff = SendPackets - lastPacketCount;
                    lastPacketCount = SendPackets;

                    PacketsPerSecond = packetDiff / updateSecs;

                    nextMessureTime = DateTime.Now + new TimeSpan(0, 0, updateSecs);
                }
            }

            mDevice.LedPort.RetryDelay = oldReplyDelay;
            mDevice.LedPort.ReplyTimeout = oldReplyTimeout;
            mDevice.LedPort.MaxRetries = oldMaxRetries;
        }


        public void Dispose()
        {
            Stop();
        }
    }
}
