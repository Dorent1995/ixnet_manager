using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace iXnet.InputHub
{
    public abstract class BaseHubDevice : IHubDevice
    {
        private InputHubClient mClient;
        public InputHubClient Client
        {
            get { return mClient; }
        }

        private IPAddress mLocalAddress;
        public IPAddress LocalAddress
        {
            get { return mLocalAddress; }
        }

        private IPEndPoint mEndPoint;
        public IPEndPoint EndPoint
        {
            get { return mEndPoint; }
        }

        private UniqueDevID mDeviceID;
        public UniqueDevID DeviceID
        {
            get { return mDeviceID; }
        }

        private Version mSWVersion;
        public Version SWVersion
        {
            get { return mSWVersion; }
        }

        private string mName;
        public string Name
        {
            get { return mName; }
        }

        private string mDevTypeName;
        public string DevTypeName
        {
            get { return mDevTypeName; }
        }

        public string Address
        {
            get { return mEndPoint.ToString(); }
        }

        public bool SupportsDebugSend
        {
            get { return mClient.ProtoVersion > 1; }
        }

        public abstract bool IsEventDevice { get; }


        private bool mIsDirectInput;
        public bool IsDirectInput
        {
            get { return mIsDirectInput; }
        }

        private int mLastPacketID = 1;
        internal int LastPacketID
        {
            get { return mLastPacketID; }
            set { mLastPacketID = value; }
        }

        protected BaseHubDevice(
            InputHubClient client,
            IPAddress localAddress,
            IPEndPoint endPoint,
            UniqueDevID deviceID,
            Version swVersion,
            string name,
            string devTypeName,
            bool isDirectInput,
            bool acquired
            )
        {
            mClient = client;
            mLocalAddress = localAddress;
            mEndPoint = endPoint;
            mDeviceID = deviceID;
            mSWVersion = swVersion;
            mName = name;
            mDevTypeName = devTypeName;
            mIsDirectInput = isDirectInput;

            InputHubEventReceiver.Singleton.RegisterReceive(this);
        }

        public bool Acquire()
        {
            if (!IsEventDevice)
                return false;

            return Client.AcquireDevice(this);
        }

        public virtual void Dispose()
        {
            if (mClient != null)
            {
                InputHubEventReceiver.Singleton.UnregisterReceive(this);
                mClient = null;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}