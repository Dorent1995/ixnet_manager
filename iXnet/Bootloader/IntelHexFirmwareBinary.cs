using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace iXnet.Bootloader
{
    /// <summary>
    /// Parses an Intel Hex format file for the firmware binary.
    /// This also parses the ixtech extended format which can include the device
    /// for which the firmware file is intended and the version of the
    /// firmware.
    /// The format extends with:
    /// #D device name
    /// #V version (4 digits)
    /// </summary>
    public class IntelHexFirmwareBinary : FirmwareBinary
    {
        protected override void OnParseFileFormat(System.IO.Stream file)
        {
            List<byte> fwBin = new List<byte>();
            int addrOffset = 0;
            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line.StartsWith(":"))
                    {
                        if (!ParseRecord(line, fwBin, ref addrOffset))
                            break;
                    }
                    else if (line.StartsWith("#D"))
                        ParseDeviceName(line);
                    else if (line.StartsWith("#V"))
                        ParseFWVersion(line);
                }
            }

            BinaryBytes = fwBin.ToArray();
        }

        private bool ParseRecord(string line,List<byte> fwBin, ref int addrOffset)
        {
            int recordType = ReadHexByte(line, 7);
            switch (recordType)
            {
                case 0:
                    return ParseRecordTyp0(line, fwBin, addrOffset);
                case 1:
                    return false;   // end of file record
                case 2:
                    return ParseRecordTyp2(line, fwBin, out addrOffset);
                default:
                    throw new FormatException("Unknown record in hex file !");
            }
        }

        private bool ParseRecordTyp2(string line, List<byte> fwBin, out int addrOffset)
        {
            int addrHigh = ReadHexByte(line, 9);
            int addrLow = ReadHexByte(line, 11);
            addrOffset = ((addrHigh << 8) | addrLow) * 16;

            return true;
        }

        private bool ParseRecordTyp0(string line, List<byte> fwBin, int addrOffset)
        {
            int byteCount = ReadHexByte(line, 1);
            int addrHigh = ReadHexByte(line, 3);
            int addrLow = ReadHexByte(line, 5);

            int addr = addrOffset + ((addrHigh << 8) | addrLow);

            for (int i = 0; i < byteCount; ++i)
            {
                byte data = ReadHexByte(line, 9 + (i * 2));
                AddByte(fwBin, addr + i, data);
            }
            return true;
        }

        private void ParseDeviceName(string line)
        {
            DeviceName = line.Substring(2).Trim();
        }

        private void ParseFWVersion(string line)
        {
            try
            {
                string vStr = line.Substring(2);
                FirmwareVersion = Version.Parse(vStr);
            }
            catch 
            {
                throw new FormatException("Cannot parse FW Version !");   	
            }
        }

        private void AddByte(List<byte> bin, int address, byte data)
        {
            if (bin.Count <= address)
            {
                for (int i = bin.Count; i < address; ++i)
                    bin.Add(0xFF);

                bin.Add(data);
            }
            else
                bin[address] = data;
        }

        private byte ReadHexByte(string line, int offset)
        {
            return (byte)Convert.ToInt32(line.Substring(offset, 2),16);
        }
    }
}
