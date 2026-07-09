using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iXnet;
using BrightIdeasSoftware;
using iXnet.Bootloader;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using iXnetManager.Controls;
using iXnetManager.Theme;

namespace iXnetManager
{
    public partial class FlashProgramMemoryForm : BaseChromeForm
    {
        private CancellationTokenSource mCancelSource;
        private bool mRunning;

        private FirmwareBinary mFirmware;
        public iXnet.Bootloader.FirmwareBinary Firmware
        {
            get { return mFirmware; }
            set 
            { 
                mFirmware = value;
                OnFirmwareChanged();
            }
        }

        public FlashProgramMemoryForm(IEnumerable<iXnetDevice> devices)
        {
            InitializeComponent();
            InstallChrome(resizable: true, showMinimize: true, showMaximize: true);
            ThemeApplier.Apply(this);

            BarRenderer render = new BarRenderer(0, 100);
            render.MaximumWidth = 10000;

            mCOLProgress.Renderer = render;
            olvColumn4.Width = 270;
            mOLVFlashStates.Refresh();

            mOLVFlashStates.FormatRow += new EventHandler<FormatRowEventArgs>(mOLVFlashStates_FormatRow);

            mOLVFlashStates.SetObjects(devices.Select(dev => new FlashFirmwareState(dev)).ToArray());

            Text = String.Format("Flashing {0} iXnet Devices", devices.Count());
            Firmware = null;

            mBTNStart.Enabled = true;
            mBTNClose.Enabled = true;
            mBTNAbort.Enabled = false;
        }

        void mOLVFlashStates_FormatRow(object sender, FormatRowEventArgs e)
        {
            FlashFirmwareState state = (FlashFirmwareState)e.Model;
            if (state.Error)
                e.Item.BackColor = Color.Red;
        }

        protected virtual void OnFirmwareChanged()
        {
            if (Firmware == null)
            {
                mLBLFWName.Text = "-";
                mLBLFWVersion.Text = "-";
                mLBLFWSize.Text = "-";
            }
            else
            {
                mLBLFWName.Text = Path.GetFileName(mFirmware.FileName);
                mLBLFWVersion.Text = mFirmware.FirmwareVersion.ToString();
                mLBLFWSize.Text = String.Format("{0} Bytes", mFirmware.BinaryBytes.Length);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (!SelectFirmware())
                Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (mRunning)
                e.Cancel = true;

            base.OnClosing(e);
        }

        private bool SelectFirmware()
        {
            using (OpenFileDialog opfDialog = new OpenFileDialog())
            {
                while (true)
                {
                    opfDialog.Filter = "ix.net FW(*.hex)|*.hex";
                    opfDialog.CheckFileExists = true;
                    if (opfDialog.ShowDialog() != DialogResult.OK)
                        return false;

                    try
                    {
                        IntelHexFirmwareBinary fwBin = new IntelHexFirmwareBinary();
                        fwBin.Parse(opfDialog.FileName);

                        if (fwBin.FirmwareVersion == new Version(0,0,0,0))
                        {
                            if (MessageBox.Show("Invalid firmware file !\nRetry other file ?",
                                "Select Firmware",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Error) == DialogResult.No)
                                return false;
                        }
                        else
                        {
                            Firmware = fwBin;
                            return true;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        if (MessageBox.Show(
                            String.Format("Failed to open firmware file !\nError: {0}\nRetry other file ?", ex.Message),
                            "Select Firmware",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Error) == DialogResult.No)
                            return false;
                    }
                }
            }
        }

        private void RefreshStates()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => RefreshStates()));
                return;
            }

            mOLVFlashStates.BuildList();

        }

        private void StartFlashing()
        {
            mBTNStart.Enabled = false;
            mBTNClose.Enabled = false;
            mBTNAbort.Enabled = true;

            mRunning = true;

            mCancelSource= new CancellationTokenSource();
            Task flashTask = new Task(() =>
            {
                try
                {
                    RunFlashing();
                }
                catch 
                {

                }
                finally
                {
                    mRunning = false;
                }
            }, mCancelSource.Token);
            flashTask.Start();
        }

        private void AbortFlashing()
        {
            mBTNAbort.Enabled = false;

            if (mCancelSource != null)
                mCancelSource.Cancel();
        }

        private void OnFlashingFinished()
        {
            mBTNStart.Enabled = true;
            mBTNClose.Enabled = true;
            mBTNAbort.Enabled = false;
        }

        private void RunFlashing()
        {
            var states = mOLVFlashStates.Objects.Cast<FlashFirmwareState>();

            foreach (FlashFirmwareState state in states)
            {
                if (state.Device.HasControlPort)
                {
                    state.Device.ControlPort.Reset(true);
                }
                else
                {
                    state.Device.Bootloader.Reset(true);
                }

                state.State = "Resetting to Bootloader ...";
            }

            RefreshStates();
            Thread.Sleep(1500);

            iXnetDevice[] devices = iXnetDevice.DiscoverDevices(true);

            // sync devices where bootloader was entered
            foreach (FlashFirmwareState state in states)
            {
                var newDev = devices.FirstOrDefault(dev => dev.MacAddress == state.Device.MacAddress);
                if (newDev == null || !newDev.HasBootloader)
                {
                    state.State = "Failed to switch to bootloader !";
                    state.Error = true;
                }
                else
                {
                    state.State = "Entered Bootloader";
                    state.Device = newDev;
                }
                state.OverallProgress = 10;
            }

            RefreshStates();

            foreach (FlashFirmwareState state in states)
            {
                if (mCancelSource.Token.IsCancellationRequested)
                    break;

                if (!state.Error)
                {
                    if (state.Device.Bootloader.DeviceName != Firmware.DeviceName)
                    {
                        state.State = "Firmware not suitable for this device";
                        state.Error = true;
                    }
                    else
                    {
                        state.State = "Flashing Firmware ...";
                        RefreshStates();

                        if (state.Device.Bootloader.FlashProgramMemory(Firmware, true, false,
                            (progress) =>
                            {
                                state.OverallProgress = 10 + (int)(70.0f * (float)progress / 100.0f);
                                RefreshStates();
                            }))
                            state.State = "Successfully flashed new firmware";
                        else
                        {
                            state.State = "Failed to flash firmware !";
                            state.Error = true;
                        }
                    }
                }
                state.OverallProgress = 80;

                RefreshStates();
            }

            foreach (FlashFirmwareState state in states)
            {
                if (state.Device.HasBootloader && !state.Error)
                {
                    state.Device.Bootloader.Reset(false);
                    state.State = "Entering Normal Mode ...";
                }

                state.OverallProgress = 90;
            }

            RefreshStates();
            Thread.Sleep(1500);

            devices = iXnetDevice.DiscoverDevices(true);
            // sync devices where bootloader could be entered
            foreach (FlashFirmwareState state in states)
            {
                if (!state.Error)
                {
                    var newDev = devices.FirstOrDefault(dev => dev.MacAddress == state.Device.MacAddress);
                    if (newDev == null || !newDev.HasControlPort)
                    {
                        state.State = "Failed to switch to normal mode !";
                        state.Error = true;
                    }
                    else
                    {
                        state.State = "Normal Operation";
                        state.Device = newDev;
                    }
                }

                state.OverallProgress = 100;
            }

            RefreshStates();


            BeginInvoke(new MethodInvoker(() => OnFlashingFinished()));
        }


        private void mBTNClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mBTNStart_Click(object sender, EventArgs e)
        {
            StartFlashing();
        }

        private void mBTNAbort_Click(object sender, EventArgs e)
        {
            AbortFlashing();
        }
    }
}
