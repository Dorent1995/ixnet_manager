namespace iXnetManager
{
    partial class AddDeviceForm
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
            this.mTBIPAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mBtnSearch = new System.Windows.Forms.Button();
            this.mTBLocalAdapter = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mTBIPAddress
            // 
            this.mTBIPAddress.Location = new System.Drawing.Point(94, 38);
            this.mTBIPAddress.Name = "mTBIPAddress";
            this.mTBIPAddress.Size = new System.Drawing.Size(132, 20);
            this.mTBIPAddress.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "ix.net Address:";
            // 
            // mBtnSearch
            // 
            this.mBtnSearch.Location = new System.Drawing.Point(151, 64);
            this.mBtnSearch.Name = "mBtnSearch";
            this.mBtnSearch.Size = new System.Drawing.Size(75, 23);
            this.mBtnSearch.TabIndex = 2;
            this.mBtnSearch.Text = "Search";
            this.mBtnSearch.UseVisualStyleBackColor = true;
            this.mBtnSearch.Click += new System.EventHandler(this.mBtnSearch_Click);
            // 
            // mTBLocalAdapter
            // 
            this.mTBLocalAdapter.Location = new System.Drawing.Point(94, 12);
            this.mTBLocalAdapter.Name = "mTBLocalAdapter";
            this.mTBLocalAdapter.Size = new System.Drawing.Size(132, 20);
            this.mTBLocalAdapter.TabIndex = 3;
            this.mTBLocalAdapter.Text = "0.0.0.0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Local Adapter:";
            // 
            // AddDeviceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 95);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.mTBLocalAdapter);
            this.Controls.Add(this.mBtnSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mTBIPAddress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AddDeviceForm";
            this.Text = "Add Device";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox mTBIPAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button mBtnSearch;
        private System.Windows.Forms.TextBox mTBLocalAdapter;
        private System.Windows.Forms.Label label2;
    }
}