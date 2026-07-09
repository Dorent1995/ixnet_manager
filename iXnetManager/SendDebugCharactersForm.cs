using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iXnet.InputHub;

namespace iXnetManager
{
    public partial class SendDebugCharactersForm : Form
    {
        private CharacterDevice mCharDevice;
        public iXnet.InputHub.CharacterDevice CharDevice
        {
            get { return mCharDevice; }
        }

        public SendDebugCharactersForm(CharacterDevice charDevice)
        {
            InitializeComponent();

            mCharDevice = charDevice;

            Text = String.Format("Debug Text to {0}", charDevice.Name);
        }

        private void SendDebugText()
        {
            string sendText = mTBDebugText.Text;
            if (mCBAppendNewline.Checked)
                sendText += "\r\n";

            mCharDevice.SendDebugCharacters(sendText);
            mTBDebugText.Text = String.Empty;
        }

        private void mBTNSend_Click(object sender, EventArgs e)
        {
            SendDebugText();
        }

        private void mBTNClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
