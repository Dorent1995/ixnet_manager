using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using iXnetManager.Properties;
using System.Threading;
using iXnet;
using iXnet.ControlPort;
using System.Reflection;
using iXnet.LedPort;
using iXnet.InputHub;
using System.IO;
using iXnet.Bootloader;
using System.Globalization;
using BrightIdeasSoftware;
using iXnetManager.Controls;
using iXnetManager.Theme;

namespace iXnetManager
{
    public partial class MainForm : BaseChromeForm
    {
        private IHubDevice mActiveInputDevice;
        private int mInputPort;


        public MainForm()
        {
            InitializeComponent();
            InstallChrome(resizable: true, showMinimize: true, showMaximize: true);
            ThemeApplier.Apply(this, mBTNApplyChanges, mBTNDiscover);

            Icon = Resources.ix_logo_ixnet;

            SetupCaption();
            SetupListView();
            SetupLedControl();
            SetupStressTesting();

            mPGDevices.PropertyValueChanged += new PropertyValueChangedEventHandler(mPGDevices_PropertyValueChanged);

            OnSelectedDevicesChanged();
        }



        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (!InitInputHub())
            {
                Close();
                return;
            }

            SetupSettings();

            BeginInvoke(new MethodInvoker(delegate()
            {
                DiscoverDevices();
            }));
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (HasChanges(false))
                if (MessageBox.Show(
                    "There are uncommitted changes, do you really want to exit ?\n(Changes will be lost)",
                    "Exit",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
                    e.Cancel = true;

        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            StopStressTestingSelectedDevice();
        }

        bool InitInputHub()
        {
            mInputPort = 2008;

            while (mInputPort < 3000)
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, mInputPort);
                try
                {
                    iXnetDevice.InitLibrary(endpoint);
                    return true;
                }
                catch (System.Exception)
                {
                }

                ++mInputPort;
            }

            if (mInputPort == 3000)
            {
                MessageBox.Show("Failed to initialize iXnet Library.\nInput Sensing will not be possible.", "Init iXnet", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mInputPort = -1;
            }

            return true;
        }

        void SetupCaption()
        {
            string[] parts = Application.ProductVersion.Split('.');
            string version= String.Join(".", parts, 0, 3);

            Text = String.Format("iXnet Manager {0}", version);
        }

        void SetupListView()
        {
            mLBDevices.SelectedIndexChanged += new EventHandler(mLBDevices_SelectedIndexChanged);
            mLBDevices.FormatRow += new EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(mLBDevices_FormatRow);
            mLBDevices.CellToolTipShowing += new EventHandler<ToolTipShowingEventArgs>(mLBDevices_CellToolTipShowing);

            mOLVCMode.Renderer = new MappedImageRenderer(new Object[] {
                OperationMode.Normal, Properties.Resources.ix_logo_ixnet.ToBitmap(),
                OperationMode.Bootloader, Properties.Resources.ix_logo_bootloader.ToBitmap()
            });

        }

        void mLBDevices_CellToolTipShowing(object sender, ToolTipShowingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                e.Text = ((NetDevice)e.Model).OpMode.ToString();
            }
        }

        void SetNetworkDefaults(int sendTimeoutMilli, int maxSendRetries, int retryDelay)
        {
            if (sendTimeoutMilli >= 0)
            {
                BroadcastServiceClientBase.DefaultReplyTimeout = sendTimeoutMilli;
                ServiceClientBase.DefaultReplyTimeout = sendTimeoutMilli;
            }

            if (maxSendRetries >= 0)
            {
                BroadcastServiceClientBase.DefaultMaxRetries = maxSendRetries;
                ServiceClientBase.DefaultMaxRetries = maxSendRetries;
            }

            if (retryDelay >= 0)
            {
                BroadcastServiceClientBase.DefaultRetryDelay = retryDelay;
                ServiceClientBase.DefaultRetryDelay = retryDelay;
            }
        }

        void SetupSettings()
        {
            int defaultTimeout = 300;
            int defaultRetries = 1;
            int defaultRetryDelay = 100;
            SetNetworkDefaults(defaultTimeout, defaultRetries, defaultRetryDelay);

            mTBNetworkMessageTimeout.Text = defaultTimeout.ToString();
            mTBNetworkMessageTimeout.Validating += new CancelEventHandler(PositiveNumber_Validating);
            mTBNetworkMessageTimeout.Validated += new EventHandler(mTBNetworkMessageTimeout_Validated);

            mTBMaxSendRetries.Text = defaultRetries.ToString();
            mTBMaxSendRetries.Validating += new CancelEventHandler(PositiveNumber_Validating);
            mTBMaxSendRetries.Validated += new EventHandler(mTBMaxSendRetries_Validated);

            mTBGlobalRetryDelay.Text = defaultRetryDelay.ToString();
            mTBGlobalRetryDelay.Validating+=new CancelEventHandler(PositiveNumber_Validating);
            mTBGlobalRetryDelay.Validated += new EventHandler(mTBGlobalRetryDelay_Validated);

            mTBInputPort.Text = mInputPort.ToString();
        }

        void mTBGlobalRetryDelay_Validated(object sender, EventArgs e)
        {
            int val = int.Parse(mTBGlobalRetryDelay.Text);
            SetNetworkDefaults(-1, -1, val);
        }

        void mTBMaxSendRetries_Validated(object sender, EventArgs e)
        {
            int val = int.Parse(mTBMaxRetries.Text);
            SetNetworkDefaults(-1, val, -1);
        }

        void mTBNetworkMessageTimeout_Validated(object sender, EventArgs e)
        {
            int val = int.Parse(mTBNetworkMessageTimeout.Text);
            SetNetworkDefaults(val, -1, -1);
        }

        void PositiveNumber_Validating(object sender, CancelEventArgs e)
        {
            var tb = (TextBox)sender;
            int val;
            if (int.TryParse(tb.Text, out val))
            {
                e.Cancel = val <= 0;
            }
            else
                e.Cancel = true;

            if (e.Cancel)
                MessageBox.Show("Invalid value, only positive integers allowed!", "Number Check", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void SetupLedControl()
        {
            SetupLedColorSelection();
            SetupFlashModeSelection();

            mRBSingleLed.CheckedChanged += (sender,args) => mTBLedID.Enabled = mRBSingleLed.Checked;
            mRBSingleLed.Checked = true;
            mCBFlashMode.SelectedIndex = 0;
            mTrackBrightness.Value = 255;

            SyncBrightness();
        }

        void SetupLedColorSelection()
        {
            foreach (var enumVal in Enum.GetValues(typeof(KnownLedColor)))
            {
                mCBLedColor.Items.Add(enumVal);
            }

            mCBLedColor.DrawMode = DrawMode.OwnerDrawFixed;
            mCBLedColor.DropDownStyle = ComboBoxStyle.DropDownList;
            mCBLedColor.DrawItem += new DrawItemEventHandler(mCBLedColor_DrawItem);
            mCBLedColor.SelectedIndex = 0;
        }

        void SetupFlashModeSelection()
        {
            mCBFlashMode.Items.Clear();
            mCBFlashMode.Items.Add(LedMode.LedOn);
            mCBFlashMode.Items.Add(LedMode.LedBlinkSlow);
            mCBFlashMode.Items.Add(LedMode.LedBlinkNormal);
            mCBFlashMode.Items.Add(LedMode.LedBlinkFast);
        }


        void mCBLedColor_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            Graphics g = e.Graphics;
            Rectangle rect = e.Bounds;
            if (e.Index >= 0)
            {
                KnownLedColor color= (KnownLedColor)((ComboBox)sender).Items[e.Index];
                string n = color.ToString();
                Color c = LedColor.FromKnownLedColor(color).ToColor();
                Brush b = new SolidBrush(c);
                g.DrawString(n, e.Font, Brushes.Black, rect.X + 14 , rect.Top);
                g.FillRectangle(b, rect.X + 2, rect.Y + 2, 12 , 12);
            }
        }

        private StressTester mStressTester;
        private System.Windows.Forms.Timer mStressTestUpdater;

        void SetupStressTesting()
        {
            mTBSendTimeout.Value = 2000;
            mTBMaxRetries.Value = 3;
            mTBRetryDelay.Value = 500;

            mBTNStopStressTesting.Enabled = false;

            ResetTestingStatistics();
        }

        void ResetTestingStatistics()
        {
            mLBLPacketsSec.Text = "0";
            mLBLSendPackets.Text = "0";
            mLBLFailedPackets.Text = "0";
            mLBLRetries.Text = "0";
        }

        void UpdateTestingStatistics()
        {
            if (mStressTester == null)
                return;

            mLBLSendPackets.Text = mStressTester.SendPackets.ToString();
            mLBLFailedPackets.Text = mStressTester.FailedPackets.ToString();
            mLBLRetries.Text = mStressTester.Retries.ToString();
            mLBLPacketsSec.Text = mStressTester.PacketsPerSecond.ToString();
        }

        void StartStressTestingSelectedDevice()
        {
            if (mStressTester != null)
                return;

            var selDevice = GetSelectedDevice();
            if (selDevice == null)
                return;

            mBTNStressTestStart.Enabled = false;

            ResetTestingStatistics();

            mStressTester = new StressTester(selDevice.Device);
            mStressTester.ReplyTimeout = (int)mTBSendTimeout.Value;
            mStressTester.MaxRetries = (int)mTBMaxRetries.Value;
            mStressTester.RetryDelay = (int)mTBRetryDelay.Value;

            mStressTestUpdater = new System.Windows.Forms.Timer();
            mStressTestUpdater.Interval = 100;
            mStressTestUpdater.Tick += new EventHandler(mStressTestUpdater_Tick);
            mStressTestUpdater.Start();

            mStressTester.Start();

            mBTNStopStressTesting.Enabled = true;
        }

        void StopStressTestingSelectedDevice()
        {
            if (mStressTester == null)
                return;

            mBTNStopStressTesting.Enabled = false;

            mStressTester.Stop();
            mStressTester = null;
            mStressTestUpdater.Dispose();
            mStressTestUpdater = null;

            mBTNStressTestStart.Enabled = true;
        }

        void mStressTestUpdater_Tick(object sender, EventArgs e)
        {
            UpdateTestingStatistics();
        }


        void UpdateChangeState()
        {
            mLBDevices.BuildList();

            OnSelectedDevicesChanged();
        }

        void OnSelectedDevicesChanged()
        {
            StopStressTestingSelectedDevice();
            UpdateButtonStates();
            ReleaseActiveInputDevice();
        }

        void UpdateButtonStates()
        {
            IEnumerable<NetDevice> selDevices = GetDevices(true, true);
            int selCount = selDevices.Count();
            int selNormalCount = selDevices.Where(dev => !dev.Device.HasBootloader).Count();

            bool hasSelChanges = selDevices.Any(dev => dev.HasChanges);
            bool hasGlobChanges = hasSelChanges || GetDevices(false).Any(dev => dev.HasChanges);
            bool hasSelBld = selDevices.Any(net => net.Device.HasBootloader);

            mBTNApplyChanges.Enabled = hasGlobChanges;
            mBTNDiscardChanges.Enabled = hasSelChanges;

            mBTNReset.Enabled = selCount > 0;
            mBTNFlashDevices.Enabled = selCount > 0;
            mBTNFlashLedModules.Enabled = !hasSelBld && selCount == 1 && selDevices.First().Device.CanFlashLedModules;
            mBTNAssignAddressRange.Enabled = selNormalCount > 1;
            
            mTPLedControl.Enabled = selNormalCount > 0 && selDevices.Any(dev => dev.Features.HasFlag(Feature.LedBus));
            if (mTPLedControl.Enabled)
                mTBLedID.Maximum = selDevices.Min(dev => dev.LedCount);

            mTPStressTesting.Enabled = !hasSelBld && selCount == 1 && selDevices.First().Features.HasFlag(Feature.LedBus);
            mTPInputMonitor.Enabled = !hasSelBld && selCount == 1 && selDevices.First().Features.HasFlag(Feature.Input);
            mTPErrorReporting.Enabled = selNormalCount > 0;
            mTPProcessManager.Enabled = !hasSelBld && selCount == 1 && selDevices.First().Features.HasFlag(Feature.Process);
            mOLVProcesses.SetObjects(null);
        }


        private bool HasChanges(bool selectionOnly)
        {
            return GetDevices(selectionOnly).Any(dev => dev.HasChanges);
        }

        // if localadapter and remoteaddress are specified, these will be used for discovery
        void DiscoverDevices(IPAddress localAdapter = null, IPAddress remoteAddress = null)
        {
            if (HasChanges(false))
                if (MessageBox.Show(
                    "There are uncommitted changes, do you really want to discover new devices ?\n(Changes will be lost)",
                    "Discover Devices",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
                    return;

            mBTNDiscover.Enabled = false;
            mLBDevices.Items.Clear();
            mPGDevices.SelectedObject = null;

            RunWithProgress(delegate(ProgressForm progressForm)
            {
                progressForm.SetProgress(0,"Discovering Devices ...");
                iXnetDevice[] clients = iXnetDevice.DiscoverDevices(true, localAdapter, remoteAddress);

                List<NetDevice> devices = new List<NetDevice>(clients.Length);
                for(int i= 0; i < clients.Length; ++i)
                {
                    iXnetDevice client= clients[i];
                    int progress = 10 + (90 * i) / clients.Length;
                    progressForm.SetProgress(progress, String.Format("Reading Properties from {0} ...", client.SerialNumber));

                    NetDevice netDev = new NetDevice(client);
                    try
                    {
                        netDev.UpdateFromDevice();
                    }
                    catch
                    {
                    	
                    }

                    devices.Add(netDev);
                }

                mLBDevices.Invoke(new MethodInvoker(
                    () => 
                        {
                            mLBDevices.SetObjects(devices.ToArray());
                            if (devices.Count > 0)
                                mLBDevices.SelectedIndex = 0;
                        }));                
            });

            mBTNDiscover.Enabled = true;
        }

        void DiscardChanges(bool selectionOnly)
        {
            if (MessageBox.Show(
                "Do you really want to discard the changes ?",
                "Discard Changes", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question) == DialogResult.No)
                return;

            RunAsync(dev => dev.DiscardChanges(), "Discard Changes from {0} ...", selectionOnly);
            UpdateChangeState();
        }

        void ApplyChanges(bool selectionOnly)
        {
            RunAsync(dev => dev.ApplyChanges(), "Apply Changes to {0} ...", selectionOnly);
            UpdateChangeState();
        }

        void RefreshProperties(bool selectionOnly)
        {
            if (HasChanges(selectionOnly))
            {
                if (MessageBox.Show("Refresh will discard changes, continue ?",
                    "Refresh Properties",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }

            RunAsync(dev => dev.UpdateFromDevice(), "Update properties from {0} ...", selectionOnly);
            UpdateChangeState();
        }

        void Reset(bool selectionOnly)
        {
            if (HasChanges(false))
                if (MessageBox.Show(
                    "There are uncommitted changes, do you really want to reset the devices ?\n(Changes will be lost)",
                    "Reset Device",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
                    return;

            RunAsync(dev => dev.Reset(), "Resetting Device {0} ...", selectionOnly);

            WaitSomeTime(1000);
            DiscoverDevices();
        }

        void FactoryReset(bool selectionOnly)
        {
            if (MessageBox.Show("Do you really want to do a factory reset for the selected devices ?",
                                "Factory Reset",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Warning) == DialogResult.No)
                return;

            RunAsync(dev => dev.FactoryReset(), "Factory Resetting Device {0} ...", selectionOnly);

            WaitSomeTime(3000);
            DiscoverDevices();
        }


        void FlashLedModules()
        {
            NetDevice dev = GetSelectedDevice();
            if (dev == null)
                return;

            if (dev.HasChanges)
                if (MessageBox.Show(
                    "There are uncommitted changes, these will be lost if you continue.\nContinue ?",
                    "Flash Led Modules",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
                    return;

            using (FlashLedModulesForm flashForm = new FlashLedModulesForm(dev))
            {
                flashForm.StartPosition = FormStartPosition.CenterParent;
                flashForm.ShowDialog();
            }

            UpdateChangeState();
        }

        void FlashSelectedDevices()
        {
            var devices = GetDevices(true, true);
            if (devices.Count() == 0)
                return;

            using (FlashProgramMemoryForm prgmForm = new FlashProgramMemoryForm(devices.Select(dev => dev.Device)))
            {
                prgmForm.ShowDialog();
            }

            DiscoverDevices();
        }

        void WaitSomeTime(int milliseconds)
        {
            UseWaitCursor = true;
            Application.DoEvents();

            Thread.Sleep(milliseconds);

            UseWaitCursor = false;
        }

        void ShowSelectedDevices()
        {
            ExecEveryDevice(dev => dev.Show(),
                "Failed to show the following device(s):\n{0}",
                "Show Device",
                true,
                mBTNShowDevice);
        }

        void ClearAllLedsSelectedDevices()
        {
            ExecEveryDevice(dev => dev.ClearAllLeds(),
                "Failed to clear leds on following device(s):\n{0}",
                "Clear All Leds",
                true,
                mBTNClearAllLeds);
        }

        void ShowLedIdsSelectedDevices()
        {
            ExecEveryDevice(dev => dev.ShowLedIds(),
                "Failed to show led ids on following device(s):\n{0}",
                "Show Led IDs",
                true,
                mBTNShowLedIds);
        }

        void SetColorSelectedDevices()
        {
            int ledID;
            if (mRBSingleLed.Checked)
                ledID = (int)mTBLedID.Value;
            else
                ledID = -1; // all leds

            LedColor color = LedColor.FromKnownLedColor((KnownLedColor)mCBLedColor.SelectedItem);
            int brightness= mTrackBrightness.Value;
            color.R = (byte)(((int)color.R * brightness) / 255);
            color.G = (byte)(((int)color.G * brightness) / 255);
            color.B = (byte)(((int)color.B * brightness) / 255);

            LedMode mode = (LedMode)mCBFlashMode.SelectedItem;
            string text= mTBLedText.Text;

            ExecEveryDevice(dev => dev.SetLedState(ledID, mode, color, text),
                "Failed to set led state on following device(s):\n{0}",
                "Set Color",
                true,
                mBTNSetLedColor);
        }

        void ClearLedSelectedDevices()
        {
            int ledID;
            if (mRBSingleLed.Checked)
                ledID = (int)mTBLedID.Value;
            else
            {
                ClearAllLedsSelectedDevices();
                return;
            }

            ExecEveryDevice(dev => dev.SetLedState(ledID, LedMode.LedOff, LedColor.Black, null),
                "Failed to set led state on following device(s):\n{0}",
                "Clear Color", 
                true,
                mBTNClearLed);
        }

        void SyncBrightness()
        {
            mTBBrightness.Text = mTrackBrightness.Value.ToString();
        }

        void AddDeviceManually()
        {
            using (var newDeviceForm = new AddDeviceForm())
            {
                var result = newDeviceForm.ShowDialog();
                if (result != DialogResult.OK)
                    return;

                DiscoverDevices(newDeviceForm.LocalAdapterIP, newDeviceForm.RemoteAddress);
            }
        }


        private void ExecEveryDevice(Func<NetDevice,bool> func, string failedMessage, string failedMessageCaption, bool selectionOnly, Button button= null)
        {
            if (button != null)
                button.Enabled = false;

            var devices = GetDevices(selectionOnly);

            List<NetDevice> failedDevices = new List<NetDevice>();
            foreach (var dev in devices)
            {
                if (!func(dev))
                    failedDevices.Add(dev);
            }

            if (failedDevices.Count != 0)
                MessageBox.Show(
                    String.Format(failedMessage, String.Join("\n", failedDevices.Select(dev => dev.SerialNumber))),
                    failedMessageCaption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );

            if (button != null)
                button.Enabled = true;
        }

        private void UpdateInputSubDevices()
        {
            var dev = GetSelectedDevice();
            if (dev == null)
                return;

            mCBInputSelect.Items.Clear();
            ReleaseActiveInputDevice();

            dev.UpdateInputDevices();
            mCBInputSelect.Items.AddRange(dev.InputDevices);
        }

        void ActivateInputDevice()
        {
            ReleaseActiveInputDevice();

            var inputDev = mCBInputSelect.SelectedItem as IHubDevice;
            if (inputDev == null)
                return;

            if (!inputDev.IsEventDevice)
            {
                ReadAnalogDevice(inputDev);
                return;
            }

            mCBInputSelect.Enabled = false;
            if (!inputDev.Acquire())
            {
                mCBInputSelect.Enabled = true;
                MessageBox.Show("Failed to Activate Input Device", "Activate Input Device", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            mActiveInputDevice = inputDev;
            if (mActiveInputDevice is CharacterDevice)
                ((CharacterDevice)mActiveInputDevice).CharReceived += ReceivedInputEvent;
            else if (mActiveInputDevice is ButtonDevice)
                ((ButtonDevice)mActiveInputDevice).ButtonChangeReceived += ReceivedInputEvent;

            mCBActivateInput.Checked = true;
            mTBReceivedInput.ReadOnly = true;
        }


        void ReadAnalogDevice(IHubDevice dev)
        {
            AnalogDevice aDev = dev as AnalogDevice;
            if (aDev == null)
                return;

            StringBuilder strBuilder = new StringBuilder();

            for (UInt16 channel = 0; channel < aDev.NumChannels; ++channel)
            {
                UInt32 value= aDev.ReadIntValue(channel);
                strBuilder.AppendFormat("Channel {0}: {1}", channel, value);
                strBuilder.AppendLine();
            }

            mTBReceivedInput.Text = strBuilder.ToString();
        }

        void ReceivedInputEvent(object sender, EventArgs args)
        {
            BeginInvoke(new MethodInvoker(delegate()
            {
                mTBReceivedInput.Text = String.Format("{0} {1}", mTBReceivedInput.Text, args);
            }));
        }

        void ReleaseActiveInputDevice()
        {
            if (mActiveInputDevice != null)
            {
                if (mActiveInputDevice is CharacterDevice)
                    ((CharacterDevice)mActiveInputDevice).CharReceived -= ReceivedInputEvent;
                else if (mActiveInputDevice is ButtonDevice)
                    ((ButtonDevice)mActiveInputDevice).ButtonChangeReceived -= ReceivedInputEvent;

                mActiveInputDevice = null;

                mCBInputSelect.Enabled = true;
            }

            mTBReceivedInput.Text = String.Empty;
            mCBActivateInput.Checked = false;
        }

        void OpenDebugSending()
        {
            var inputDev = mCBInputSelect.SelectedItem as IHubDevice;
            if (inputDev == null)
                return;

            if (!inputDev.SupportsDebugSend)
                return;

            if (inputDev is CharacterDevice)
            {
                SendDebugCharactersForm debugForm = new SendDebugCharactersForm((CharacterDevice)inputDev);
                debugForm.StartPosition = FormStartPosition.CenterParent;
                debugForm.Show();
            }
        }


        private void RunAsync(Action<NetDevice> perDeviceAction, String perDeviceDescription, bool selectionOnly, Action finishedAction= null)
        {
            NetDevice[] devices = GetDevices(selectionOnly).ToArray();

            RunWithProgress(delegate(ProgressForm progressForm)
            {
                for (int i = 0; i < devices.Length; ++i)
                {
                    NetDevice dev = devices[i];
                    int progress = (100 * i) / devices.Length;
                    string description = String.Format(perDeviceDescription, dev.SerialNumber);

                    progressForm.SetProgress(progress, description);

                    try
                    {
                        perDeviceAction(dev);
                    }
                    catch
                    {

                    }
                }

                if (finishedAction != null)
                {
                    this.BeginInvoke(new MethodInvoker(delegate()
                        {
                            finishedAction();
                        }));
                }
            });
        }

        private void RunWithProgress(Action<ProgressForm> action)
        {
            ProgressForm progressForm = new ProgressForm();

            ThreadPool.QueueUserWorkItem(delegate(object state)
            {
                while (!progressForm.IsHandleCreated)
                    Thread.Sleep(10);

                try
                {
                    action(progressForm);
                }
                catch
                {
                	
                }

                progressForm.Invoke(new MethodInvoker(delegate() { progressForm.Close(); }));
            });

            progressForm.ShowDialog();
        }

        private IEnumerable<NetDevice> GetDevices(bool selectionOnly, bool includeBootloader= false)
        {
            if (selectionOnly)
                return mLBDevices.SelectedObjects.Cast<NetDevice>().Where(dev => includeBootloader ? true : !dev.Device.HasBootloader);
            else
                if (mLBDevices.Objects == null)
                    return Enumerable.Empty<NetDevice>();
                else
                    return mLBDevices.Objects.Cast<NetDevice>().Where(dev => includeBootloader ? true : !dev.Device.HasBootloader);
        }

        private NetDevice GetSelectedDevice(bool includeBootloader= false)
        {
            return GetDevices(true,includeBootloader).FirstOrDefault();
        }

        private void ReadErrors(bool selectionOnly)
        {
            List<ErrorObject> errors = (List<ErrorObject>)mOLVErrors.Objects;
            if (errors == null)
                errors = new List<ErrorObject>();

            RunAsync(dev => errors.AddRange(dev.ReadErrors().Select(error => new ErrorObject(error, dev.SerialNumber))),
                "Read errors from {0}",
                selectionOnly,
                () =>
                {
                    mOLVErrors.SetObjects(errors);
                });
        }

        private void ClearErrors()
        {
            mOLVErrors.SetObjects(null);
        }

        private bool HasErrorLog()
        {
            List<ErrorObject> errors = (List<ErrorObject>)mOLVErrors.Objects;
            if (errors == null)
                return false;

            return errors.Count > 0;
        }

        private void SaveErrorLog()
        {
            if (!HasErrorLog())
                return;

            SaveFileDialog fileDialog = new SaveFileDialog();

            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                SaveErrorLog(fileDialog.FileName);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(String.Format("Failed to save log !\nError: {0}",ex.Message));        	
            }
        }

        private void SaveErrorLog(string fileName)
        {
            List<ErrorObject> errors = (List<ErrorObject>)mOLVErrors.Objects;
            if (errors == null)
                return;

            using (var logFile = File.Create(fileName))
            {
                using (var logWriter = new StreamWriter(logFile))
                {
                    logWriter.WriteLine("Time ; Serialnumber ; Error Code ; Error Text");
                    foreach (var error in errors)
                    {
                        logWriter.WriteLine("{0} ; {1} ; {2} ; {3}", error.TimeStamp, error.SerialNumber, error.ErrorCode, error.ErrorText);
                    }
                }

                logFile.Close();
            }

        }

        private void UpdateProcessList()
        {
            var dev= GetSelectedDevice();
            if (dev == null)
                return;


            ProcessManager procManager = new ProcessManager(dev.Device.ControlPort);
            if (!procManager.UpdateListing())
            {
                MessageBox.Show("Failed to read processes !", "Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            mOLVProcesses.SetObjects(procManager.Processes);            
        }

        private void DeleteSelectedProcesses()
        {
            var selItems = mOLVProcesses.SelectedObjects.Cast<Process>();
            var selCount = selItems.Count();
            if (selCount == 0)
                return;

            var selDevice= GetSelectedDevice();
            if (selDevice == null)
                return;

            string processWord = selCount == 1 ? "Process" : "Processes";
            if (MessageBox.Show(
                String.Format("Delete {0} {1}?", selCount, processWord),
                String.Format("Delete {0}", processWord),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No)
                return;

            ProcessManager procManager = selDevice.ProcessManager;

            RunWithProgress(delegate (ProgressForm progressForm)
            {
                progressForm.SetProgress(0, "Deleting Processes ...");

                for (int i = 0; i < selItems.Count(); ++i)
                {
                    var proc = selItems.ElementAt(i);
                    int progress = 10 + (90 * i) / selItems.Count();
                    progressForm.SetProgress(progress, String.Format("Deleting Process: {0} ...", proc.Name));

                    procManager.DeleteProcess(proc);
                }
            });

            UpdateProcessList();
        }

        private void BackupSelectedProcesses()
        {
            var selItems = mOLVProcesses.SelectedObjects.Cast<Process>();
            var selCount = selItems.Count();
            if (selCount == 0)
                return;

            var selDevice = GetSelectedDevice();
            if (selDevice == null)
                return;

            mBTNBackupProcess.Enabled = false;

            ProcessManager procManager = selDevice.ProcessManager;

            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                    return;

                RunWithProgress(delegate (ProgressForm progressForm)
                {
                    progressForm.SetProgress(0, "Backup Processes ...");

                    for (int i = 0; i < selItems.Count(); ++i)
                    {
                        var proc = selItems.ElementAt(i);
                        int progress = 10 + (90 * i) / selItems.Count();
                        progressForm.SetProgress(progress, String.Format("Backup of Process: {0} ...", proc.Name));

                        byte[] backupData = procManager.BackupProcess(proc);
                        try
                        {
                            string fileName = string.Format("{0}\\{1}.proc", folderBrowserDialog.SelectedPath, RemoveIllegalPathChars(proc.Name));
                            File.WriteAllBytes(fileName, backupData);
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show(String.Format("Failed to store process '{0}'!\nError:{1}", proc.Name, ex.Message), "Backup Process");
                            continue;
                        }
                    }
                });

                MessageBox.Show(String.Format("Successfully saved processes to '{0}'", folderBrowserDialog.SelectedPath), "Backup Process");
            }

            mBTNBackupProcess.Enabled = true;
        }

        private void BackupFullEeprom()
        {
            /*
            // Stresstest
            var selDevice = GetSelectedDevice();
            if (selDevice == null)
                return;

            ProcessManager procManager = selDevice.ProcessManager;
            byte[] backupData1 = procManager.BackupEeprom(); ;
            byte[] backupData2;

            for (int i = 0; i < 1000; i++)
            {
                procManager.RestoreEeprom(backupData1);
                backupData2 = procManager.BackupEeprom();

                var identical = backupData1.SequenceEqual(backupData2); 

                if (identical)
                    Console.WriteLine(string.Format("{0} : Identical", i));
                else
                {
                    Console.WriteLine(string.Format("{0} : NOT identical", i));
                    Console.WriteLine(string.Format("{0} : Size written {1}, size read {2}", i, backupData1.Length, backupData2.Length));

                    File.WriteAllBytes("c:\\temp\\written.eep", backupData1);
                    File.WriteAllBytes("c:\\temp\\readback.eep", backupData2);
                }

            }
            */

            var selDevice = GetSelectedDevice();
            if (selDevice == null)
                return;

            ProcessManager procManager = selDevice.ProcessManager;
            using (SaveFileDialog fileDialog = new SaveFileDialog())
            {
                fileDialog.FileName = String.Format("{0}.eep", selDevice.SerialNumber);
                fileDialog.Title = String.Format("Select backup file for '{0}'", selDevice.SerialNumber);
                if (fileDialog.ShowDialog() != DialogResult.OK)
                    return;

                byte[] backupData = procManager.BackupEeprom();
                try
                {
                    File.WriteAllBytes(fileDialog.FileName, backupData);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(String.Format("Failed to store eeprom of device '{0}'!\nError:{1}", selDevice.SerialNumber, ex.Message), "Backup Eeprom");
                    return;
                }
                MessageBox.Show(String.Format("Successfully created backup file {0} of device '{1}'.", fileDialog.FileName, selDevice.SerialNumber), "Backup Eeprom");
            }
        }

        private void RestoreFullEeprom()
        {
            var selDevice = GetSelectedDevice();
            if (selDevice == null)
                return;

            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Title = "Select eeprom backup file";
                openDialog.CheckFileExists = true;
                if (openDialog.ShowDialog() != DialogResult.OK)
                    return;

                byte[] backupData;
                try
                {
                    backupData = File.ReadAllBytes(openDialog.FileName);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(String.Format("Failed to eeprom open backup file '{0}' !\nError:{1}", openDialog.FileName, ex.Message));
                    return;
                }

                if (MessageBox.Show(
                      String.Format("Warning! This will overwrite all processes. Continue?"),
                      "Overwrite all processes",
                      MessageBoxButtons.YesNo,
                      MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                ProcessManager procManager = selDevice.ProcessManager;
                procManager.RestoreEeprom(backupData);

                MessageBox.Show(String.Format("Successfully restored eeprom file {0} to device '{1}'.", openDialog.FileName, selDevice.SerialNumber), "Restore Eeprom");
            }

            UpdateProcessList();
        }

        private void RestoreProcess()
        {
            var selDevice = GetSelectedDevice();
            if (selDevice == null)
                return;

            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Title = "Select backup file";
                openDialog.CheckFileExists = true;
                openDialog.Multiselect = true;

                if (openDialog.ShowDialog() != DialogResult.OK)
                    return;

                RunWithProgress(delegate (ProgressForm progressForm)
                {
                    progressForm.SetProgress(0, "Restoring Processes ...");

                    for (int i = 0; i < openDialog.FileNames.Count(); ++i)
                    {
                        var fileName = openDialog.FileNames.ElementAt(i);
                        int progress = 10 + (90 * i) / openDialog.FileNames.Count();

                        progressForm.SetProgress(progress, String.Format("Restoring of Process: {0} ...", Path.GetFileName(fileName)));

                        byte[] backupData;
                        try
                        {
                            backupData = File.ReadAllBytes(fileName);
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show(String.Format("Failed to open backup file '{0}'!\nError:{1}", fileName, ex.Message));
                            return;
                        }

                        ProcessManager procManager = selDevice.ProcessManager;
                        string procName = procManager.GetProcessNameFromBackup(backupData);

                        procManager.UpdateListing();
                        if (procManager.Processes.FirstOrDefault(p => p.Name == procName) != null)
                        {
                            if (MessageBox.Show(
                                String.Format("There is already a process with name '{0}'.\nRestoring will overwrite that process.\nDo you want to continue?", procName),
                                "Restore Process",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) != DialogResult.Yes)
                                return;
                        }

                        procManager.RestoreProcess(backupData);
                    }
                });
            }

            UpdateProcessList();
        }

        private string RemoveIllegalPathChars(string path)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                path = path.Replace(c.ToString(), "");
            }

            return path;
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                RefreshProperties(false);
                return true;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ReadLog(bool selectionOnly)
        {
            List<LogEntry> logs = new List<LogEntry>();

            RunAsync(dev => logs.AddRange(dev.ReadLogs()),
                "Read logs from {0}",
                selectionOnly,
                () =>
                {
                    StringBuilder logBuilder = new StringBuilder();
                    foreach (var log in logs)
                    {
                        logBuilder.AppendLine(log.Message);
                    }

                    mTBLogs.Text = logBuilder.ToString();
                });
        }

        void mPGDevices_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            UpdateChangeState();
        }

        void mLBDevices_FormatRow(object sender, BrightIdeasSoftware.FormatRowEventArgs e)
        {
            NetDevice dev = (NetDevice)e.Model;
            if (dev.HasChanges)
                e.Item.Font = new Font(e.Item.Font, FontStyle.Bold);
        }


        void mLBDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mLBDevices.SelectedItems != null)
            {
                mPGDevices.SelectedObjects = mLBDevices.SelectedObjects.Cast<NetDevice>()
                   .Where(dev => !dev.Device.HasBootloader).Select(dev => dev.PropertyTable).ToArray();
            }
            else
            {
                mPGDevices.SelectedObject = null;
            }

            OnSelectedDevicesChanged();
        }

        private void mBTNDiscover_Click(object sender, EventArgs e)
        {
            DiscoverDevices();
        }

        private void mBTNReset_Click(object sender, EventArgs e)
        {
            Reset(true);
        }

        private void mBTNApplyChanges_Click(object sender, EventArgs e)
        {
            ApplyChanges(false);
        }

        private void mBTNDiscardChanges_Click(object sender, EventArgs e)
        {
            DiscardChanges(true);
        }

        private void mBTNAssignAddressRange_Click(object sender, EventArgs e)
        {
            IPRangeAssignForm assignForm = new IPRangeAssignForm();
            assignForm.StartPosition = FormStartPosition.CenterParent;
            assignForm.Devices = GetDevices(true);
            
            assignForm.ShowDialog();
            assignForm.Dispose();

            UpdateChangeState();
        }

        private void mBTNShowDevice_Click(object sender, EventArgs e)
        {
            ShowSelectedDevices();
        }

        private void mBTNClearAllLeds_Click(object sender, EventArgs e)
        {
            ClearAllLedsSelectedDevices();
        }

        private void mBTNShowLedIds_Click(object sender, EventArgs e)
        {
            ShowLedIdsSelectedDevices();
        }

        private void mBTNSetLedColor_Click(object sender, EventArgs e)
        {
            SetColorSelectedDevices();
        }

        private void mBTNClearLed_Click(object sender, EventArgs e)
        {
            ClearLedSelectedDevices();
        }

        private void mCBInputSelect_DropDown(object sender, EventArgs e)
        {
            UpdateInputSubDevices();
        }

        private void mCBActivateInput_Click(object sender, EventArgs e)
        {
            if (mCBActivateInput.Checked)
                ReleaseActiveInputDevice();
            else
                ActivateInputDevice();
        }

        private void mBTNFlashDevices_Click(object sender, EventArgs e)
        {
            FlashSelectedDevices();
        }

        private void mBTNFlashLedModules_Click(object sender, EventArgs e)
        {
            using (SecurityTokenForm passwordForm = new SecurityTokenForm())
            {
                if (passwordForm.ShowDialog() == DialogResult.OK)
                {
                    var currentWeek= GetIso8601WeekOfYear(DateTime.Now) * 2;
                    var currentPassword = "KW" + currentWeek.ToString();
                    if (passwordForm.Password == currentPassword)
                        FlashLedModules();
                    else
                        MessageBox.Show("Wrong Password!", "Security Check", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void mBTNReadErrors_Click(object sender, EventArgs e)
        {
            ReadErrors(true);
        }

        private void mBTNClearErrors_Click(object sender, EventArgs e)
        {
            ClearErrors();
        }

        private void mBTNSaveLog_Click(object sender, EventArgs e)
        {
            SaveErrorLog();
        }


        private void mBTNDebugSend_Click(object sender, EventArgs e)
        {
            OpenDebugSending();
        }

        private void mBTNUpdateProcessList_Click(object sender, EventArgs e)
        {
            UpdateProcessList();
        }

        private void mBTNFullBackup_Click(object sender, EventArgs e)
        {
            BackupFullEeprom();
        }

        private void mBTNFullRestore_Click(object sender, EventArgs e)
        {
            RestoreFullEeprom();
        }

        private void mBTNDeleteProcess_Click(object sender, EventArgs e)
        {
            DeleteSelectedProcesses();
        }

        private void mBTNBackupProcess_Click(object sender, EventArgs e)
        {
            BackupSelectedProcesses();
        }

        private void mBTNRestoreProcess_Click(object sender, EventArgs e)
        {
            RestoreProcess();
        }

        private void mBTNStressTestStart_Click(object sender, EventArgs e)
        {
            StartStressTestingSelectedDevice();
        }

        private void mBTNStopStressTesting_Click(object sender, EventArgs e)
        {
            StopStressTestingSelectedDevice();
        }

        private void mTrackBrightness_Scroll(object sender, EventArgs e)
        {
            SyncBrightness();
        }

        private void mBTNReadLog_Click(object sender, EventArgs e)
        {
            ReadLog(true);
        }

        private void mBTNFactoryReset_Click(object sender, EventArgs e)
        {
            FactoryReset(true);
        }

        private void mBtnAddManually_Click(object sender, EventArgs e)
        {
            AddDeviceManually();
        }

        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        private int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        
            //ResetLedBusSelectedDevices();
            //timer1.Interval = new Random().Next(1000, 5000);
        }
    }
}
