using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace iXnet.InputHub
{
    public class InputHubEventReceiver
    {
        public static readonly UInt16 LocalEventPort = 2005;

        #region Singleton
        private static InputHubEventReceiver mSingleton;
        public static InputHubEventReceiver Singleton
        {
            get 
            {
                if (mSingleton == null)
                    CreateInstance();

                return mSingleton; 
            }
        }

        public static bool HasInstance
        {
            get { return mSingleton != null; }
        }

        public static void CreateInstance()
        {
            if (HasInstance)
                throw new InvalidOperationException("There is already an input hub event receiver");

            mSingleton = new InputHubEventReceiver();
        }

        public static void DestroyInstance()
        {
            mSingleton = null;
        }

        #endregion

        private UdpClient mClient;
        private Dictionary<UniqueDevID, IHubDevice> mRegisteredDevices;
        private object mThisLock;

        public IPEndPoint LocalEndpoint
        {
            get { return (IPEndPoint)mClient.Client.LocalEndPoint; }
        }

        private InputHubEventReceiver()
        {
            mRegisteredDevices = new Dictionary<UniqueDevID, IHubDevice>();
            mThisLock = new object();
        }

        public void Open(IPEndPoint endPoint = null)
        {
            SetupUdpClient(endPoint);
        }


        private void SetupUdpClient(IPEndPoint endpoint)
        {
            if (endpoint == null)
            {
                endpoint = new IPEndPoint(IPAddress.Any, LocalEventPort);
            }
            else
            {
                if (endpoint.Port == 0)
                    endpoint.Port = LocalEventPort;
            }

            if (endpoint.Address == IPAddress.Any)
                mClient = new UdpClient(endpoint.Port);
            else
                mClient = new UdpClient(endpoint);

            BeginReceiveUdpPacket();
        }

        private void BeginReceiveUdpPacket()
        {
            mClient.BeginReceive(new AsyncCallback(ReceiveUdpPacket), null);
        }

        private void ReceiveUdpPacket(IAsyncResult result)
        {
            try
            {
                if (mClient == null)
                    return;

                IPEndPoint srcEndpoint = null;
                byte[] packet = mClient.EndReceive(result, ref srcEndpoint);

                try
                {
                    OnPacketReceived(packet, srcEndpoint);
                }
                catch
                {
                    // error, keep running
                }

                BeginReceiveUdpPacket();
            }
            catch
            {
                // do nothing, socket closed
                return;
            }
        }

        internal void RegisterReceive(IHubDevice dev)
        {
            lock (mThisLock)
            {
                mRegisteredDevices[dev.DeviceID] = dev;
            }
        }

        internal void UnregisterReceive(IHubDevice dev)
        {
            lock (mThisLock)
            {
                mRegisteredDevices.Remove(dev.DeviceID);
            }
        }

        void OnPacketReceived(byte[] packet, IPEndPoint source)
        {
            switch ((InputHubClient.Commands)packet[1])
            {
                case InputHubClient.Commands.CharReceived:
                    HandleCharReceivedEvent(packet, source);
                    break;
                case InputHubClient.Commands.ButtonReceived:
                    HandleButtonReceivedEvent(packet, source);
                    break;
            }
        }

        private void HandleCharReceivedEvent(byte[] packet, IPEndPoint src)
        {
            var dev = GetHubDevice<CharacterDevice>(packet);
            if (dev == null)
                return;

            byte packetID = packet[0];
            if (dev.LastPacketID == packetID)
                return;

            dev.LastPacketID = packetID;

            byte numChars = packet[10];
            char[] chars = Converter.GetCharArray(packet, 11, numChars);

            dev.RaiseOnCharReceived(chars);
        }

        private void HandleButtonReceivedEvent(byte[] packet, IPEndPoint src)
        {
            var dev = GetHubDevice<ButtonDevice>(packet);
            if (dev == null)
                return;

            byte packetID = packet[0];
            if (dev.LastPacketID == packetID)
                return;

            dev.LastPacketID = packetID;

            byte buttonState = packet[10];
            int buttonID = Converter.GetUInt16(packet, 11, false);
            switch (buttonState)
            {
                case 1:
                    dev.RaiseButtonChangeReceived(ButtonState.Up, buttonID);
                    break;
                case 2:
                    dev.RaiseButtonChangeReceived(ButtonState.Down, buttonID);
                    break;
                case 3: // button pressed
                    dev.RaiseButtonChangeReceived(ButtonState.Down, buttonID);
                    dev.RaiseButtonChangeReceived(ButtonState.Up, buttonID);
                    break;
            }
        }

        private T_DEV GetHubDevice<T_DEV>(byte[] eventPacket) where T_DEV : class,IHubDevice
        {
            UniqueDevID uniqueId;
            uniqueId.GlobalDevID = Converter.GetUInt32(eventPacket, 2, false);
            uniqueId.DevID = Converter.GetUInt32(eventPacket, 6, false);

            IHubDevice vDev = null;
            lock (mThisLock)
            {
                if (!mRegisteredDevices.TryGetValue(uniqueId, out vDev))
                    return default(T_DEV);
            }

            return vDev as T_DEV;
        }


        private IPAddress FindLocalAdapterAddress(IPAddress localAddress)
        {
            IPAddress[] localAdapters = Network.GetAllUnicastAddresses();
            if (localAdapters == null || localAdapters.Length == 0)
                return null;

            IPAddress foundAddress = null;
            if (localAddress != null)
                foundAddress = localAdapters.FirstOrDefault(addr => addr.Equals(localAddress));
            if (foundAddress == null)
                foundAddress = localAdapters.FirstOrDefault(addr => addr.AddressFamily == AddressFamily.InterNetwork);

            return foundAddress;
        }
    }
}
