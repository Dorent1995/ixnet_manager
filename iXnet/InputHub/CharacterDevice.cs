using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace iXnet.InputHub
{
    [Serializable]
    public class CharacterDevice : BaseHubDevice
    {
        public override bool IsEventDevice
        {
            get { return true; }
        }

        public event CharacterReceivedHandler CharReceived;
        protected virtual void OnCharReceived(char[] chars)
        {
            CharacterReceivedHandler handler = CharReceived;
            if (handler != null)
                handler(this, new CharacterReceivedEventArgs(chars));
        }

        public CharacterDevice(
            InputHubClient client,
            IPAddress localAddress,
            IPEndPoint endPoint,
            UniqueDevID deviceID,
            Version swVersion,
            string name,
            string devTypeName,
            bool isDirectInput,
            bool acquired)
            : base(client, localAddress, endPoint, deviceID, swVersion, name, devTypeName, isDirectInput, acquired)
        {}

        internal void RaiseOnCharReceived(char[] chars)
        {
            OnCharReceived(chars);
        }

        public bool SendDebugCharacters(string chars)
        {
            if (!SupportsDebugSend)
                throw new NotSupportedException("Debug Sending is not supported by the device");

            return Client.SendCharacterDebug(DeviceID.DevID, chars);
        }

        public override string ToString()
        {
            return String.Format("{0} (Character)", Name);
        }
    }
}
