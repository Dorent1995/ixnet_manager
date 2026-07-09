using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace iXnet.ControlPort
{
    // control port protocol client
    // supports only clients starting from proto version 2
    // proto version 1 will be discovered but no interaction is possible because of the major differences in communication
    public class ControlPortClient : BroadcastServiceClientBase
    {
        private static UInt16 ClientPort = 1999;

        private static readonly byte[] CmdIDProto1 = { 0x80, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        private static readonly byte[] CmdIDProto2 = { 0x80, 0x01, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10, 0x20, 0x30 };

        enum Cmd
        {
            Discover = 1,
            Reset,
            GetInterface,
            SetInterface,
            GetProperties,
            SetProperty,
            TriggerUpdate,
            GetError,
            ReadEEProm,
            WriteEEProm,
            GetNextProcessHeader,
            DeleteProcess,
            ReserveProcessSpace,
            GetLog,
            FactoryReset,
        }

        private byte mNextPacketID = 0;
        private byte[] mProtoIDMap;

        private int mProtoVersion;
        public int ProtoVersion
        {
            get { return mProtoVersion; }
        }
        private MacAddress mAddress;
        public MacAddress Address
        {
            get { return mAddress; }
        }

        private IPAddress mIPAddress;
        public IPAddress IPAddress
        {
            get { return mIPAddress; }
        }

        private Version mFWVersion;
        public System.Version FWVersion
        {
            get { return mFWVersion; }
        }

        private string mDeviceName;
        public string DeviceName
        {
            get { return mDeviceName; }
        }

        private string mSerialNumber;
        public string SerialNumber
        {
            get { return mSerialNumber; }
        }

        public bool CanReset
        {
            get { return HasCmdId(Cmd.Reset); }
        }

        public bool CanGetInterface
        {
            get { return HasCmdId(Cmd.GetInterface); }
        }

        public bool CanSetInterface
        {
            get { return HasCmdId(Cmd.SetInterface); }
        }

        public bool CanGetProperties
        {
            get { return HasCmdId(Cmd.GetProperties); }
        }

        public bool CanSetProperty
        {
            get { return HasCmdId(Cmd.SetProperty); }
        }

        public ControlPortClient(int protoVersion, MacAddress address, Version fwVersion)
            : this(null, null, null, protoVersion, address, fwVersion, null, null)
        {
        }

        public ControlPortClient(IPAddress localAdapter, IPAddress broadCastTarget, IPAddress ipAddress, int protoVersion, MacAddress address, Version fwVersion, string deviceName, string serialNumber)
            : base(localAdapter,broadCastTarget)
        {
            mIPAddress = ipAddress;
            mProtoVersion = protoVersion;
            mAddress = address;
            mFWVersion = fwVersion;
            mDeviceName = deviceName;
            mSerialNumber = serialNumber;

            if (ProtoVersion == 1)
                mProtoIDMap = CmdIDProto1;
            else if (ProtoVersion == 2)
                mProtoIDMap = CmdIDProto2;
            else
                throw new NotSupportedException("Protocol version " + protoVersion + " is not supported");
        }


        public static ControlPortClient[] DiscoverDevices(IPAddress localAdapter = null, IPAddress broadcastTarget = null)
        {
            byte[] discoverPacket = new byte[2];
            discoverPacket[0] = 0;
            discoverPacket[1] = 0x01;

            BroadcastDiscoverResult[] responses = DiscoverDevices(
                ClientPort, 
                discoverPacket, 
                packet => {
                    if (packet.Length < 53)
                        return BroadcastAddress.Invalid;

                    if (packet[1] != 0x81)  // discover reply
                        return BroadcastAddress.Invalid;

                    int protoVersion = packet[2];
                    if (!(protoVersion == 1 || protoVersion == 2)) // proto version 1 and 2 supported
                        return BroadcastAddress.Invalid;

                    return BroadcastAddress.ReadFromPacket(packet, 3, 6);
                },
                localAdapter,
                0,
                broadcastTarget);

            List<ControlPortClient> clients = new List<ControlPortClient>();

            foreach (BroadcastDiscoverResult response in responses)
            {
                byte[] inpacket = response.Data;
                int protoVersion = inpacket[2];

                MacAddress mac= new MacAddress(response.BroadcastAddress.Address);

                Version fwVersion = new Version(
                    inpacket[9],
                    inpacket[10],
                    inpacket[11],
                    inpacket[12]
                    );

                string deviceName = Helper.GetStringFromAscii(inpacket, 13, 20);
                string serialNumber = Helper.GetStringFromAscii(inpacket, 33, 20);

                try
                {
                    clients.Add(new ControlPortClient(response.LocalAddress, response.BroadcastTarget, response.SrcAddress.Address, protoVersion, mac, fwVersion, deviceName, serialNumber));
                }
                catch
                {

                }
            }

            return clients.ToArray();
        }

        public bool Reset(bool enterBootloader)
        {
            byte[] msg = CreateMessage(Cmd.Reset, 9);
            msg[8] = (byte)(enterBootloader ? 1 : 0);

            byte[] dummy = null;
            return SendMessage(Cmd.Reset, msg, out dummy);
        }

        public bool GetInterface(out InterfaceProperties props)
        {
            props = new InterfaceProperties();

            byte[] msg = CreateMessage(Cmd.GetInterface, 8);
            byte[] response = null;

            if (!SendMessage(Cmd.GetInterface, msg, out response))
                return false;

            if (response.Length < 21)
                return false;

            try
            {
                props.Address = new IPAddress(Helper.GetSubArray(response, 8, 4));
                props.Netmask = new IPAddress(Helper.GetSubArray(response, 12, 4));
                props.Gateway = new IPAddress(Helper.GetSubArray(response, 16, 4));
            }
            catch
            {
                return true;    // malformed ip somewhere
            }

            return true;
        }

        public bool SetInterface(InterfaceProperties props)
        {
            byte[] msg = CreateMessage(Cmd.SetInterface, 20);
            Array.Copy(GetAddressBytes(props.Address), 0, msg, 8, 4);
            Array.Copy(GetAddressBytes(props.Netmask), 0, msg, 12, 4);
            Array.Copy(GetAddressBytes(props.Gateway), 0, msg, 16, 4);

            byte[] dummy = null;
            return SendMessage(Cmd.SetInterface, msg, out dummy);
        }

        private byte[] GetAddressBytes(IPAddress address)
        {
            return address.GetAddressBytes().Reverse().ToArray();
        }

        public IProperty[] GetProperties()
        {
            byte[] msg = CreateMessage(Cmd.GetProperties, 8);
            byte[] response;
            if (!SendMessage(Cmd.GetProperties, msg, out response))
                return null;

            List<IProperty> properties = new List<IProperty>();
            int numProps = response[8];
            int currPos = 9;
            for (int i = 0; i < numProps; ++i)
            {
                if (currPos + 1 >= response.Length)
                    break;  // out of range

                int propID = response[currPos++];
                int propLen = response[currPos++];
                IProperty prop = PropertyFactory.CreateProperty(propID);
                if (prop == null)
                    prop = new UnknownProperty(propID, propLen, "unknown id");
                else if (prop.RawLength != propLen)
                    prop = new UnknownProperty(propID, propLen, "invalid len");

                if ((currPos + propLen) > response.Length)
                    break;  // out of range

                prop.SetRawValue(Helper.GetSubArray(response, currPos, prop.RawLength));

                currPos += prop.RawLength;

                properties.Add(prop);
            }

            return properties.ToArray();
        }

        public bool SetProperty(IProperty prop)
        {
            if (prop == null)
                throw new ArgumentNullException("prop");

            int totalLen = 9 + prop.RawLength;

            byte[] msg = CreateMessage(Cmd.SetProperty, totalLen);
            msg[8] = (byte)prop.ID;
            Array.Copy(prop.GetRawValue(), 0, msg, 9, prop.RawLength);

            byte[] result;
            if (!SendMessage(Cmd.SetProperty, msg, out result))
                return false;

            return result.Length > 8 && result[8] != 0;
        }

        public bool TriggerUpdate(FwComponent component, FwUpdateMode updateMode)
        {
            byte[] msg = CreateMessage(Cmd.TriggerUpdate, 54);
            msg[8] = (byte)component;
            Converter.FillUInt32(0,msg,9,false);

            if (updateMode == FwUpdateMode.Incremental)
            {
                switch (component)
                {
                    case FwComponent.LedBus:
                        Converter.FillUInt32(0x45A3F129, msg, 9, false);
                        break;
                }
            }

            msg[13] = (byte)((updateMode == FwUpdateMode.Full) ? 0 : 1);
            Converter.FillUInt64(Time.GetUnixTimeStamp(DateTime.Now), msg, 14, false);
            Helper.PutStringIntoAscii(msg, 22, 16, Environment.MachineName);
            Helper.PutStringIntoAscii(msg, 38, 16, Environment.UserName);

            byte[] result;
            // we use a bigger timeout value since the update process can take some time
            if (!SendMessage(Cmd.TriggerUpdate, msg, out result, 4000))
                return false;

            return result.Length > 8 && result[8] != 0;
        }

        public bool FactoryReset(FactoryResetFlags flags)
        {
            byte[] msg = CreateMessage(Cmd.FactoryReset, 49);
            msg[8] = (byte)flags;

            Converter.FillUInt64(Time.GetUnixTimeStamp(DateTime.Now), msg, 9, false);
            Helper.PutStringIntoAscii(msg, 17, 16, Environment.MachineName);
            Helper.PutStringIntoAscii(msg, 33, 16, Environment.UserName);

            byte[] result;
            if (!SendMessage(Cmd.FactoryReset, msg, out result))
                return false;

            return true;
        }

        public ErrorEntry[] ReadErrors(int maxErrors= 64)
        {
            List<ErrorEntry> errors = new List<ErrorEntry>();
            while(errors.Count < maxErrors)
            {
                ErrorEntry error= ReadError();
                if (error == null)
                    break;
                errors.Add(error);
            }

            return errors.ToArray();
        }

        private ErrorEntry ReadError()
        {
            byte[] msg = CreateMessage(Cmd.GetError, 8);
            byte[] response;
            if (!SendMessage(Cmd.GetError, msg, out response))
                return null;

            if (response.Length < 21)   // invalid packet
                return null;

            byte errorCode = response[8];
            if (errorCode == 0) // no further errors
                return null;

            UInt32 errorAge = Converter.GetUInt32(response, 9, false);
            DateTime timeStamp= DateTime.Now - new TimeSpan(0,0,(int)errorAge);

            switch (errorCode & 0xF0)
            {
                case 0x20:  // ledbus
                    {
                        int ledID= Converter.GetUInt16(response,13,false);
                        return new LedErrorEntry(timeStamp, errorCode, ledID);
                    }
                case 0x30:  // lightgrid
                    {
                        return new LightGridErrorEntry(timeStamp, errorCode);
                    }
                default:
                    {
                        string errorText = String.Format("Unknown error 0x{0:X2}", errorCode);
                        return new ErrorEntry(timeStamp, errorCode, errorText);
                    }
            }
        }

        public LogEntry[] ReadLogs()
        {
            int id = 0;
            List<LogEntry> logs = new List<LogEntry>();
            while (true)
            {
                var log = ReadLogLine(id);
                if (log != null)
                    logs.Add(log);
                else
                    return logs.ToArray();

                ++id;
            }
        }

        private LogEntry ReadLogLine(int id)
        {
            byte[] msg = CreateMessage(Cmd.GetLog, 10);
            Converter.FillUInt16((ushort)id, msg, 8, false);

            byte[] response;
            if (!SendMessage(Cmd.GetLog, msg, out response, 1000))
                return null;

            if (response.Length < 73) // invalid packet length
                return null;

            byte success = response[8];
            if (success == 0)
                return null;

            UInt64 timeStamp = Converter.GetUInt64(response, 9, false);
            String message = Helper.GetStringFromAscii(response, 17, 56);
            String timeStr;
            DateTime time;

            if ((timeStamp & 0x8000000000000000UL) != 0)
            {
                // internal timestamp, combination of power cycle count and uptime
                UInt32 uptimeSec = (UInt32)(timeStamp & 0xFFFFFFFFF);
                UInt32 powerCycleCount = (UInt32)((timeStamp >> 32) & 0x7FFFFFFF);
                TimeSpan upTime= new TimeSpan(0,0,(int)uptimeSec);

                timeStr = String.Format("{0} pcycle, {1:f1} hrs", powerCycleCount, upTime.TotalHours);
                time = DateTime.MinValue;
            }
            else
            {
                time = Time.GetFromUnixTimeStamp(timeStamp);
                timeStr= time.ToString();
            }

            return new LogEntry
            {
                Time = time,
                Message = String.Format("{0}: {1}",timeStr,message)
            };
        }

        public byte[] ReadEEProm(int addr, int numBytes)
        {
            int retries = 0;
            int maxRetries = 3;

            // we can read max 128 bytes at once from the device
            List<byte[]> blocks = new List<byte[]>();

            if (numBytes < 0)
                numBytes = int.MaxValue;

            while (numBytes > 0)
            {
                byte bytes2read = (byte)(numBytes > 128 ? 128 : numBytes);
                var block = ReadEEPromBlock(addr, bytes2read);
                if (block == null)
                {
                    if (retries < maxRetries)
                    {
                        ++retries;
                        continue;
                    }
                    else
                        return null;
                }
                retries = 0;

                blocks.Add(block);

                numBytes -= block.Length;
                addr += block.Length;

                if (block.Length == 0)  // eof eeprom
                    break;
            }

            return blocks.SelectMany(b => b).ToArray();
        }

        private byte[] ReadEEPromBlock(int addr, byte numBytes)
        {
            byte[] msg = CreateMessage(Cmd.ReadEEProm,13);
            Converter.FillUInt32((UInt32)addr, msg, 8, false);
            msg[12] = numBytes;

            byte[] response;
            if (!SendMessage(Cmd.ReadEEProm, msg, out response))
                return null;

            if (response.Length < 9)
                return null;

            byte readBytes = response[8];
            byte[] data = new byte[readBytes];

            if (response.Length < (9 + readBytes))
                return null;

            Array.Copy(response, 9, data, 0, readBytes);

            return data;
        }

        public int WriteEEProm(int addr, byte[] data)
        {
            int len = data.Length;
            int dataOffset= 0;
            while (len > 0)
            {
                byte bytes2write= (byte)(len > 128 ? 128 : len);
                byte written = WriteEEPromBlock(addr, data, dataOffset, bytes2write);

                if (written == 0)   // eof eeprom
                    break;

                len -= written;
                dataOffset += written;
                addr += written;
            }

            return data.Length - len;
        }

        private byte WriteEEPromBlock(int addr, byte[] data, int dataOffset, byte numBytes)
        {
            if (numBytes > 128)
                numBytes = 128;

            if (data.Length - dataOffset < numBytes)
                numBytes = (byte)(data.Length - dataOffset);

            byte[] msg = CreateMessage(Cmd.WriteEEProm, 13 + numBytes);
            Converter.FillUInt32((UInt32)addr, msg, 8, false);
            msg[12] = numBytes;

            Array.Copy(data, dataOffset, msg, 13, numBytes);

            byte[] response;
            if (!SendMessage(Cmd.WriteEEProm, msg, out response,2000))
                return 0;

            if (response.Length < 9)
                return 0;

            byte writtenBytes = response[8];
            return writtenBytes;
        }

        public Process[] ReadProcesses()
        {
            try
            {
                var props = GetProperties();
                if (props == null)
                    return null;

                var eepromSizeProp= props.OfType<EEPromSizeProperty>().FirstOrDefault();
                if (eepromSizeProp == null)
                    return null;

                UInt32 headerPos = 0;
                List<Process> processes = new List<Process>();
                while (true)
                {
                    if (headerPos > eepromSizeProp.EEPromSize)
                        return null;    // malformed eeprom

                    Process newProc = ReadNextProcessHeader(headerPos, out headerPos);
                    if (newProc == null) // timeout or malformed message
                        return null;

                    if (String.IsNullOrWhiteSpace(newProc.Name))
                        break;  // end of list reached

                    processes.Add(newProc);
                    if (newProc.ByteSize == 0)  // malformed process
                        break;
                }

                return processes.ToArray();
            }
            catch
            {
                return null;
            }
        }

        private Process ReadNextProcessHeader(UInt32 headerPos, out UInt32 nextHeaderPos)
        {
            byte[] msg = CreateMessage(Cmd.GetNextProcessHeader, 12);
            Converter.FillUInt32(headerPos, msg, 8, false);

            byte[] response;
            if (!SendMessage(Cmd.GetNextProcessHeader, msg, out response) || response.Length < 43)
            {
                nextHeaderPos = 0;
                return null;
            }

            string name = Helper.GetStringFromAscii(response, 8, 21);
            byte procId = response[29];
            byte optionFlags = response[30];
            headerPos = Converter.GetUInt32(response, 31, false);
            UInt32 byteSize = Converter.GetUInt32(response, 35, false);
            nextHeaderPos = Converter.GetUInt32(response, 39, false);

            return new Process((int)headerPos, name, procId, optionFlags, (int)byteSize);
        }

        public bool DeleteProcess(string name)
        {
            byte[] msg = CreateMessage(Cmd.DeleteProcess, 29);
            Helper.PutStringIntoAscii(msg, 8, 21, name);

            byte[] response;
            if (!SendMessage(Cmd.DeleteProcess, msg, out response,5000) || response.Length < 9)
                return false;

            return response[8] == 1;
        }

        public bool ReserveProcessSpace(string name, UInt32 fullByteSize, out UInt32 newHeaderPos)
        {
            byte[] msg = CreateMessage(Cmd.ReserveProcessSpace, 33);
            Helper.PutStringIntoAscii(msg, 8, 21, name);

            Converter.FillUInt32(fullByteSize, msg, 29, false);

            byte[] response;
            if (!SendMessage(Cmd.ReserveProcessSpace, msg, out response) || response.Length < 13)
            {
                newHeaderPos = 0;
                return false;
            }

            newHeaderPos = Converter.GetUInt32(response, 9, false);
            return response[8] != 0;
        }

        private bool HasCmdId(Cmd cmd)
        {
            return mProtoIDMap[(int)cmd] != 0;
        }

        private byte GetCmdId(Cmd cmd, bool reply)
        {
            return (byte)(mProtoIDMap[(int)cmd] | (reply ? mProtoIDMap[0] : 0));
        }

        private byte[] CreateMessage(Cmd cmd, int totalLen)
        {
            byte[] msg = new byte[totalLen];
            msg[0] = mNextPacketID;
            ++mNextPacketID;
            msg[1] = GetCmdId(cmd, false);
            if (msg[1] == 0)
                throw new NotSupportedException(String.Format("The command {0} is not supported by the device protocol", cmd));

            for (int i = 0; i < 6; ++i)
                msg[2 + i] = Address.Bytes[i];

            return msg;
        }

        private bool SendMessage(Cmd cmd, byte[] msg, out byte[] response, int timeoutMilli= -1)
        {
            byte responseID = GetCmdId(cmd, true);
            return SendPacket(
                ClientPort,
                msg,
                out response,
                r =>
                {
                    if (r.Length < 8)   // not too short
                        return false;

                    if (r[1] != responseID) // not the requested response
                        return false;

                    for (int i = 0; i < 6; ++i)     // mac address equal
                        if (r[2 + i] != msg[2 + i])
                            return false;

                    return true;
                },
                timeoutMilli);
        }

        public override string ToString()
        {
            return String.Format("{0} - {1} - {2}", SerialNumber, Address, FWVersion);
        }
    }
}
