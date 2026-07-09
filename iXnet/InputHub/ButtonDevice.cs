using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace iXnet.InputHub
{
    public class ButtonDevice : BaseHubDevice
    {
        public override bool IsEventDevice
        {
            get { return true; }
        }

        private int mNumButtons;
        public int NumButtons
        {
            get { return mNumButtons; }
        }

        public event ButtonReceivedHandler ButtonChangeReceived;
        protected virtual void OnButtonReceived(ButtonState newState, int buttonID)
        {
            ButtonReceivedHandler handler = ButtonChangeReceived;
            if (handler != null)
                handler(this, new ButtonReceivedEventArgs(newState, buttonID));
        }

        public ButtonDevice(
             InputHubClient client,
             IPAddress localAddress,
             IPEndPoint endPoint,
             UniqueDevID deviceID,
             Version swVersion,
             string name,
             string devTypeName,
             bool isDirectInput,
             int numButtons,
             bool acquired)
            : base(client, localAddress, endPoint, deviceID, swVersion, name, devTypeName, isDirectInput, acquired)
        {
            mNumButtons = numButtons;
        }

        internal void RaiseButtonChangeReceived(ButtonState newState, int buttonID)
        {
            OnButtonReceived(newState, buttonID);
        }

        public override string ToString()
        {
            return String.Format("{0} (Button)",Name);
        }
    }
}
