using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using iXnetManager.Controls;
using iXnetManager.Theme;

namespace iXnetManager
{
    public partial class AddDeviceForm : BaseChromeForm
    {
        public IPAddress LocalAdapterIP { get; set; }
        public IPAddress RemoteAddress { get; set; }

        public AddDeviceForm()
        {
            LocalAdapterIP = IPAddress.Any;
            RemoteAddress = IPAddress.Broadcast;

            InitializeComponent();
            InstallChrome(resizable: false, showMinimize: false, showMaximize: false, showThemeToggle: false);
            ThemeApplier.Apply(this, mBtnSearch);
        }

        protected override void OnShown(EventArgs e)
        {
            SetupView();
            base.OnShown(e);
        }

        private void SetupView()
        {
            mTBLocalAdapter.Text = LocalAdapterIP.ToString();
            mTBIPAddress.Text = RemoteAddress.ToString();
        }

        private void mBtnSearch_Click(object sender, EventArgs e)
        {
            IPAddress localAdapter;
            IPAddress remoteAddress;
            if (!ParseIPAddress(mTBLocalAdapter.Text, out localAdapter))
                return;
            if (!ParseIPAddress(mTBIPAddress.Text, out remoteAddress))
                return;

            LocalAdapterIP= localAdapter;
            RemoteAddress= remoteAddress;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool ParseIPAddress(string ipText, out IPAddress address)
        {
            if (!IPAddress.TryParse(ipText, out address))
            {
                MessageBox.Show(String.Format("Invalid IP Address '{0}'", ipText), "IP Check", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

    }
}
