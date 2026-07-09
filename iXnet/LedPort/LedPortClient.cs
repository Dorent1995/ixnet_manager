using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;

namespace iXnet.LedPort
{
    public class LedPortClient : ServiceClientBase
    {
        private static readonly UInt16 LedServicePort = 2000;
        private byte mPacketID;

        private Version mSWVersion;
        public System.Version SWVersion
        {
            get { return mSWVersion; }
        }

        private int mLedCount;
        public int LedCount
        {
            get { return mLedCount; }
        }

        private int mDigitCount;
        public int DigitCount
        {
            get { return mDigitCount; }
        }

    	public bool HasDigits
	    {
            get { return mDigitCount > 0; }
	    }

        public LedPortClient(IPAddress localAddress, IPAddress deviceAddress, Version swVersion, int digitCount, int ledCount)
            : base(localAddress,deviceAddress,LedServicePort)
        {
            mPacketID = 0;
            mSWVersion = swVersion;
            mDigitCount = digitCount;
            mLedCount = ledCount;
        }


        #region Commands

        public bool ClearAllLeds()
        {
            byte[] msg = CreateNewMessage(0x6c);
            return SendMessage(msg);
        }

        public bool ResetLedBus()
        {
            byte[] msg = CreateNewMessage(0x65);
            return SendMessage(msg);
        }

        public bool ResetCalibrateLedBus()
        {
            byte[] msg = CreateNewMessage(0x63);
            return SendMessage(msg);
        }

        public bool SetLedState(int ledID, LedMode mode, LedColor color, string text)
        {
            if (ledID >= LedCount)
                throw new ArgumentOutOfRangeException("ledID");
            if (ledID < 0)
            {
                if (SWVersion < new Version(2, 0))
                    return SetAllLedStatesIndividual(mode, color, text);

                ledID = 0xFFFF; // all led states
            }

            LedMode ledFlags = mode & LedMode.LedFlagAllFlags;
            mode = mode & ~LedMode.LedFlagAllFlags;

            byte[] msg = CreateNewMessage(0x6e);

            msg[2] = (byte)(ledID & 0xFF);
            msg[3] = (byte)((ledID >> 8) & 0xFF);
            msg[4] = (byte)2;   // both banks
            msg[5] = EncodeModeAndColor(mode, color);
            if (mode != LedMode.LedOff)
            {
                msg[6] = color.R;
                msg[7] = color.G;
                msg[8] = color.B;
            }
            else
            {
                msg[6] = 0;
                msg[7] = 0;
                msg[8] = 0;
            }

            if (HasDigits)
            {
                int textLen = text == null ? 0 : text.Length;
                int textPos = textLen - 1;
                byte pointMask = EncodeCharTo7Segment('.');

                for (int i= 0; i < DigitCount; ++i)
                {
                    msg[9 + i] = 0;
                }
                for (int i = 0; (i < DigitCount) && (textPos >= 0);--textPos)
                {
                    if (text[textPos] == '.')
                    {
                        msg[9 + i] = EncodeCharTo7Segment('.');
                        if (((textPos - 1) >= 0) && (text[textPos - 1] == '.'))
                            ++i;
                    }
                    else
                    {
                        msg[9 + i] |= EncodeCharTo7Segment(text[textPos]);
                        ++i;
                    }
                }

            }

            msg[15] = ConvertLedFlags(ledFlags);

            return SendMessage(msg);
        }

        private byte ConvertLedFlags(LedMode ledFlags)
        {
            byte flags = 0x00;
            if (ledFlags.HasFlag(LedMode.LedFlagNoClearOnButtonPress))
                flags |= 0x01;

            if (ledFlags.HasFlag(LedMode.LedFlagClearIRCalibration))
                flags |= 0x02;

            return flags;
        }

        private bool SetAllLedStatesIndividual(LedMode mode, LedColor color, string text)
        {
            // currently there is no packet which sets all leds
            // we accept 3 % loss, because of possible wlan connections
            int maxFailCount = (int)Math.Round((float)LedCount * 0.03f);

            int failedCount = 0;
            for (int i = 0; i < LedCount; ++i)
            {
                if (!SetLedState(i, mode, color, text))
                {
                    ++failedCount;
                    if (failedCount > maxFailCount)
                        return false;
                }
            }

            return true;
        }

        // table from http://en.wikipedia.org/wiki/Seven-segment_display_character_representations
        // latin alphabet
        private static Dictionary<char, byte> mCodeTable = new Dictionary<char, byte>()
            {
                { '0' , 0x3F },
                { '1' , 0x06 },
                { '2' , 0x5B },
                { '3' , 0x4F },
                { '4' , 0x66 },
                { '5' , 0x6D },
                { '6' , 0x7D },
                { '7' , 0x07 },
                { '8' , 0x7F },
                { '9' , 0x6F },
                { 'A' , 0x77 },
                { 'B' , 0x7F },
                { 'C' , 0x39 },
                { 'D' , 0x3F },
                { 'E' , 0x79 },
                { 'F' , 0x71 },
                { 'G' , 0x3D },
                { 'H' , 0x76 },
                { 'I' , 0x06 },
                { 'J' , 0x1E },
                { 'K' , 0x7A },
                { 'L' , 0x38 },
                { 'M' , 0x55 },
                { 'N' , 0x37 },
                { 'O' , 0x3F },
                { 'P' , 0x73 },
                { 'Q' , 0x67 },
                { 'R' , 0x31 },
                { 'S' , 0x6D },
                { 'T' , 0x07 },
                { 'U' , 0x3E },
                { 'V' , 0x2A },
                { 'W' , 0x2A },
                { 'X' , 0x76 },
                { 'Y' , 0x6E },
                { 'Z' , 0x5B },
                { 'a' , 0x5F },
                { 'b' , 0x7C },
                { 'c' , 0x58 },
                { 'd' , 0x5E },
                { 'e' , 0x7B },
                { 'f' , 0x71 },
                { 'g' , 0x6F },
                { 'h' , 0x74 },
                { 'i' , 0x06 },
                { 'j' , 0x0E },
                { 'k' , 0x75 },
                { 'l' , 0x30 },
                { 'm' , 0x15 },
                { 'n' , 0x54 },
                { 'o' , 0x5C },
                { 'p' , 0x73 },
                { 'q' , 0x67 },
                { 'r' , 0x50 },
                { 's' , 0x64 },
                { 't' , 0x78 },
                { 'u' , 0x1C },
                { 'v' , 0x1C },
                { 'w' , 0x1D },
                { 'x' , 0x49 },
                { 'y' , 0x72 },
                { 'z' , 0x52 },
                { ' ' , 0x00 },
                { '-' , 0x40 },
                { '.' , 0x80 },
                { '!' , 0x82 },
            };


        private byte EncodeCharTo7Segment(char c)
        {
            byte code = 0x00;
            mCodeTable.TryGetValue(c, out code);
            return code;
        }

        private byte EncodeModeAndColor(LedMode mode, LedColor color)
        {
            byte enc;
            if (mode == LedMode.LedOff)
            {
                enc = 0;
            }
            else
            {
                byte r = (byte)(color.R >> 6);
                byte g = (byte)(color.G >> 6);
                byte b = (byte)(color.B >> 6);

                byte blink;
                switch (mode)
                {
                    case LedMode.LedOn:
                        blink = 3;
                        break;
                    case LedMode.LedBlinkSlow:
                        blink = 2;
                        break;
                    case LedMode.LedBlinkNormal:
                        blink = 1;
                        break;
                    case LedMode.LedBlinkFast:
                        blink = 0;
                        break;
                    default:
                        blink = 3; // off
                        break;
                }

                enc = (byte)((r << 6) | (g << 4) | (b << 2) | blink);
            }
            return enc;
        }

        private byte[] CreateNewMessage(byte cmd)
        {
            byte[] msg = new byte[20];
            msg[0] = mPacketID;
            msg[1] = cmd;
            ++mPacketID;

            return msg;
        }

        private bool SendMessage(byte[] msg)
        {
            byte[] response;
            return SendUdpPacket(out response, msg);
        }

        #endregion
    }
}
