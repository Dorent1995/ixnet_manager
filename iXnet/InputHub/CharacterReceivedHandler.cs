using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iXnet.InputHub
{
    public delegate void CharacterReceivedHandler(object sender, CharacterReceivedEventArgs args);

    public class CharacterReceivedEventArgs : EventArgs
    {
        private char[] mReceivedChars;
        public char[] ReceivedChars
        {
            get { return mReceivedChars; }
        }

        public CharacterReceivedEventArgs(char[] receivedChars)
        {
            mReceivedChars = receivedChars;
        }

        public override string ToString()
        {
            return new String(ReceivedChars);
        }
    }
}
