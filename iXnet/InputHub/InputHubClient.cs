using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.ComponentModel;
using System.Net.Sockets;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Runtime.Serialization;
using iXnet.ControlPort;


namespace iXnet.InputHub
{
    // This is the connector class for udp input devices which implement the following udp protocol.
    //
    // Communication Protocol: UDP
    // Device Port: 2004
    // Server Port: 2005 (can be changed)
    //
    // Communication is packet based. Each packet has the following header.
    // Packet Header:
    // 1 Byte   - Incrementing PacketID
    // 1 Byte   - Command Id
    //
    // The packet id gets incremented for every packet send by the server. The device should answer
    // to commands with a reply packet, which has the same header but the reply flag set which is 
    // 0x80 on the command id.
    // 
    // Multibyte data is stored as big endian
    //
    // Packets initiated from the server:
    //
    // Discover Request: Command Id = 0x01
    //
    // Discover Reply: Command Id = 0x81
    // 1 Byte   - Proto Version, currently 1
    // 4 Bytes  - Device Firmware Version Major.Minor.Bugfix.Revision
    // 20 Bytes - Serialnumber
    // 20 Bytes - Device Type Name
    // 1 Byte   - NumSubDevices (max. 50 sub devices)
    // Int Array - SubDeviceID each 4 Bytes to identify each sub device later on
    // 
    // SubDevice Property Request: Command Id = 0x02
    // 4 Bytes  - SubDeviceID
    // 
    // SubDevice Property Reply: Command Id= 0x82
    // 4 Bytes  - SubDeviceID
    // 1 Byte   - Device Type - 1 = Character Device
    // 20 Bytes - Sub device name
    // 1 Byte   - Direct input device if not set to zero, direct input devices update the
    //            whole input buffer at once
    //
    // SetListener Request: Command Id = 0x03
    // 4 Bytes  - IP Address of Server
    // 2 Bytes  - Listener Port of Server for incoming messages (Default is 2005)
    // 4 Bytes  - Global Device Id, a random id from the server to identify the device later on (even if its ip has changed)
    // 
    // SetListener Reply: Command Id= 0x83
    //
    // Packets initiated from the device:
    //
    // Character Received Request: Command Id = 0x04
    // 4 Bytes  - Global Device Id, previously set by SetListener
    // 4 Bytes  - SubDeviceID
    // 1 Byte   - NumChars
    // Array Char - Characters to send to the character device buffer
    // 
    // Character Received Reply: Command Id = 0x84
    //

    public class InputHubClient : ServiceClientBase
    {
        [Flags]
        internal enum Commands
        {
            Discover = 0x01,
            DeviceProps = 0x02,
            SetListener = 0x03,
            CharReceived = 0x04,
            ButtonReceived = 0x05,
            AnalogRead = 0x06,
            CharDebug = 0x0A,
            ButtonDebug = 0x0B,
            Reply = 0x80,
        }

        [NonSerialized]
        private byte mPacketID;

        private byte mProtoVersion = 1;
        public byte ProtoVersion
        {
            get { return mProtoVersion; }
        }

        private static readonly UInt16 InputHubRemotePort = 2004;

        public InputHubClient(IPAddress localAddress, IPAddress remoteAddress, byte protoVersion = 1)
            : base(localAddress,remoteAddress,InputHubRemotePort)
        {
            mProtoVersion = protoVersion;
        }

        public IHubDevice[] GetHubDevices()
        {
            byte[] msg = CreateNewMessage(Commands.Discover);
            byte[] packet;
            if (!SendUdpPacket(out packet, msg))
                return new IHubDevice[0];

            List<IHubDevice> charDevices = new List<IHubDevice>();
            if (packet[1] != (byte)(Commands.Discover | Commands.Reply))
                return new IHubDevice[0];

            byte protoVersion = packet[2];
            if (protoVersion != 1 && protoVersion != 2)
                return new IHubDevice[0];

            mProtoVersion = protoVersion;

            Version fwVersion = new Version(
                packet[3],
                packet[4],
                packet[5],
                packet[6]
                );

            string serialNumber = Helper.GetStringFromAscii(packet, 7, 20);
            string devHubName = Helper.GetStringFromAscii(packet, 27, 20);
            byte numDevices = packet[47];
            UInt32[] devIds = new UInt32[numDevices];
            for (int i = 0; i < numDevices; ++i)
            {
                // big endian
                devIds[i] = Converter.GetUInt32(packet, 48 + i * 4, false);
            }

            UInt32 globalDevId = CreateGlobalDeviceId(serialNumber, devHubName);
            for (int i = 0; i < numDevices; ++i)
            {
                IHubDevice dev =
                    CreateDeviceFromProperties(globalDevId, devIds[i], fwVersion, serialNumber, devHubName);

                if (dev != null)
                    charDevices.Add(dev);
            }

            return charDevices.ToArray();
        }

        private IHubDevice CreateDeviceFromProperties(UInt32 globalDevId, UInt32 devId, Version fwVersion, string serialNumber, string devHubName)
        {
            byte[] devPropMsg = CreateNewMessage(Commands.DeviceProps);

            Converter.FillUInt32(devId, devPropMsg, 2, false);
            byte[] propReply;
            if (!SendUdpPacket(out propReply, devPropMsg) || propReply.Length < 7)
                return null;

            UniqueDevID deviceId = new UniqueDevID { GlobalDevID = globalDevId, DevID = devId };

            byte devType = propReply[6];
            switch (devType)
            {
                case 1: // char device
                    {
                        if (propReply.Length < 28)
                            return null;

                        string devName = Helper.GetStringFromAscii(propReply, 7, 20);
                        bool isDirectInput = propReply[27] != 0;

                        return new CharacterDevice(
                            this,
                            LocalAddress,
                            RemoteEndPoint,
                            deviceId,
                            fwVersion,
                            devName,
                            devHubName,
                            isDirectInput,
                            false);
                    }
                case 2: // button device
                    {
                        if (propReply.Length < 30)
                            return null;

                        string devName = Helper.GetStringFromAscii(propReply, 7, 20);
                        bool isDirectInput = propReply[27] != 0;
                        int numButtons = Converter.GetUInt16(propReply, 28, false);

                        return new ButtonDevice(
                            this, 
                            LocalAddress,
                            RemoteEndPoint, 
                            deviceId, 
                            fwVersion, 
                            devName, 
                            devHubName, 
                            isDirectInput, 
                            numButtons, 
                            false);
                    }
                case 3:  // analog device
                    {
                        if (propReply.Length < 34)
                            return null;

                        string devName = Helper.GetStringFromAscii(propReply, 7, 20);
                        bool isDirectInput = propReply[27] != 0;
                        UInt16 numChannels = Converter.GetUInt16(propReply, 28, false);
                        UInt32 maxValue = Converter.GetUInt32(propReply, 30, false);

                        return new AnalogDevice(
                            this,
                            LocalAddress,
                            RemoteEndPoint, 
                            deviceId, 
                            fwVersion, 
                            devName, 
                            devHubName, 
                            isDirectInput, 
                            maxValue, 
                            numChannels);
                    }
                default:
                    {
                        // unknown device
                        return null;
                    }
            }
        }


        internal bool AcquireDevice(IHubDevice dev)
        {
            return SetListenerAddress(dev);
        }

        private bool SetListenerAddress(IHubDevice dev)
        {
            IPAddress localAddress = dev.LocalAddress;
            if (localAddress == null)
                localAddress = InputHubEventReceiver.Singleton.LocalEndpoint.Address;

            byte[] msg = CreateNewMessage(Commands.SetListener);
            Array.Copy(localAddress.GetAddressBytes(), 0, msg, 2, 4);
            Converter.FillUInt16((UInt16)InputHubEventReceiver.Singleton.LocalEndpoint.Port, msg, 6, false);
            Converter.FillUInt32(dev.DeviceID.GlobalDevID, msg, 8, false);

            byte[] reply;
            return SendUdpPacket(out reply, msg);
        }

        internal bool SendCharacterDebug(UInt32 subDeviceID, string characters)
        {
            if (characters.Length > 59)
                characters= characters.Substring(0,59);
                
            byte[] chars= System.Text.ASCIIEncoding.ASCII.GetBytes(characters);

            byte[] msg = CreateNewMessage(Commands.CharDebug, chars.Length + 7);
            Converter.FillUInt32(subDeviceID, msg, 2, false);
            msg[6] = (byte)chars.Length;
            Array.Copy(chars, 0, msg, 7, chars.Length);

            return SendUdpPacket(msg);
        }


        internal UInt32 ReadAnalogValue(UInt32 subDeviceID, UInt16 channel)
        {
            byte[] msg = CreateNewMessage(Commands.AnalogRead);
            Converter.FillUInt32(subDeviceID, msg, 2, false);
            Converter.FillUInt16(channel, msg, 6, false);

            byte[] response;
            if (!SendUdpPacket(out response, msg) || response.Length < 10)
                return 0;

            return Converter.GetUInt32(response, 6, false);
        }


        private UInt32 CreateGlobalDeviceId(string serialNumber, string devHubName)
        {
            using (MD5 md5 = MD5.Create())
            {
                string str = String.Format("{0}-{1}", serialNumber, devHubName);
                byte[] result = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                return Converter.GetUInt32(result, 0, true);
            }
        }

        private byte[] CreateNewMessage(Commands cmd, int size= 20)
        {
            byte[] msg = new byte[size];
            msg[0] = mPacketID;
            msg[1] = (byte)cmd;
            ++mPacketID;

            return msg;
        }

    }
}
