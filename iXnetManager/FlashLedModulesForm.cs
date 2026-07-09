using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iXnet.ControlPort;

namespace iXnetManager
{
    internal partial class FlashLedModulesForm : Form
    {
        private FwUpdateMode mLastUpdateMode = FwUpdateMode.Full;

        private NetDevice mNetDevice;
        public iXnetManager.NetDevice NetDevice
        {
            get { return mNetDevice; }
        }

        private Timer mLedUpdateTimer;


        public FlashLedModulesForm(NetDevice netDevice)
        {
            if (netDevice == null)
                throw new ArgumentNullException();

            mNetDevice = netDevice;
            InitializeComponent();

            Text = String.Format("Update Led Modules on {0}", netDevice.SerialNumber);
        }

        private void SetupLedCount()
        {
            SetLedCount(0);

            mLedUpdateTimer = new Timer();
            mLedUpdateTimer.Interval = 1000;
            mLedUpdateTimer.Tick += new EventHandler(mLedUpdateTimer_Tick);
            mLedUpdateTimer.Start();
        }

        private void DestroyLedCount()
        {
            mLedUpdateTimer.Dispose();
        }

        void mLedUpdateTimer_Tick(object sender, EventArgs e)
        {
            mLedUpdateTimer.Stop();

            System.Threading.ThreadPool.QueueUserWorkItem((state) =>
                {
                    mNetDevice.Device.UpdateProperties();
                    if (!this.IsDisposed)
                        this.BeginInvoke(new MethodInvoker(() =>
                        {
                            SetLedCount(mNetDevice.Device.ActiveLedCount);
                            mLedUpdateTimer.Start();
                        }));
                });
        }

        private void SetLedCount(int number)
        {
            mLBLLedCount.Text = number.ToString();
        }

        private void StartUpdate()
        {
            FwUpdateMode updateMode = mRBFull.Checked ? FwUpdateMode.Full : FwUpdateMode.Incremental;
            mBTNStartUpdate.Enabled = false;

            if (!NetDevice.TriggerLedBusFwUpdate(updateMode))
                MessageBox.Show("Failed to update led modules!\nCheck connection on first device.", "Update Led Modules", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                mLastUpdateMode = updateMode;

            mBTNStartUpdate.Enabled = true;
        }

        private void mBTNStartUpdate_Click(object sender, EventArgs e)
        {
            StartUpdate();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (e.Cancel)
                return;

            if (mLastUpdateMode == FwUpdateMode.Incremental && Properties.Settings.Default.SecurityEnabled == true)
            {
                MessageBox.Show("Last update was done incremental. Please run a full update!", "Security Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            SetupLedCount();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            DestroyLedCount();
        }
    }
}
