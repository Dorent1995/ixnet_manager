namespace iXnetManager
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.mLBDevices = new BrightIdeasSoftware.ObjectListView();
            this.mOLVCMode = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn6 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn4 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn5 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.mPGDevices = new System.Windows.Forms.PropertyGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mBtnAddManually = new System.Windows.Forms.Button();
            this.mBTNFactoryReset = new System.Windows.Forms.Button();
            this.mBTNFlashLedModules = new System.Windows.Forms.Button();
            this.mBTNFlashDevices = new System.Windows.Forms.Button();
            this.mBTNDiscardChanges = new System.Windows.Forms.Button();
            this.mBTNReset = new System.Windows.Forms.Button();
            this.mBTNApplyChanges = new System.Windows.Forms.Button();
            this.mBTNDiscover = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.mBTNAssignAddressRange = new System.Windows.Forms.Button();
            this.mFunctionTabs = new System.Windows.Forms.TabControl();
            this.mTPLedControl = new System.Windows.Forms.TabPage();
            this.mTBBrightness = new System.Windows.Forms.TextBox();
            this.mCBLedColor = new System.Windows.Forms.ComboBox();
            this.mTrackBrightness = new iXnetManager.Controls.ModernSlider();
            this.label17 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.mBTNShowDevice = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.mTBLedID = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.mRBSingleLed = new System.Windows.Forms.RadioButton();
            this.mTBLedText = new System.Windows.Forms.TextBox();
            this.mRBAllLeds = new System.Windows.Forms.RadioButton();
            this.mBTNClearAllLeds = new System.Windows.Forms.Button();
            this.mBTNShowLedIds = new System.Windows.Forms.Button();
            this.mBTNSetLedColor = new System.Windows.Forms.Button();
            this.mBTNClearLed = new System.Windows.Forms.Button();
            this.mCBFlashMode = new System.Windows.Forms.ComboBox();
            this.mTPStressTesting = new System.Windows.Forms.TabPage();
            this.mBTNStopStressTesting = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.mTBSendTimeout = new System.Windows.Forms.NumericUpDown();
            this.mBTNStressTestStart = new System.Windows.Forms.Button();
            this.mLBLPacketsSec = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.mLBLFailedPackets = new System.Windows.Forms.Label();
            this.mLBLSendPackets = new System.Windows.Forms.Label();
            this.mLBLRetries = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.mTBRetryDelay = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.mTBMaxRetries = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.mTPInputMonitor = new System.Windows.Forms.TabPage();
            this.mBTNDebugSend = new System.Windows.Forms.Button();
            this.mCBActivateInput = new iXnetManager.Controls.ToggleSwitch();
            this.mLBLActivateInput = new System.Windows.Forms.Label();
            this.mCBInputSelect = new System.Windows.Forms.ComboBox();
            this.mTBReceivedInput = new System.Windows.Forms.TextBox();
            this.mTPErrorReporting = new System.Windows.Forms.TabPage();
            this.mBTNSaveLog = new System.Windows.Forms.Button();
            this.mBTNClearErrors = new System.Windows.Forms.Button();
            this.mOLVErrors = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn7 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn11 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn8 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn9 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn10 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.mBTNReadErrors = new System.Windows.Forms.Button();
            this.mTPLog = new System.Windows.Forms.TabPage();
            this.mBTNReadLog = new System.Windows.Forms.Button();
            this.mTBLogs = new System.Windows.Forms.TextBox();
            this.mTPProcessManager = new System.Windows.Forms.TabPage();
            this.mBTNDeleteProcess = new System.Windows.Forms.Button();
            this.mBTNRestoreProcess = new System.Windows.Forms.Button();
            this.mBTNBackupProcess = new System.Windows.Forms.Button();
            this.mBTNUpdateProcessList = new System.Windows.Forms.Button();
            this.mOLVProcesses = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn12 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn13 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn14 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.mTBInputPort = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.mTBGlobalRetryDelay = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.mTBMaxSendRetries = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.mTBNetworkMessageTimeout = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.mBTNFullBackup = new System.Windows.Forms.Button();
            this.mBTNFullRestore = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mLBDevices)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.mFunctionTabs.SuspendLayout();
            this.mTPLedControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mTBLedID)).BeginInit();
            this.mTPStressTesting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mTBSendTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mTBRetryDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mTBMaxRetries)).BeginInit();
            this.mTPInputMonitor.SuspendLayout();
            this.mTPErrorReporting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mOLVErrors)).BeginInit();
            this.mTPLog.SuspendLayout();
            this.mTPProcessManager.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mOLVProcesses)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 269F));
            this.tableLayoutPanel1.Controls.Add(this.mLBDevices, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.mPGDevices, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.mFunctionTabs, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 217F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1008, 585);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // mLBDevices
            // 
            this.mLBDevices.AllColumns.Add(this.mOLVCMode);
            this.mLBDevices.AllColumns.Add(this.olvColumn1);
            this.mLBDevices.AllColumns.Add(this.olvColumn2);
            this.mLBDevices.AllColumns.Add(this.olvColumn6);
            this.mLBDevices.AllColumns.Add(this.olvColumn3);
            this.mLBDevices.AllColumns.Add(this.olvColumn4);
            this.mLBDevices.AllColumns.Add(this.olvColumn5);
            this.mLBDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.mOLVCMode,
            this.olvColumn1,
            this.olvColumn2,
            this.olvColumn6,
            this.olvColumn3,
            this.olvColumn4,
            this.olvColumn5});
            this.mLBDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mLBDevices.EmptyListMsg = "No Devices Found";
            this.mLBDevices.FullRowSelect = true;
            this.mLBDevices.HideSelection = false;
            this.mLBDevices.Location = new System.Drawing.Point(3, 3);
            this.mLBDevices.Name = "mLBDevices";
            this.mLBDevices.OwnerDraw = true;
            this.mLBDevices.ShowGroups = false;
            this.mLBDevices.Size = new System.Drawing.Size(733, 302);
            this.mLBDevices.TabIndex = 0;
            this.mLBDevices.UseCompatibleStateImageBehavior = false;
            this.mLBDevices.View = System.Windows.Forms.View.Details;
            // 
            // mOLVCMode
            // 
            this.mOLVCMode.AspectName = "OpMode";
            this.mOLVCMode.CellPadding = null;
            this.mOLVCMode.Text = "Mode";
            this.mOLVCMode.Width = 40;
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "SerialNumber";
            this.olvColumn1.CellPadding = null;
            this.olvColumn1.Text = "Serialnumber";
            this.olvColumn1.Width = 100;
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "IPAddress";
            this.olvColumn2.CellPadding = null;
            this.olvColumn2.Text = "IP";
            this.olvColumn2.Width = 106;
            // 
            // olvColumn6
            // 
            this.olvColumn6.AspectName = "UpTime";
            this.olvColumn6.CellPadding = null;
            this.olvColumn6.Text = "Up Time";
            this.olvColumn6.Width = 81;
            // 
            // olvColumn3
            // 
            this.olvColumn3.AspectName = "FWVersion";
            this.olvColumn3.CellPadding = null;
            this.olvColumn3.Text = "FW Version";
            this.olvColumn3.Width = 87;
            // 
            // olvColumn4
            // 
            this.olvColumn4.AspectName = "MACAddress";
            this.olvColumn4.CellPadding = null;
            this.olvColumn4.Text = "MAC";
            this.olvColumn4.Width = 120;
            // 
            // olvColumn5
            // 
            this.olvColumn5.AspectName = "Features";
            this.olvColumn5.CellPadding = null;
            this.olvColumn5.Text = "Features";
            this.olvColumn5.Width = 164;
            // 
            // mPGDevices
            // 
            this.mPGDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mPGDevices.LineColor = System.Drawing.SystemColors.ControlDark;
            this.mPGDevices.Location = new System.Drawing.Point(742, 3);
            this.mPGDevices.Name = "mPGDevices";
            this.mPGDevices.Size = new System.Drawing.Size(263, 302);
            this.mPGDevices.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.mBtnAddManually);
            this.panel1.Controls.Add(this.mBTNFactoryReset);
            this.panel1.Controls.Add(this.mBTNFlashLedModules);
            this.panel1.Controls.Add(this.mBTNFlashDevices);
            this.panel1.Controls.Add(this.mBTNDiscardChanges);
            this.panel1.Controls.Add(this.mBTNReset);
            this.panel1.Controls.Add(this.mBTNApplyChanges);
            this.panel1.Controls.Add(this.mBTNDiscover);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 311);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(733, 54);
            this.panel1.TabIndex = 2;
            // 
            // mBtnAddManually
            // 
            this.mBtnAddManually.Location = new System.Drawing.Point(237, 28);
            this.mBtnAddManually.Name = "mBtnAddManually";
            this.mBtnAddManually.Size = new System.Drawing.Size(111, 23);
            this.mBtnAddManually.TabIndex = 10;
            this.mBtnAddManually.Text = "Add Device...";
            this.mBtnAddManually.UseVisualStyleBackColor = true;
            this.mBtnAddManually.Click += new System.EventHandler(this.mBtnAddManually_Click);
            // 
            // mBTNFactoryReset
            // 
            this.mBTNFactoryReset.Location = new System.Drawing.Point(471, 3);
            this.mBTNFactoryReset.Name = "mBTNFactoryReset";
            this.mBTNFactoryReset.Size = new System.Drawing.Size(111, 23);
            this.mBTNFactoryReset.TabIndex = 7;
            this.mBTNFactoryReset.Text = "Factory Reset ...";
            this.mBTNFactoryReset.UseVisualStyleBackColor = true;
            this.mBTNFactoryReset.Click += new System.EventHandler(this.mBTNFactoryReset_Click);
            // 
            // mBTNFlashLedModules
            // 
            this.mBTNFlashLedModules.Location = new System.Drawing.Point(120, 28);
            this.mBTNFlashLedModules.Name = "mBTNFlashLedModules";
            this.mBTNFlashLedModules.Size = new System.Drawing.Size(111, 23);
            this.mBTNFlashLedModules.TabIndex = 6;
            this.mBTNFlashLedModules.Text = "Flash Led Modules";
            this.mBTNFlashLedModules.UseVisualStyleBackColor = true;
            this.mBTNFlashLedModules.Click += new System.EventHandler(this.mBTNFlashLedModules_Click);
            // 
            // mBTNFlashDevices
            // 
            this.mBTNFlashDevices.Location = new System.Drawing.Point(3, 28);
            this.mBTNFlashDevices.Name = "mBTNFlashDevices";
            this.mBTNFlashDevices.Size = new System.Drawing.Size(111, 23);
            this.mBTNFlashDevices.TabIndex = 5;
            this.mBTNFlashDevices.Text = "Flash FW ...";
            this.mBTNFlashDevices.UseVisualStyleBackColor = true;
            this.mBTNFlashDevices.Click += new System.EventHandler(this.mBTNFlashDevices_Click);
            // 
            // mBTNDiscardChanges
            // 
            this.mBTNDiscardChanges.Enabled = false;
            this.mBTNDiscardChanges.Location = new System.Drawing.Point(237, 3);
            this.mBTNDiscardChanges.Name = "mBTNDiscardChanges";
            this.mBTNDiscardChanges.Size = new System.Drawing.Size(111, 23);
            this.mBTNDiscardChanges.TabIndex = 2;
            this.mBTNDiscardChanges.Text = "Discard Changes";
            this.mBTNDiscardChanges.UseVisualStyleBackColor = true;
            this.mBTNDiscardChanges.Click += new System.EventHandler(this.mBTNDiscardChanges_Click);
            // 
            // mBTNReset
            // 
            this.mBTNReset.Location = new System.Drawing.Point(354, 3);
            this.mBTNReset.Name = "mBTNReset";
            this.mBTNReset.Size = new System.Drawing.Size(111, 23);
            this.mBTNReset.TabIndex = 1;
            this.mBTNReset.Text = "Reset Devices";
            this.mBTNReset.UseVisualStyleBackColor = true;
            this.mBTNReset.Click += new System.EventHandler(this.mBTNReset_Click);
            // 
            // mBTNApplyChanges
            // 
            this.mBTNApplyChanges.Enabled = false;
            this.mBTNApplyChanges.Location = new System.Drawing.Point(120, 3);
            this.mBTNApplyChanges.Name = "mBTNApplyChanges";
            this.mBTNApplyChanges.Size = new System.Drawing.Size(111, 23);
            this.mBTNApplyChanges.TabIndex = 1;
            this.mBTNApplyChanges.Text = "Apply Changes";
            this.mBTNApplyChanges.UseVisualStyleBackColor = true;
            this.mBTNApplyChanges.Click += new System.EventHandler(this.mBTNApplyChanges_Click);
            // 
            // mBTNDiscover
            // 
            this.mBTNDiscover.Location = new System.Drawing.Point(3, 3);
            this.mBTNDiscover.Name = "mBTNDiscover";
            this.mBTNDiscover.Size = new System.Drawing.Size(111, 23);
            this.mBTNDiscover.TabIndex = 0;
            this.mBTNDiscover.Text = "Search Devices";
            this.mBTNDiscover.UseVisualStyleBackColor = true;
            this.mBTNDiscover.Click += new System.EventHandler(this.mBTNDiscover_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.mBTNAssignAddressRange);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(742, 311);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(263, 54);
            this.panel2.TabIndex = 3;
            // 
            // mBTNAssignAddressRange
            // 
            this.mBTNAssignAddressRange.Location = new System.Drawing.Point(3, 3);
            this.mBTNAssignAddressRange.Name = "mBTNAssignAddressRange";
            this.mBTNAssignAddressRange.Size = new System.Drawing.Size(153, 23);
            this.mBTNAssignAddressRange.TabIndex = 3;
            this.mBTNAssignAddressRange.Text = "Assign Address Range ...";
            this.mBTNAssignAddressRange.UseVisualStyleBackColor = true;
            this.mBTNAssignAddressRange.Click += new System.EventHandler(this.mBTNAssignAddressRange_Click);
            // 
            // mFunctionTabs
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.mFunctionTabs, 2);
            this.mFunctionTabs.Controls.Add(this.mTPLedControl);
            this.mFunctionTabs.Controls.Add(this.mTPStressTesting);
            this.mFunctionTabs.Controls.Add(this.mTPInputMonitor);
            this.mFunctionTabs.Controls.Add(this.mTPErrorReporting);
            this.mFunctionTabs.Controls.Add(this.mTPLog);
            this.mFunctionTabs.Controls.Add(this.mTPProcessManager);
            this.mFunctionTabs.Controls.Add(this.tabPage1);
            this.mFunctionTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mFunctionTabs.Location = new System.Drawing.Point(3, 371);
            this.mFunctionTabs.Name = "mFunctionTabs";
            this.mFunctionTabs.SelectedIndex = 0;
            this.mFunctionTabs.Size = new System.Drawing.Size(1002, 211);
            this.mFunctionTabs.TabIndex = 4;
            // 
            // mTPLedControl
            // 
            this.mTPLedControl.BackColor = System.Drawing.SystemColors.Control;
            this.mTPLedControl.Controls.Add(this.mTBBrightness);
            this.mTPLedControl.Controls.Add(this.mCBLedColor);
            this.mTPLedControl.Controls.Add(this.mTrackBrightness);
            this.mTPLedControl.Controls.Add(this.label17);
            this.mTPLedControl.Controls.Add(this.label4);
            this.mTPLedControl.Controls.Add(this.label3);
            this.mTPLedControl.Controls.Add(this.mBTNShowDevice);
            this.mTPLedControl.Controls.Add(this.label2);
            this.mTPLedControl.Controls.Add(this.mTBLedID);
            this.mTPLedControl.Controls.Add(this.label1);
            this.mTPLedControl.Controls.Add(this.mRBSingleLed);
            this.mTPLedControl.Controls.Add(this.mTBLedText);
            this.mTPLedControl.Controls.Add(this.mRBAllLeds);
            this.mTPLedControl.Controls.Add(this.mBTNClearAllLeds);
            this.mTPLedControl.Controls.Add(this.mBTNShowLedIds);
            this.mTPLedControl.Controls.Add(this.mBTNSetLedColor);
            this.mTPLedControl.Controls.Add(this.mBTNClearLed);
            this.mTPLedControl.Controls.Add(this.mCBFlashMode);
            this.mTPLedControl.Location = new System.Drawing.Point(4, 22);
            this.mTPLedControl.Name = "mTPLedControl";
            this.mTPLedControl.Padding = new System.Windows.Forms.Padding(3);
            this.mTPLedControl.Size = new System.Drawing.Size(994, 185);
            this.mTPLedControl.TabIndex = 0;
            this.mTPLedControl.Text = "Led Control";
            // 
            // mTBBrightness
            // 
            this.mTBBrightness.Location = new System.Drawing.Point(228, 92);
            this.mTBBrightness.Name = "mTBBrightness";
            this.mTBBrightness.ReadOnly = true;
            this.mTBBrightness.Size = new System.Drawing.Size(34, 20);
            this.mTBBrightness.TabIndex = 22;
            this.mTBBrightness.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // mCBLedColor
            // 
            this.mCBLedColor.FormattingEnabled = true;
            this.mCBLedColor.Location = new System.Drawing.Point(72, 122);
            this.mCBLedColor.Name = "mCBLedColor";
            this.mCBLedColor.Size = new System.Drawing.Size(150, 21);
            this.mCBLedColor.TabIndex = 10;
            // 
            // mTrackBrightness
            // 
            this.mTrackBrightness.Location = new System.Drawing.Point(71, 92);
            this.mTrackBrightness.Maximum = 255;
            this.mTrackBrightness.Minimum = 8;
            this.mTrackBrightness.Name = "mTrackBrightness";
            this.mTrackBrightness.Size = new System.Drawing.Size(151, 45);
            this.mTrackBrightness.TabIndex = 21;
            this.mTrackBrightness.TickStyle = System.Windows.Forms.TickStyle.None;
            this.mTrackBrightness.Value = 8;
            this.mTrackBrightness.Scroll += new System.EventHandler(this.mTrackBrightness_Scroll);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 96);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(59, 13);
            this.label17.TabIndex = 20;
            this.label17.Text = "Brightness:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Text:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Color:";
            // 
            // mBTNShowDevice
            // 
            this.mBTNShowDevice.Location = new System.Drawing.Point(6, 6);
            this.mBTNShowDevice.Name = "mBTNShowDevice";
            this.mBTNShowDevice.Size = new System.Drawing.Size(105, 23);
            this.mBTNShowDevice.TabIndex = 0;
            this.mBTNShowDevice.Text = "Show Device";
            this.mBTNShowDevice.UseVisualStyleBackColor = true;
            this.mBTNShowDevice.Click += new System.EventHandler(this.mBTNShowDevice_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Flash Mode:";
            // 
            // mTBLedID
            // 
            this.mTBLedID.Location = new System.Drawing.Point(92, 42);
            this.mTBLedID.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.mTBLedID.Name = "mTBLedID";
            this.mTBLedID.Size = new System.Drawing.Size(62, 20);
            this.mTBLedID.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "LedID:";
            // 
            // mRBSingleLed
            // 
            this.mRBSingleLed.AutoSize = true;
            this.mRBSingleLed.Location = new System.Drawing.Point(72, 44);
            this.mRBSingleLed.Name = "mRBSingleLed";
            this.mRBSingleLed.Size = new System.Drawing.Size(14, 13);
            this.mRBSingleLed.TabIndex = 3;
            this.mRBSingleLed.TabStop = true;
            this.mRBSingleLed.UseVisualStyleBackColor = true;
            // 
            // mTBLedText
            // 
            this.mTBLedText.Location = new System.Drawing.Point(72, 149);
            this.mTBLedText.Name = "mTBLedText";
            this.mTBLedText.Size = new System.Drawing.Size(150, 20);
            this.mTBLedText.TabIndex = 15;
            // 
            // mRBAllLeds
            // 
            this.mRBAllLeds.AutoSize = true;
            this.mRBAllLeds.Location = new System.Drawing.Point(160, 42);
            this.mRBAllLeds.Name = "mRBAllLeds";
            this.mRBAllLeds.Size = new System.Drawing.Size(62, 17);
            this.mRBAllLeds.TabIndex = 4;
            this.mRBAllLeds.TabStop = true;
            this.mRBAllLeds.Text = "All Leds";
            this.mRBAllLeds.UseVisualStyleBackColor = true;
            // 
            // mBTNClearAllLeds
            // 
            this.mBTNClearAllLeds.Location = new System.Drawing.Point(116, 6);
            this.mBTNClearAllLeds.Name = "mBTNClearAllLeds";
            this.mBTNClearAllLeds.Size = new System.Drawing.Size(105, 23);
            this.mBTNClearAllLeds.TabIndex = 14;
            this.mBTNClearAllLeds.Text = "Clear All";
            this.mBTNClearAllLeds.UseVisualStyleBackColor = true;
            this.mBTNClearAllLeds.Click += new System.EventHandler(this.mBTNClearAllLeds_Click);
            //
            // mBTNShowLedIds
            //
            this.mBTNShowLedIds.Location = new System.Drawing.Point(227, 6);
            this.mBTNShowLedIds.Name = "mBTNShowLedIds";
            this.mBTNShowLedIds.Size = new System.Drawing.Size(105, 23);
            this.mBTNShowLedIds.TabIndex = 15;
            this.mBTNShowLedIds.Text = "Show Led IDs";
            this.mBTNShowLedIds.UseVisualStyleBackColor = true;
            this.mBTNShowLedIds.Click += new System.EventHandler(this.mBTNShowLedIds_Click);
            // 
            // mBTNSetLedColor
            // 
            this.mBTNSetLedColor.Location = new System.Drawing.Point(228, 120);
            this.mBTNSetLedColor.Name = "mBTNSetLedColor";
            this.mBTNSetLedColor.Size = new System.Drawing.Size(105, 23);
            this.mBTNSetLedColor.TabIndex = 5;
            this.mBTNSetLedColor.Text = "Set";
            this.mBTNSetLedColor.UseVisualStyleBackColor = true;
            this.mBTNSetLedColor.Click += new System.EventHandler(this.mBTNSetLedColor_Click);
            // 
            // mBTNClearLed
            // 
            this.mBTNClearLed.Location = new System.Drawing.Point(228, 147);
            this.mBTNClearLed.Name = "mBTNClearLed";
            this.mBTNClearLed.Size = new System.Drawing.Size(105, 23);
            this.mBTNClearLed.TabIndex = 13;
            this.mBTNClearLed.Text = "Clear";
            this.mBTNClearLed.UseVisualStyleBackColor = true;
            this.mBTNClearLed.Click += new System.EventHandler(this.mBTNClearLed_Click);
            // 
            // mCBFlashMode
            // 
            this.mCBFlashMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mCBFlashMode.FormattingEnabled = true;
            this.mCBFlashMode.Items.AddRange(new object[] {
            "Static",
            "Blink Slow",
            "Blink Middle",
            "Blink Fast"});
            this.mCBFlashMode.Location = new System.Drawing.Point(72, 65);
            this.mCBFlashMode.Name = "mCBFlashMode";
            this.mCBFlashMode.Size = new System.Drawing.Size(150, 21);
            this.mCBFlashMode.TabIndex = 11;
            // 
            // mTPStressTesting
            // 
            this.mTPStressTesting.BackColor = System.Drawing.SystemColors.Control;
            this.mTPStressTesting.Controls.Add(this.mBTNStopStressTesting);
            this.mTPStressTesting.Controls.Add(this.label6);
            this.mTPStressTesting.Controls.Add(this.mTBSendTimeout);
            this.mTPStressTesting.Controls.Add(this.mBTNStressTestStart);
            this.mTPStressTesting.Controls.Add(this.mLBLPacketsSec);
            this.mTPStressTesting.Controls.Add(this.label5);
            this.mTPStressTesting.Controls.Add(this.mLBLFailedPackets);
            this.mTPStressTesting.Controls.Add(this.mLBLSendPackets);
            this.mTPStressTesting.Controls.Add(this.mLBLRetries);
            this.mTPStressTesting.Controls.Add(this.label7);
            this.mTPStressTesting.Controls.Add(this.mTBRetryDelay);
            this.mTPStressTesting.Controls.Add(this.label8);
            this.mTPStressTesting.Controls.Add(this.label11);
            this.mTPStressTesting.Controls.Add(this.label9);
            this.mTPStressTesting.Controls.Add(this.mTBMaxRetries);
            this.mTPStressTesting.Controls.Add(this.label10);
            this.mTPStressTesting.Location = new System.Drawing.Point(4, 22);
            this.mTPStressTesting.Name = "mTPStressTesting";
            this.mTPStressTesting.Padding = new System.Windows.Forms.Padding(3);
            this.mTPStressTesting.Size = new System.Drawing.Size(994, 185);
            this.mTPStressTesting.TabIndex = 1;
            this.mTPStressTesting.Text = "Stress Testing";
            // 
            // mBTNStopStressTesting
            // 
            this.mBTNStopStressTesting.Location = new System.Drawing.Point(6, 34);
            this.mBTNStopStressTesting.Name = "mBTNStopStressTesting";
            this.mBTNStopStressTesting.Size = new System.Drawing.Size(105, 23);
            this.mBTNStopStressTesting.TabIndex = 16;
            this.mBTNStopStressTesting.Text = "Stop Test";
            this.mBTNStopStressTesting.UseVisualStyleBackColor = true;
            this.mBTNStopStressTesting.Click += new System.EventHandler(this.mBTNStopStressTesting_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(121, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Send Timeout:";
            // 
            // mTBSendTimeout
            // 
            this.mTBSendTimeout.Location = new System.Drawing.Point(202, 6);
            this.mTBSendTimeout.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.mTBSendTimeout.Name = "mTBSendTimeout";
            this.mTBSendTimeout.Size = new System.Drawing.Size(50, 20);
            this.mTBSendTimeout.TabIndex = 14;
            // 
            // mBTNStressTestStart
            // 
            this.mBTNStressTestStart.Location = new System.Drawing.Point(6, 6);
            this.mBTNStressTestStart.Name = "mBTNStressTestStart";
            this.mBTNStressTestStart.Size = new System.Drawing.Size(105, 23);
            this.mBTNStressTestStart.TabIndex = 1;
            this.mBTNStressTestStart.Text = "Start Test";
            this.mBTNStressTestStart.UseVisualStyleBackColor = true;
            this.mBTNStressTestStart.Click += new System.EventHandler(this.mBTNStressTestStart_Click);
            // 
            // mLBLPacketsSec
            // 
            this.mLBLPacketsSec.AutoSize = true;
            this.mLBLPacketsSec.Location = new System.Drawing.Point(90, 130);
            this.mLBLPacketsSec.Name = "mLBLPacketsSec";
            this.mLBLPacketsSec.Size = new System.Drawing.Size(13, 13);
            this.mLBLPacketsSec.TabIndex = 13;
            this.mLBLPacketsSec.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 85);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Send Packets:";
            // 
            // mLBLFailedPackets
            // 
            this.mLBLFailedPackets.AutoSize = true;
            this.mLBLFailedPackets.Location = new System.Drawing.Point(90, 115);
            this.mLBLFailedPackets.Name = "mLBLFailedPackets";
            this.mLBLFailedPackets.Size = new System.Drawing.Size(13, 13);
            this.mLBLFailedPackets.TabIndex = 12;
            this.mLBLFailedPackets.Text = "0";
            // 
            // mLBLSendPackets
            // 
            this.mLBLSendPackets.AutoSize = true;
            this.mLBLSendPackets.Location = new System.Drawing.Point(90, 85);
            this.mLBLSendPackets.Name = "mLBLSendPackets";
            this.mLBLSendPackets.Size = new System.Drawing.Size(13, 13);
            this.mLBLSendPackets.TabIndex = 3;
            this.mLBLSendPackets.Text = "0";
            // 
            // mLBLRetries
            // 
            this.mLBLRetries.AutoSize = true;
            this.mLBLRetries.Location = new System.Drawing.Point(90, 100);
            this.mLBLRetries.Name = "mLBLRetries";
            this.mLBLRetries.Size = new System.Drawing.Size(13, 13);
            this.mLBLRetries.TabIndex = 11;
            this.mLBLRetries.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Retries:";
            // 
            // mTBRetryDelay
            // 
            this.mTBRetryDelay.Location = new System.Drawing.Point(202, 58);
            this.mTBRetryDelay.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.mTBRetryDelay.Name = "mTBRetryDelay";
            this.mTBRetryDelay.Size = new System.Drawing.Size(51, 20);
            this.mTBRetryDelay.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 130);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Packets/Sec:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(121, 60);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 13);
            this.label11.TabIndex = 9;
            this.label11.Text = "Retry Delay:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 115);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Failed Packets:";
            // 
            // mTBMaxRetries
            // 
            this.mTBMaxRetries.Location = new System.Drawing.Point(202, 32);
            this.mTBMaxRetries.Name = "mTBMaxRetries";
            this.mTBMaxRetries.Size = new System.Drawing.Size(50, 20);
            this.mTBMaxRetries.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(121, 34);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 13);
            this.label10.TabIndex = 7;
            this.label10.Text = "Max Retries:";
            // 
            // mTPInputMonitor
            // 
            this.mTPInputMonitor.BackColor = System.Drawing.SystemColors.Control;
            this.mTPInputMonitor.Controls.Add(this.mBTNDebugSend);
            this.mTPInputMonitor.Controls.Add(this.mCBActivateInput);
            this.mTPInputMonitor.Controls.Add(this.mLBLActivateInput);
            this.mTPInputMonitor.Controls.Add(this.mCBInputSelect);
            this.mTPInputMonitor.Controls.Add(this.mTBReceivedInput);
            this.mTPInputMonitor.Location = new System.Drawing.Point(4, 22);
            this.mTPInputMonitor.Name = "mTPInputMonitor";
            this.mTPInputMonitor.Padding = new System.Windows.Forms.Padding(3);
            this.mTPInputMonitor.Size = new System.Drawing.Size(994, 185);
            this.mTPInputMonitor.TabIndex = 2;
            this.mTPInputMonitor.Text = "Input Monitor";
            // 
            // mBTNDebugSend
            // 
            this.mBTNDebugSend.Location = new System.Drawing.Point(6, 63);
            this.mBTNDebugSend.Name = "mBTNDebugSend";
            this.mBTNDebugSend.Size = new System.Drawing.Size(121, 23);
            this.mBTNDebugSend.TabIndex = 6;
            this.mBTNDebugSend.Text = "Debug Send";
            this.mBTNDebugSend.UseVisualStyleBackColor = true;
            this.mBTNDebugSend.Click += new System.EventHandler(this.mBTNDebugSend_Click);
            // 
            // mLBLActivateInput
            // 
            this.mLBLActivateInput.AutoSize = false;
            this.mLBLActivateInput.Location = new System.Drawing.Point(6, 36);
            this.mLBLActivateInput.Name = "mLBLActivateInput";
            this.mLBLActivateInput.Size = new System.Drawing.Size(80, 15);
            this.mLBLActivateInput.TabIndex = 20;
            this.mLBLActivateInput.Text = "Activate Input";
            // 
            // mCBActivateInput
            // 
            this.mCBActivateInput.Location = new System.Drawing.Point(90, 33);
            this.mCBActivateInput.Name = "mCBActivateInput";
            this.mCBActivateInput.Size = new System.Drawing.Size(36, 20);
            this.mCBActivateInput.TabIndex = 5;
            this.mCBActivateInput.Click += new System.EventHandler(this.mCBActivateInput_Click);
            // 
            // mCBInputSelect
            // 
            this.mCBInputSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mCBInputSelect.FormattingEnabled = true;
            this.mCBInputSelect.Location = new System.Drawing.Point(6, 6);
            this.mCBInputSelect.Name = "mCBInputSelect";
            this.mCBInputSelect.Size = new System.Drawing.Size(121, 21);
            this.mCBInputSelect.TabIndex = 4;
            this.mCBInputSelect.DropDown += new System.EventHandler(this.mCBInputSelect_DropDown);
            // 
            // mTBReceivedInput
            // 
            this.mTBReceivedInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mTBReceivedInput.Location = new System.Drawing.Point(133, 6);
            this.mTBReceivedInput.Multiline = true;
            this.mTBReceivedInput.Name = "mTBReceivedInput";
            this.mTBReceivedInput.ReadOnly = true;
            this.mTBReceivedInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mTBReceivedInput.Size = new System.Drawing.Size(829, 173);
            this.mTBReceivedInput.TabIndex = 3;
            // 
            // mTPErrorReporting
            // 
            this.mTPErrorReporting.BackColor = System.Drawing.SystemColors.Control;
            this.mTPErrorReporting.Controls.Add(this.mBTNSaveLog);
            this.mTPErrorReporting.Controls.Add(this.mBTNClearErrors);
            this.mTPErrorReporting.Controls.Add(this.mOLVErrors);
            this.mTPErrorReporting.Controls.Add(this.mBTNReadErrors);
            this.mTPErrorReporting.Location = new System.Drawing.Point(4, 22);
            this.mTPErrorReporting.Name = "mTPErrorReporting";
            this.mTPErrorReporting.Padding = new System.Windows.Forms.Padding(3);
            this.mTPErrorReporting.Size = new System.Drawing.Size(994, 185);
            this.mTPErrorReporting.TabIndex = 3;
            this.mTPErrorReporting.Text = "Error Reporting";
            // 
            // mBTNSaveLog
            // 
            this.mBTNSaveLog.Location = new System.Drawing.Point(6, 64);
            this.mBTNSaveLog.Name = "mBTNSaveLog";
            this.mBTNSaveLog.Size = new System.Drawing.Size(75, 23);
            this.mBTNSaveLog.TabIndex = 3;
            this.mBTNSaveLog.Text = "Save Log ...";
            this.mBTNSaveLog.UseVisualStyleBackColor = true;
            this.mBTNSaveLog.Click += new System.EventHandler(this.mBTNSaveLog_Click);
            // 
            // mBTNClearErrors
            // 
            this.mBTNClearErrors.Location = new System.Drawing.Point(6, 35);
            this.mBTNClearErrors.Name = "mBTNClearErrors";
            this.mBTNClearErrors.Size = new System.Drawing.Size(75, 23);
            this.mBTNClearErrors.TabIndex = 2;
            this.mBTNClearErrors.Text = "Clear Errors";
            this.mBTNClearErrors.UseVisualStyleBackColor = true;
            this.mBTNClearErrors.Click += new System.EventHandler(this.mBTNClearErrors_Click);
            // 
            // mOLVErrors
            // 
            this.mOLVErrors.AllColumns.Add(this.olvColumn7);
            this.mOLVErrors.AllColumns.Add(this.olvColumn11);
            this.mOLVErrors.AllColumns.Add(this.olvColumn8);
            this.mOLVErrors.AllColumns.Add(this.olvColumn9);
            this.mOLVErrors.AllColumns.Add(this.olvColumn10);
            this.mOLVErrors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mOLVErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn7,
            this.olvColumn11,
            this.olvColumn8,
            this.olvColumn9,
            this.olvColumn10});
            this.mOLVErrors.FullRowSelect = true;
            this.mOLVErrors.HeaderUsesThemes = false;
            this.mOLVErrors.HideSelection = false;
            this.mOLVErrors.Location = new System.Drawing.Point(87, 6);
            this.mOLVErrors.Name = "mOLVErrors";
            this.mOLVErrors.ShowGroups = false;
            this.mOLVErrors.Size = new System.Drawing.Size(874, 173);
            this.mOLVErrors.TabIndex = 1;
            this.mOLVErrors.UseCompatibleStateImageBehavior = false;
            this.mOLVErrors.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn7
            // 
            this.olvColumn7.AspectName = "TimeStamp";
            this.olvColumn7.CellPadding = null;
            this.olvColumn7.Groupable = false;
            this.olvColumn7.Text = "Time";
            this.olvColumn7.Width = 125;
            // 
            // olvColumn11
            // 
            this.olvColumn11.AspectName = "SerialNumber";
            this.olvColumn11.CellPadding = null;
            this.olvColumn11.Text = "Serialnumber";
            this.olvColumn11.Width = 100;
            // 
            // olvColumn8
            // 
            this.olvColumn8.AspectName = "Age";
            this.olvColumn8.CellPadding = null;
            this.olvColumn8.Groupable = false;
            this.olvColumn8.Text = "Age";
            this.olvColumn8.Width = 82;
            // 
            // olvColumn9
            // 
            this.olvColumn9.AspectName = "ErrorCode";
            this.olvColumn9.CellPadding = null;
            this.olvColumn9.Text = "Code";
            // 
            // olvColumn10
            // 
            this.olvColumn10.AspectName = "ErrorText";
            this.olvColumn10.CellPadding = null;
            this.olvColumn10.Groupable = false;
            this.olvColumn10.Text = "Text";
            this.olvColumn10.Width = 500;
            // 
            // mBTNReadErrors
            // 
            this.mBTNReadErrors.Location = new System.Drawing.Point(6, 6);
            this.mBTNReadErrors.Name = "mBTNReadErrors";
            this.mBTNReadErrors.Size = new System.Drawing.Size(75, 23);
            this.mBTNReadErrors.TabIndex = 0;
            this.mBTNReadErrors.Text = "Read Errors";
            this.mBTNReadErrors.UseVisualStyleBackColor = true;
            this.mBTNReadErrors.Click += new System.EventHandler(this.mBTNReadErrors_Click);
            // 
            // mTPLog
            // 
            this.mTPLog.BackColor = System.Drawing.SystemColors.Control;
            this.mTPLog.Controls.Add(this.mBTNReadLog);
            this.mTPLog.Controls.Add(this.mTBLogs);
            this.mTPLog.Location = new System.Drawing.Point(4, 22);
            this.mTPLog.Name = "mTPLog";
            this.mTPLog.Padding = new System.Windows.Forms.Padding(3);
            this.mTPLog.Size = new System.Drawing.Size(994, 185);
            this.mTPLog.TabIndex = 6;
            this.mTPLog.Text = "Logs";
            // 
            // mBTNReadLog
            // 
            this.mBTNReadLog.Location = new System.Drawing.Point(6, 6);
            this.mBTNReadLog.Name = "mBTNReadLog";
            this.mBTNReadLog.Size = new System.Drawing.Size(104, 23);
            this.mBTNReadLog.TabIndex = 1;
            this.mBTNReadLog.Text = "Read Log";
            this.mBTNReadLog.UseVisualStyleBackColor = true;
            this.mBTNReadLog.Click += new System.EventHandler(this.mBTNReadLog_Click);
            // 
            // mTBLogs
            // 
            this.mTBLogs.Location = new System.Drawing.Point(116, 6);
            this.mTBLogs.Multiline = true;
            this.mTBLogs.Name = "mTBLogs";
            this.mTBLogs.ReadOnly = true;
            this.mTBLogs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mTBLogs.Size = new System.Drawing.Size(845, 173);
            this.mTBLogs.TabIndex = 0;
            // 
            // mTPProcessManager
            // 
            this.mTPProcessManager.BackColor = System.Drawing.SystemColors.Control;
            this.mTPProcessManager.Controls.Add(this.mBTNFullRestore);
            this.mTPProcessManager.Controls.Add(this.mBTNFullBackup);
            this.mTPProcessManager.Controls.Add(this.mBTNDeleteProcess);
            this.mTPProcessManager.Controls.Add(this.mBTNRestoreProcess);
            this.mTPProcessManager.Controls.Add(this.mBTNBackupProcess);
            this.mTPProcessManager.Controls.Add(this.mBTNUpdateProcessList);
            this.mTPProcessManager.Controls.Add(this.mOLVProcesses);
            this.mTPProcessManager.Location = new System.Drawing.Point(4, 22);
            this.mTPProcessManager.Name = "mTPProcessManager";
            this.mTPProcessManager.Padding = new System.Windows.Forms.Padding(3);
            this.mTPProcessManager.Size = new System.Drawing.Size(994, 185);
            this.mTPProcessManager.TabIndex = 4;
            this.mTPProcessManager.Text = "Process Manager";
            // 
            // mBTNDeleteProcess
            // 
            this.mBTNDeleteProcess.Location = new System.Drawing.Point(6, 151);
            this.mBTNDeleteProcess.Name = "mBTNDeleteProcess";
            this.mBTNDeleteProcess.Size = new System.Drawing.Size(111, 23);
            this.mBTNDeleteProcess.TabIndex = 4;
            this.mBTNDeleteProcess.Text = "Delete";
            this.mBTNDeleteProcess.UseVisualStyleBackColor = true;
            this.mBTNDeleteProcess.Click += new System.EventHandler(this.mBTNDeleteProcess_Click);
            // 
            // mBTNRestoreProcess
            // 
            this.mBTNRestoreProcess.Location = new System.Drawing.Point(6, 122);
            this.mBTNRestoreProcess.Name = "mBTNRestoreProcess";
            this.mBTNRestoreProcess.Size = new System.Drawing.Size(111, 23);
            this.mBTNRestoreProcess.TabIndex = 3;
            this.mBTNRestoreProcess.Text = "Restore ...";
            this.mBTNRestoreProcess.UseVisualStyleBackColor = true;
            this.mBTNRestoreProcess.Click += new System.EventHandler(this.mBTNRestoreProcess_Click);
            // 
            // mBTNBackupProcess
            // 
            this.mBTNBackupProcess.Location = new System.Drawing.Point(6, 93);
            this.mBTNBackupProcess.Name = "mBTNBackupProcess";
            this.mBTNBackupProcess.Size = new System.Drawing.Size(111, 23);
            this.mBTNBackupProcess.TabIndex = 2;
            this.mBTNBackupProcess.Text = "Backup ...";
            this.mBTNBackupProcess.UseVisualStyleBackColor = true;
            this.mBTNBackupProcess.Click += new System.EventHandler(this.mBTNBackupProcess_Click);
            // 
            // mBTNUpdateProcessList
            // 
            this.mBTNUpdateProcessList.Location = new System.Drawing.Point(6, 6);
            this.mBTNUpdateProcessList.Name = "mBTNUpdateProcessList";
            this.mBTNUpdateProcessList.Size = new System.Drawing.Size(111, 23);
            this.mBTNUpdateProcessList.TabIndex = 1;
            this.mBTNUpdateProcessList.Text = "Update List";
            this.mBTNUpdateProcessList.UseVisualStyleBackColor = true;
            this.mBTNUpdateProcessList.Click += new System.EventHandler(this.mBTNUpdateProcessList_Click);
            // 
            // mOLVProcesses
            // 
            this.mOLVProcesses.AllColumns.Add(this.olvColumn12);
            this.mOLVProcesses.AllColumns.Add(this.olvColumn13);
            this.mOLVProcesses.AllColumns.Add(this.olvColumn14);
            this.mOLVProcesses.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mOLVProcesses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn12,
            this.olvColumn13,
            this.olvColumn14});
            this.mOLVProcesses.FullRowSelect = true;
            this.mOLVProcesses.HideSelection = false;
            this.mOLVProcesses.Location = new System.Drawing.Point(123, 6);
            this.mOLVProcesses.Name = "mOLVProcesses";
            this.mOLVProcesses.ShowGroups = false;
            this.mOLVProcesses.Size = new System.Drawing.Size(838, 173);
            this.mOLVProcesses.TabIndex = 0;
            this.mOLVProcesses.UseCompatibleStateImageBehavior = false;
            this.mOLVProcesses.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn12
            // 
            this.olvColumn12.AspectName = "Name";
            this.olvColumn12.CellPadding = null;
            this.olvColumn12.Text = "Name";
            this.olvColumn12.Width = 193;
            // 
            // olvColumn13
            // 
            this.olvColumn13.AspectName = "ID";
            this.olvColumn13.CellPadding = null;
            this.olvColumn13.Text = "ID";
            // 
            // olvColumn14
            // 
            this.olvColumn14.AspectName = "ByteSize";
            this.olvColumn14.CellPadding = null;
            this.olvColumn14.Text = "Size (Bytes)";
            this.olvColumn14.Width = 81;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.mTBInputPort);
            this.tabPage1.Controls.Add(this.label18);
            this.tabPage1.Controls.Add(this.label16);
            this.tabPage1.Controls.Add(this.label15);
            this.tabPage1.Controls.Add(this.mTBGlobalRetryDelay);
            this.tabPage1.Controls.Add(this.label14);
            this.tabPage1.Controls.Add(this.mTBMaxSendRetries);
            this.tabPage1.Controls.Add(this.label13);
            this.tabPage1.Controls.Add(this.mTBNetworkMessageTimeout);
            this.tabPage1.Controls.Add(this.label12);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(994, 185);
            this.tabPage1.TabIndex = 5;
            this.tabPage1.Text = "iXnet Manager Settings";
            // 
            // mTBInputPort
            // 
            this.mTBInputPort.Location = new System.Drawing.Point(149, 90);
            this.mTBInputPort.Name = "mTBInputPort";
            this.mTBInputPort.ReadOnly = true;
            this.mTBInputPort.Size = new System.Drawing.Size(50, 20);
            this.mTBInputPort.TabIndex = 9;
            this.mTBInputPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(87, 93);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(56, 13);
            this.label18.TabIndex = 8;
            this.label18.Text = "Input Port:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(207, 67);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(20, 13);
            this.label16.TabIndex = 7;
            this.label16.Text = "ms";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(207, 14);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(20, 13);
            this.label15.TabIndex = 6;
            this.label15.Text = "ms";
            // 
            // mTBGlobalRetryDelay
            // 
            this.mTBGlobalRetryDelay.Location = new System.Drawing.Point(149, 64);
            this.mTBGlobalRetryDelay.Name = "mTBGlobalRetryDelay";
            this.mTBGlobalRetryDelay.Size = new System.Drawing.Size(50, 20);
            this.mTBGlobalRetryDelay.TabIndex = 5;
            this.mTBGlobalRetryDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(78, 67);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 13);
            this.label14.TabIndex = 4;
            this.label14.Text = "Retry Delay:";
            // 
            // mTBMaxSendRetries
            // 
            this.mTBMaxSendRetries.Location = new System.Drawing.Point(149, 38);
            this.mTBMaxSendRetries.Name = "mTBMaxSendRetries";
            this.mTBMaxSendRetries.Size = new System.Drawing.Size(50, 20);
            this.mTBMaxSendRetries.TabIndex = 3;
            this.mTBMaxSendRetries.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(72, 41);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(71, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "Send Retries:";
            // 
            // mTBNetworkMessageTimeout
            // 
            this.mTBNetworkMessageTimeout.Location = new System.Drawing.Point(149, 11);
            this.mTBNetworkMessageTimeout.Name = "mTBNetworkMessageTimeout";
            this.mTBNetworkMessageTimeout.Size = new System.Drawing.Size(50, 20);
            this.mTBNetworkMessageTimeout.TabIndex = 1;
            this.mTBNetworkMessageTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 14);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(137, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Network Message Timeout:";
            // 
            // timer1
            // 
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // mBTNFullBackup
            // 
            this.mBTNFullBackup.Location = new System.Drawing.Point(6, 35);
            this.mBTNFullBackup.Name = "mBTNFullBackup";
            this.mBTNFullBackup.Size = new System.Drawing.Size(111, 23);
            this.mBTNFullBackup.TabIndex = 5;
            this.mBTNFullBackup.Text = "Full Backup ...";
            this.mBTNFullBackup.UseVisualStyleBackColor = true;
            this.mBTNFullBackup.Click += new System.EventHandler(this.mBTNFullBackup_Click);
            // 
            // mBTNFullRestore
            // 
            this.mBTNFullRestore.Location = new System.Drawing.Point(6, 64);
            this.mBTNFullRestore.Name = "mBTNFullRestore";
            this.mBTNFullRestore.Size = new System.Drawing.Size(111, 23);
            this.mBTNFullRestore.TabIndex = 6;
            this.mBTNFullRestore.Text = "Full Restore ...";
            this.mBTNFullRestore.UseVisualStyleBackColor = true;
            this.mBTNFullRestore.Click += new System.EventHandler(this.mBTNFullRestore_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 621);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(997, 659);
            this.Name = "MainForm";
            this.Text = "iXnet Manager";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mLBDevices)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.mFunctionTabs.ResumeLayout(false);
            this.mTPLedControl.ResumeLayout(false);
            this.mTPLedControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mTBLedID)).EndInit();
            this.mTPStressTesting.ResumeLayout(false);
            this.mTPStressTesting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mTBSendTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mTBRetryDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mTBMaxRetries)).EndInit();
            this.mTPInputMonitor.ResumeLayout(false);
            this.mTPInputMonitor.PerformLayout();
            this.mTPErrorReporting.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mOLVErrors)).EndInit();
            this.mTPLog.ResumeLayout(false);
            this.mTPLog.PerformLayout();
            this.mTPProcessManager.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mOLVProcesses)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private BrightIdeasSoftware.ObjectListView mLBDevices;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private System.Windows.Forms.PropertyGrid mPGDevices;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button mBTNReset;
        private System.Windows.Forms.Button mBTNDiscover;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button mBTNDiscardChanges;
        private System.Windows.Forms.Button mBTNApplyChanges;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private BrightIdeasSoftware.OLVColumn olvColumn3;
        private BrightIdeasSoftware.OLVColumn olvColumn4;
        private System.Windows.Forms.Button mBTNAssignAddressRange;
        private BrightIdeasSoftware.OLVColumn olvColumn5;
        private System.Windows.Forms.Button mBTNFlashDevices;
        private System.Windows.Forms.TextBox mTBReceivedInput;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox mTBLedText;
        private System.Windows.Forms.Button mBTNClearAllLeds;
        private System.Windows.Forms.Button mBTNShowLedIds;
        private System.Windows.Forms.Button mBTNClearLed;
        private System.Windows.Forms.ComboBox mCBFlashMode;
        private System.Windows.Forms.ComboBox mCBLedColor;
        private System.Windows.Forms.Button mBTNSetLedColor;
        private System.Windows.Forms.RadioButton mRBAllLeds;
        private System.Windows.Forms.RadioButton mRBSingleLed;
        private System.Windows.Forms.NumericUpDown mTBLedID;
        private System.Windows.Forms.Button mBTNShowDevice;
        private System.Windows.Forms.Label mLBLSendPackets;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button mBTNStressTestStart;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown mTBRetryDelay;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown mTBMaxRetries;
        private System.Windows.Forms.Label mLBLPacketsSec;
        private System.Windows.Forms.Label mLBLFailedPackets;
        private System.Windows.Forms.Label mLBLRetries;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown mTBSendTimeout;
        private System.Windows.Forms.ComboBox mCBInputSelect;
        private iXnetManager.Controls.ToggleSwitch mCBActivateInput;
        private System.Windows.Forms.Label mLBLActivateInput;
        private System.Windows.Forms.Button mBTNFlashLedModules;
        private BrightIdeasSoftware.OLVColumn olvColumn6;
        private System.Windows.Forms.TabControl mFunctionTabs;
        private System.Windows.Forms.TabPage mTPLedControl;
        private System.Windows.Forms.TabPage mTPStressTesting;
        private System.Windows.Forms.TabPage mTPInputMonitor;
        private System.Windows.Forms.TabPage mTPErrorReporting;
        private BrightIdeasSoftware.ObjectListView mOLVErrors;
        private System.Windows.Forms.Button mBTNReadErrors;
        private BrightIdeasSoftware.OLVColumn olvColumn7;
        private BrightIdeasSoftware.OLVColumn olvColumn8;
        private BrightIdeasSoftware.OLVColumn olvColumn9;
        private BrightIdeasSoftware.OLVColumn olvColumn10;
        private BrightIdeasSoftware.OLVColumn olvColumn11;
        private System.Windows.Forms.Button mBTNClearErrors;
        private System.Windows.Forms.Button mBTNDebugSend;
        private System.Windows.Forms.TabPage mTPProcessManager;
        private System.Windows.Forms.Button mBTNDeleteProcess;
        private System.Windows.Forms.Button mBTNRestoreProcess;
        private System.Windows.Forms.Button mBTNBackupProcess;
        private System.Windows.Forms.Button mBTNUpdateProcessList;
        private BrightIdeasSoftware.ObjectListView mOLVProcesses;
        private BrightIdeasSoftware.OLVColumn olvColumn12;
        private BrightIdeasSoftware.OLVColumn olvColumn13;
        private BrightIdeasSoftware.OLVColumn olvColumn14;
        private BrightIdeasSoftware.OLVColumn mOLVCMode;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox mTBNetworkMessageTimeout;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox mTBMaxSendRetries;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox mTBGlobalRetryDelay;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button mBTNStopStressTesting;
        private System.Windows.Forms.Button mBTNSaveLog;
        private iXnetManager.Controls.ModernSlider mTrackBrightness;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox mTBBrightness;
        private System.Windows.Forms.TabPage mTPLog;
        private System.Windows.Forms.Button mBTNReadLog;
        private System.Windows.Forms.TextBox mTBLogs;
        private System.Windows.Forms.TextBox mTBInputPort;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button mBTNFactoryReset;
        private System.Windows.Forms.Button mBtnAddManually;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button mBTNFullRestore;
        private System.Windows.Forms.Button mBTNFullBackup;
    }
}

