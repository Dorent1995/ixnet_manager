using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.InputHub
{
    public delegate void ButtonReceivedHandler(object sender, ButtonReceivedEventArgs args);

    public class ButtonReceivedEventArgs : EventArgs
    {
        private ButtonState mNewState;
        public ButtonState NewState
        {
            get { return mNewState; }
        }

        private int mButtonID;
        public int ButtonID
        {
            get { return mButtonID; }
        }

        public ButtonReceivedEventArgs(ButtonState newState, int buttonID)
        {
            mNewState= newState;
            mButtonID = buttonID;
        }

        public override string ToString()
        {
            return String.Format("{0}:{1}", ButtonID, NewState);
        }
    }
}
