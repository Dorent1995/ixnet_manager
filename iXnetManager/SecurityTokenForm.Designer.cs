namespace iXnetManager
{
    partial class SecurityTokenForm
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
            this.mTBPassword = new System.Windows.Forms.TextBox();
            this.mBTNOk = new System.Windows.Forms.Button();
            this.mBTNCancel = new System.Windows.Forms.Button();
            this.mContentPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Password:";
            // 
            // mTBPassword
            // 
            this.mTBPassword.Location = new System.Drawing.Point(68, 13);
            this.mTBPassword.Name = "mTBPassword";
            this.mTBPassword.Size = new System.Drawing.Size(100, 20);
            this.mTBPassword.TabIndex = 1;
            this.mTBPassword.UseSystemPasswordChar = true;
            // 
            // mBTNOk
            // 
            this.mBTNOk.Location = new System.Drawing.Point(12, 47);
            this.mBTNOk.Name = "mBTNOk";
            this.mBTNOk.Size = new System.Drawing.Size(75, 23);
            this.mBTNOk.TabIndex = 2;
            this.mBTNOk.Text = "Ok";
            this.mBTNOk.UseVisualStyleBackColor = true;
            this.mBTNOk.Click += new System.EventHandler(this.mBTNOk_Click);
            // 
            // mBTNCancel
            // 
            this.mBTNCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.mBTNCancel.Location = new System.Drawing.Point(93, 47);
            this.mBTNCancel.Name = "mBTNCancel";
            this.mBTNCancel.Size = new System.Drawing.Size(75, 23);
            this.mBTNCancel.TabIndex = 3;
            this.mBTNCancel.Text = "Cancel";
            this.mBTNCancel.UseVisualStyleBackColor = true;
            // 
            // SecurityTokenForm
            // 
            this.AcceptButton = this.mBTNOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.mBTNCancel;
            this.ClientSize = new System.Drawing.Size(177, 118);
            this.mContentPanel.Controls.Add(this.mBTNCancel);
            this.mContentPanel.Controls.Add(this.mBTNOk);
            this.mContentPanel.Controls.Add(this.mTBPassword);
            this.mContentPanel.Controls.Add(this.label1);
            this.mContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mContentPanel.Location = new System.Drawing.Point(0, 0);
            this.mContentPanel.Name = "mContentPanel";
            this.mContentPanel.Size = new System.Drawing.Size(177, 82);
            this.mContentPanel.TabIndex = 0;
            this.Controls.Add(this.mContentPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SecurityTokenForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Security Check";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox mTBPassword;
        private System.Windows.Forms.Button mBTNOk;
        private System.Windows.Forms.Button mBTNCancel;
        private System.Windows.Forms.Panel mContentPanel;
    }
}