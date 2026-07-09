namespace iXnetManager
{
    partial class FlashProgramMemoryForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mLBLFWName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.mLBLFWSize = new System.Windows.Forms.Label();
            this.mLBLFWVersion = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mBTNAbort = new System.Windows.Forms.Button();
            this.mBTNStart = new System.Windows.Forms.Button();
            this.mBTNClose = new System.Windows.Forms.Button();
            this.mOLVFlashStates = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.mCOLProgress = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn4 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mOLVFlashStates)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.mOLVFlashStates, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(706, 398);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.mLBLFWName);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.mLBLFWSize);
            this.panel1.Controls.Add(this.mLBLFWVersion);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.mBTNAbort);
            this.panel1.Controls.Add(this.mBTNStart);
            this.panel1.Controls.Add(this.mBTNClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 367);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(700, 28);
            this.panel1.TabIndex = 0;
            // 
            // mLBLFWName
            // 
            this.mLBLFWName.AutoEllipsis = true;
            this.mLBLFWName.Location = new System.Drawing.Point(87, 8);
            this.mLBLFWName.Name = "mLBLFWName";
            this.mLBLFWName.Size = new System.Drawing.Size(85, 13);
            this.mLBLFWName.TabIndex = 8;
            this.mLBLFWName.Text = "label3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "New Firmware:";
            // 
            // mLBLFWSize
            // 
            this.mLBLFWSize.AutoSize = true;
            this.mLBLFWSize.Location = new System.Drawing.Point(341, 8);
            this.mLBLFWSize.Name = "mLBLFWSize";
            this.mLBLFWSize.Size = new System.Drawing.Size(35, 13);
            this.mLBLFWSize.TabIndex = 6;
            this.mLBLFWSize.Text = "label3";
            // 
            // mLBLFWVersion
            // 
            this.mLBLFWVersion.AutoSize = true;
            this.mLBLFWVersion.Location = new System.Drawing.Point(229, 8);
            this.mLBLFWVersion.Name = "mLBLFWVersion";
            this.mLBLFWVersion.Size = new System.Drawing.Size(35, 13);
            this.mLBLFWVersion.TabIndex = 5;
            this.mLBLFWVersion.Text = "label3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(305, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Size:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(178, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Version:";
            // 
            // mBTNAbort
            // 
            this.mBTNAbort.Location = new System.Drawing.Point(540, 3);
            this.mBTNAbort.Name = "mBTNAbort";
            this.mBTNAbort.Size = new System.Drawing.Size(75, 23);
            this.mBTNAbort.TabIndex = 2;
            this.mBTNAbort.Text = "Abort";
            this.mBTNAbort.UseVisualStyleBackColor = true;
            this.mBTNAbort.Click += new System.EventHandler(this.mBTNAbort_Click);
            // 
            // mBTNStart
            // 
            this.mBTNStart.Location = new System.Drawing.Point(459, 3);
            this.mBTNStart.Name = "mBTNStart";
            this.mBTNStart.Size = new System.Drawing.Size(75, 23);
            this.mBTNStart.TabIndex = 1;
            this.mBTNStart.Text = "Start";
            this.mBTNStart.UseVisualStyleBackColor = true;
            this.mBTNStart.Click += new System.EventHandler(this.mBTNStart_Click);
            // 
            // mBTNClose
            // 
            this.mBTNClose.Location = new System.Drawing.Point(621, 3);
            this.mBTNClose.Name = "mBTNClose";
            this.mBTNClose.Size = new System.Drawing.Size(75, 23);
            this.mBTNClose.TabIndex = 0;
            this.mBTNClose.Text = "Close";
            this.mBTNClose.UseVisualStyleBackColor = true;
            this.mBTNClose.Click += new System.EventHandler(this.mBTNClose_Click);
            // 
            // mOLVFlashStates
            // 
            this.mOLVFlashStates.AllColumns.Add(this.olvColumn1);
            this.mOLVFlashStates.AllColumns.Add(this.olvColumn2);
            this.mOLVFlashStates.AllColumns.Add(this.mCOLProgress);
            this.mOLVFlashStates.AllColumns.Add(this.olvColumn4);
            this.mOLVFlashStates.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvColumn2,
            this.mCOLProgress,
            this.olvColumn4});
            this.mOLVFlashStates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mOLVFlashStates.FullRowSelect = true;
            this.mOLVFlashStates.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.mOLVFlashStates.Location = new System.Drawing.Point(3, 3);
            this.mOLVFlashStates.MultiSelect = false;
            this.mOLVFlashStates.Name = "mOLVFlashStates";
            this.mOLVFlashStates.OwnerDraw = true;
            this.mOLVFlashStates.ShowGroups = false;
            this.mOLVFlashStates.Size = new System.Drawing.Size(700, 358);
            this.mOLVFlashStates.TabIndex = 1;
            this.mOLVFlashStates.UseCompatibleStateImageBehavior = false;
            this.mOLVFlashStates.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "SerialNumber";
            this.olvColumn1.CellPadding = null;
            this.olvColumn1.Text = "Serial Number";
            this.olvColumn1.Width = 100;
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "FirmwareVersion";
            this.olvColumn2.CellPadding = null;
            this.olvColumn2.Text = "Firmware Version";
            this.olvColumn2.Width = 99;
            // 
            // mCOLProgress
            // 
            this.mCOLProgress.AspectName = "OverallProgress";
            this.mCOLProgress.CellPadding = null;
            this.mCOLProgress.Text = "Progress";
            this.mCOLProgress.Width = 212;
            // 
            // olvColumn4
            // 
            this.olvColumn4.AspectName = "State";
            this.olvColumn4.CellPadding = null;
            this.olvColumn4.Text = "State";
            this.olvColumn4.Width = 247;
            // 
            // FlashProgramMemoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 434);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(722, 272);
            this.Name = "FlashProgramMemoryForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Flash Program Memory";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mOLVFlashStates)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button mBTNAbort;
        private System.Windows.Forms.Button mBTNStart;
        private System.Windows.Forms.Button mBTNClose;
        private BrightIdeasSoftware.ObjectListView mOLVFlashStates;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private BrightIdeasSoftware.OLVColumn mCOLProgress;
        private BrightIdeasSoftware.OLVColumn olvColumn4;
        private System.Windows.Forms.Label mLBLFWSize;
        private System.Windows.Forms.Label mLBLFWVersion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label mLBLFWName;
        private System.Windows.Forms.Label label3;
    }
}