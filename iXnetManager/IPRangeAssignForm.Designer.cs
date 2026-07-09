namespace iXnetManager
{
    partial class IPRangeAssignForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.mTBNetwork = new System.Windows.Forms.TextBox();
            this.mTBStartIPMask = new System.Windows.Forms.TextBox();
            this.mTBStartIP = new System.Windows.Forms.TextBox();
            this.mTBEndIP = new System.Windows.Forms.TextBox();
            this.mTBEndIPMask = new System.Windows.Forms.TextBox();
            this.mBTNAssign = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.mTBGateway = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Network:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "End IP:";
            // 
            // mTBNetwork
            // 
            this.mTBNetwork.Location = new System.Drawing.Point(68, 6);
            this.mTBNetwork.Name = "mTBNetwork";
            this.mTBNetwork.Size = new System.Drawing.Size(108, 20);
            this.mTBNetwork.TabIndex = 3;
            // 
            // mTBStartIPMask
            // 
            this.mTBStartIPMask.Enabled = false;
            this.mTBStartIPMask.Location = new System.Drawing.Point(68, 32);
            this.mTBStartIPMask.Name = "mTBStartIPMask";
            this.mTBStartIPMask.Size = new System.Drawing.Size(71, 20);
            this.mTBStartIPMask.TabIndex = 4;
            this.mTBStartIPMask.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // mTBStartIP
            // 
            this.mTBStartIP.Location = new System.Drawing.Point(145, 32);
            this.mTBStartIP.Name = "mTBStartIP";
            this.mTBStartIP.Size = new System.Drawing.Size(31, 20);
            this.mTBStartIP.TabIndex = 5;
            // 
            // mTBEndIP
            // 
            this.mTBEndIP.Enabled = false;
            this.mTBEndIP.Location = new System.Drawing.Point(145, 58);
            this.mTBEndIP.Name = "mTBEndIP";
            this.mTBEndIP.Size = new System.Drawing.Size(31, 20);
            this.mTBEndIP.TabIndex = 7;
            // 
            // mTBEndIPMask
            // 
            this.mTBEndIPMask.Enabled = false;
            this.mTBEndIPMask.Location = new System.Drawing.Point(68, 58);
            this.mTBEndIPMask.Name = "mTBEndIPMask";
            this.mTBEndIPMask.Size = new System.Drawing.Size(71, 20);
            this.mTBEndIPMask.TabIndex = 6;
            this.mTBEndIPMask.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // mBTNAssign
            // 
            this.mBTNAssign.Location = new System.Drawing.Point(101, 110);
            this.mBTNAssign.Name = "mBTNAssign";
            this.mBTNAssign.Size = new System.Drawing.Size(75, 23);
            this.mBTNAssign.TabIndex = 8;
            this.mBTNAssign.Text = "Assign";
            this.mBTNAssign.UseVisualStyleBackColor = true;
            this.mBTNAssign.Click += new System.EventHandler(this.mBTNAssign_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Gateway:";
            // 
            // mTBGateway
            // 
            this.mTBGateway.Location = new System.Drawing.Point(68, 84);
            this.mTBGateway.Name = "mTBGateway";
            this.mTBGateway.Size = new System.Drawing.Size(108, 20);
            this.mTBGateway.TabIndex = 10;
            // 
            // IPRangeAssignForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(196, 145);
            this.Controls.Add(this.mTBGateway);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.mBTNAssign);
            this.Controls.Add(this.mTBEndIP);
            this.Controls.Add(this.mTBEndIPMask);
            this.Controls.Add(this.mTBStartIP);
            this.Controls.Add(this.mTBStartIPMask);
            this.Controls.Add(this.mTBNetwork);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "IPRangeAssignForm";
            this.ShowInTaskbar = false;
            this.Text = "Assign IP Range";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox mTBNetwork;
        private System.Windows.Forms.TextBox mTBStartIPMask;
        private System.Windows.Forms.TextBox mTBStartIP;
        private System.Windows.Forms.TextBox mTBEndIP;
        private System.Windows.Forms.TextBox mTBEndIPMask;
        private System.Windows.Forms.Button mBTNAssign;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox mTBGateway;
    }
}