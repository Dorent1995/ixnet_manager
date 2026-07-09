using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace iXnet.Bootloader
{
    public class BootloaderClient : BroadcastServiceClientBase
    {
        public static readonly UInt16 ClientPort = 2001;
        public static readonly UInt16 LocalPort = 2002;

        enum BldCommands
        {
            CmdDiscover = 0,
            CmdReset = 1,
            CmdFlashInitWrite = 2,
            CmdFlashWritePage = 3,
            CmdFlashReadPage = 4,
            CmdFlashVerifyPage = 7,
            CmdGetProps = 5,
            CmdSetProps = 6,
        }

        private byte mPacketID = 0;

        private MacAddress mAddress;
        public MacAddress Address
        {
            get { return mAddress; }
        }

        private Version mBootloaderVersion;
        public Version BootloaderVersion
        {
            get { return mBootloaderVersion; }
        }

        private string mDeviceName;
        public string DeviceName
        {
            get { return mDeviceName; }
        }

        private BootloaderProperties mProperties;
        public BootloaderProperties Properties
        {
            get { return mProperties; }
        }

        public BootloaderClient(IPAddress localAdapter, IPAddress broadcastTarget, MacAddress address, Version bldVersion, string deviceName)
            : base(localAdapter, broadcastTarget)
        {
            mAddress = address;
            mBootloaderVersion = bldVersion;
            mDeviceName = deviceName;
        }

        public static BootloaderClient[] DiscoverDevices(IPAddress localAdapter = null)
        {
            byte[] msg = new byte[8];
            msg[0] = (byte)BldCommands.CmdDiscover;

            BroadcastDiscoverResult[] responses = DiscoverDevices(
                ClientPort,
                msg,
                r =>
                {
                    if (r.Length < 31)
                        return BroadcastAddress.Invalid;

                    return BroadcastAddress.ReadFromPacket(r, 1, 6);
                },
                localAdapter,
                LocalPort);

            List<BootloaderClient> clients = new List<BootloaderClient>();
            foreach (BroadcastDiscoverResult response in responses)
            {
                Version bldVersion = new Version(
                    response.Data[8],
                    response.Data[9],
                    response.Data[10],
                    0);

                string devName = Helper.GetStringFromAscii(response.Data, 11, 20);
                MacAddress macAddr= new MacAddress(response.BroadcastAddress.Address);

                var client = new BootloaderClient(response.LocalAddress, null, macAddr, bldVersion, devName);
                client.UpdateProperties();

                clients.Add(client);
            }

            return clients.ToArray();
        }

        public bool UpdateProperties()
        {
            byte[] msg = CreateMessage(BldCommands.CmdGetProps);

            byte[] response;
            if (!SendMessage(msg, out response) || response.Length < 38)
                return false;

            BootloaderProperties props = new BootloaderProperties();
            props.SerialNumber = Helper.GetStringFromAscii(response, 8, 18);
            props.Features = (Feature)response[26];

            byte[] macBytes = new byte[6];
            Array.Copy(response, 28, macBytes, 0, 6);
            props.MacAddress = new MacAddress(macBytes);
            
            props.FirmwareVersion = new Version(
                response[34],
                response[35],
                response[36],
                response[37]
                );

            mProperties = props;
            return true;
        }

        public bool SetProperties(BootloaderProperties properties)
        {
            if (properties.SerialNumber == null)
                throw new ArgumentNullException("properties.SerialNumber");
            if (properties.FirmwareVersion == null)
                throw new ArgumentNullException("properties.FirmwareVersion");
            if (properties.MacAddress.Bytes == null)
                throw new ArgumentNullException("properties.MacAddress.Bytes");

            byte[] msg = CreateMessage(BldCommands.CmdSetProps, 38);

            Helper.PutStringIntoAscii(msg, 8, 18, properties.SerialNumber);
            msg[26] = (byte)(properties.Features);
            msg[27] = 0;
            Array.Copy(properties.MacAddress.Bytes, 0, msg, 28, 6);

            msg[34] = (byte)properties.FirmwareVersion.Major;
            msg[35] = (byte)properties.FirmwareVersion.Minor;
            msg[36] = (byte)properties.FirmwareVersion.Build;
            msg[37] = (byte)properties.FirmwareVersion.Revision;

            byte[] response;
            if (!SendMessage(msg, out response))
                return false;

            return UpdateProperties();
        }

        public bool Reset(bool stayInBootloader)
        {
            byte[] msg = CreateMessage(BldCommands.CmdReset, 9);
            msg[8] = (byte)(stayInBootloader ? 1 : 0);

            byte[] response;
            return SendMessage(msg, out response);
        }


        public bool FlashProgramMemory(FirmwareBinary binary, bool verify, bool clearEEProm, Action<int> progress = null)
        {
            if (binary == null)
                throw new ArgumentNullException("binary");

            if (binary.BinaryBytes == null)
                throw new ArgumentException("Firmware binary must be loaded");

            if (!UpdateProperties())
                return false;

            UInt16 spmPageSize;

            if (!SendFlashInitWrite(clearEEProm, out spmPageSize))
                return false;

            if (progress != null)
                progress(1);

            int verifyRetry = 0;
            int maxVerifyRetries = 1;
            byte[] bytes = binary.BinaryBytes;
            for (int currAddress = 0; currAddress < bytes.Length;)
            {
                if (progress != null)
                    progress(1 + (int)(((double)currAddress / (double)bytes.Length) * 98.0));

                byte[] page = ExtractFullPage(bytes, currAddress, spmPageSize);
                if (!SendFlashWritePage((uint)currAddress, page))
                    return false;

                if (verify)
                {
                    if (!SendFlashVerifyPage((uint)currAddress, page))
                    {
                        if (verifyRetry >= maxVerifyRetries)
                            return false;

                        ++verifyRetry;
                    }
                    else
                    {
                        verifyRetry = 0;
                        currAddress += spmPageSize;
                    }
                }
                else
                    currAddress += spmPageSize;
            }

            if (progress != null)
                progress(99);

            var props = Properties;
            props.FirmwareVersion = binary.FirmwareVersion;

            if (!SetProperties(props))
                return false;

            if (progress != null)
                progress(100);

            return true;
        }

        private byte[] ExtractFullPage(byte[] image, int offset, UInt16 pageSize)
        {
            byte[] page = new byte[pageSize];
            int remainBytes = image.Length - offset;
            Array.Copy(image, offset, page, 0, Math.Min(pageSize, remainBytes));
            if (remainBytes < pageSize)
                for (int i = remainBytes; i < pageSize; ++i)
                    page[i] = 0xFF;

            return page;
        }

        private bool SendFlashInitWrite(bool clearEEProm, out UInt16 spmPageSize)
        {
            byte[] msg = CreateMessage(BldCommands.CmdFlashInitWrite, 9);
            msg[8] = (byte)(clearEEProm ? 1 : 0);

            byte[] response;
            if (!SendMessage(msg, out response))
            {
                spmPageSize = 0;
                return false;
            }

            spmPageSize= Converter.GetUInt16(response, 9, false);
            return true;
        }

        private bool SendFlashWritePage(UInt32 pageAddress, byte[] pageData)
        {
            byte[] msg = CreateMessage(BldCommands.CmdFlashWritePage, 13 + pageData.Length);
            msg[8] = 0;
            Converter.FillUInt32(pageAddress, msg, 9, false);
            Array.Copy(pageData, 0, msg, 13, pageData.Length);

            byte[] response;
            if (!SendMessage(msg, out response) || response.Length < 9)
                return false;

            return response[8] == 1;
        }

        private bool SendFlashVerifyPage(UInt32 pageAddress, byte[] pageData)
        {
            if (BootloaderVersion < new Version(1, 1, 0, 0))
                return true;    // no verify command

            byte[] msg = CreateMessage(BldCommands.CmdFlashVerifyPage, 13 + pageData.Length);
            msg[8] = 0;
            Converter.FillUInt32(pageAddress, msg, 9, false);
            Array.Copy(pageData, 0, msg, 13, pageData.Length);

            byte[] response;
            if (!SendMessage(msg, out response) || response.Length < 9)
                return false;

            return response[8] == 1;
        }

       
        private byte[] CreateMessage(BldCommands command, int len = 8)
        {
            byte[] msg = new byte[len];
            msg[0] = (byte)command;
            Array.Copy(Address.Bytes,0,msg,1,6);
            msg[7] = mPacketID++;

            return msg;
        }

        public bool SendMessage(byte[] msg, out byte[] response)
        {
            return SendPacket(
                ClientPort,
                msg,
                out response,
                r =>
                {
                    if (r.Length < 8)
                        return false;

                    var addr = BroadcastAddress.ReadFromPacket(r, 1, 6);
                    return addr.Address.SequenceEqual(Address.Bytes);
                }, 
                -1, 
                LocalPort);
        }
    }
}
