using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using iXnet.ControlPort;
using iXnetManager.Controls;
using iXnetManager.Theme;

namespace iXnetManager
{
    partial class IPRangeAssignForm : BaseChromeForm
    {
        private IEnumerable<NetDevice> mDevices;
        public IEnumerable<NetDevice> Devices
        {
            get { return mDevices; }
            set { mDevices = value; }
        }

        public IPRangeAssignForm()
        {
            InitializeComponent();
            InstallChrome(resizable: false, showMinimize: false, showMaximize: false, showThemeToggle: false);
            ThemeApplier.Apply(this, mBTNAssign);

            mTBNetwork.TextChanged += new EventHandler(mTBNetwork_TextChanged);
            mTBStartIP.TextChanged += new EventHandler(mTBStartIP_TextChanged);
        }

        private void ShowError(string errorMsg)
        {
            MessageBox.Show(errorMsg, "Assign IP Range", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (Devices == null || Devices.Count() == 0)
            {
                Close();
                return;
            }

            if (Devices.Count() > 255)
            {
                ShowError("Cannot assign IP Address Range to more than 255 devices");
                Close();
                return;
            }

            mTBNetwork.Text = "192.168.1.255";
            mTBStartIP.Text = "1";
            mTBGateway.Text = "0.0.0.0";

            base.OnLoad(e);
        }

        private void UpdateIPMask()
        {
            string netMask;
            IPAddress network = ParseNetwork();
            if (network == IPAddress.None)
                netMask= String.Empty;
            else
            {
                byte[] addressBytes = network.GetAddressBytes();
                netMask= String.Format("{0}.{1}.{2}.", addressBytes[0], addressBytes[1], addressBytes[2]);
            }

            mTBStartIPMask.Text = netMask;
            mTBEndIPMask.Text = netMask;
        }

        private IPAddress ParseNetwork()
        {
            return ParseIPAddress(mTBNetwork.Text);
        }

        private IPAddress ParseIPAddress(string address)
        {
            IPAddress netAddress;
            if (IPAddress.TryParse(address, out netAddress))
                return netAddress;

            return IPAddress.None;
        }

        private void UpdateEndIP()
        {
            string startIPText = mTBStartIP.Text;
            int startIP;
            string endIPText;
            if (int.TryParse(startIPText, out startIP))
            {
                int endIP = startIP + Devices.Count() - 1;
                if (endIP >= 255)
                    endIPText = "ERR";
                else
                    endIPText = endIP.ToString();
            }
            else
                endIPText = String.Empty;

            mTBEndIP.Text = endIPText;
        }

        private bool AssignIPRange()
        {
            IPAddress network = ParseNetwork();
            if (network == IPAddress.None)
            {
                ShowError("Network is invalid, expected something like 192.168.1.255");
                return false;
            }

            int startIP;
            if (int.TryParse(mTBStartIP.Text, out startIP))
            {
                if (startIP <= 0 || startIP >= 255)
                {
                    ShowError("Start ip is out of range, must be in range of 1 to 254");
                    return false;
                }
                int endIP = startIP + Devices.Count() - 1;
                if (endIP >= 255)
                {
                    ShowError(String.Format("Start ip value is too high, end ip would be {0} which is out of range!", endIP));
                    return false;
                }
            }

            IPAddress gateway = ParseIPAddress(mTBGateway.Text);

            return AssignIPRange(network, startIP, gateway);
        }

        private bool AssignIPRange(IPAddress network, int startIP, IPAddress gateway)
        {
            if (startIP < 1)
                return false;

            IPAddress netmask = IPAddress.Parse("255.255.255.0");
            int endIP= startIP + Devices.Count() - 1;
            if (endIP >= 255)
                return false;   // out of range

            int currIP = startIP;
            byte[] ipBytes = network.GetAddressBytes();

            foreach (NetDevice dev in Devices.OrderBy(d => d.SerialNumber))
            {
                ipBytes[3] = (byte)currIP;
                InterfaceProperties iProps = new InterfaceProperties();
                iProps.Address = new IPAddress(ipBytes);
                iProps.Netmask = netmask;
                iProps.Gateway = gateway;

                dev.SetInterfaceProperties(iProps);
                ++currIP;
            }

            return true;
        }

        void mTBStartIP_TextChanged(object sender, EventArgs e)
        {
            UpdateEndIP();
        }

        void mTBNetwork_TextChanged(object sender, EventArgs e)
        {
            UpdateIPMask();
        }

        private void mBTNAssign_Click(object sender, EventArgs e)
        {
            if (AssignIPRange())
                Close();
        }

    }
}
