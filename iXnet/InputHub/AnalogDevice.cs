using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace iXnet.InputHub
{
    public class AnalogDevice : BaseHubDevice
    {
        public override bool IsEventDevice
        {
            get { return false; }
        }

        private UInt32 mMaxValue;
        public System.UInt32 MaxValue
        {
            get { return mMaxValue; }
        }

        private UInt16 mNumChannels;
        public UInt16 NumChannels
	    {
		    get { return mNumChannels; }
    	}

        public AnalogDevice(
            InputHubClient client,
            IPAddress localAddress,
            IPEndPoint endPoint,
            UniqueDevID deviceID,
            Version swVersion,
            string name,
            string devTypeName,
            bool isDirectInput,
            UInt32 maxValue,
            UInt16 numChannels
            )
            : base(client, localAddress, endPoint, deviceID, swVersion, name, devTypeName, isDirectInput, false)
        {
            mMaxValue = maxValue;
            mNumChannels = numChannels;
        }

        public UInt32 ReadIntValue(UInt16 channel)
        {
            return Client.ReadAnalogValue(DeviceID.DevID, channel);
        }

        /// <summary>
        /// Read the analog value as float
        /// </summary>
        /// <returns>A value between 0 and 1, relative to max value</returns>
        public float ReadFloatValue(UInt16 channel)
        {
            UInt32 currVal = ReadIntValue(channel);
            return (float)((double)currVal / (double)mMaxValue);
        }

        public override string ToString()
        {
            return String.Format("{0} (Analog)", Name);
        }
    }
}
