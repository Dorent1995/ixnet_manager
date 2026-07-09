namespace iXnetManager
{
    partial class SendDebugCharactersForm
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
            this.mTBDebugText = new System.Windows.Forms.TextBox();
            this.mBTNClose = new System.Windows.Forms.Button();
            this.mBTNSend = new System.Windows.Forms.Button();
            this.mCBAppendNewline = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Debug Text:";
            // 
            // mTBDebugText
            // 
            this.mTBDebugText.Location = new System.Drawing.Point(85, 19);
            this.mTBDebugText.Name = "mTBDebugText";
            this.mTBDebugText.Size = new System.Drawing.Size(285, 20);
            this.mTBDebugText.TabIndex = 1;
            // 
            // mBTNClose
            // 
            this.mBTNClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.mBTNClose.Location = new System.Drawing.Point(295, 49);
            this.mBTNClose.Name = "mBTNClose";
            this.mBTNClose.Size = new System.Drawing.Size(75, 23);
            this.mBTNClose.TabIndex = 2;
            this.mBTNClose.Text = "Close";
            this.mBTNClose.UseVisualStyleBackColor = true;
            this.mBTNClose.Click += new System.EventHandler(this.mBTNClose_Click);
            // 
            // mBTNSend
            // 
            this.mBTNSend.Location = new System.Drawing.Point(214, 49);
            this.mBTNSend.Name = "mBTNSend";
            this.mBTNSend.Size = new System.Drawing.Size(75, 23);
            this.mBTNSend.TabIndex = 3;
            this.mBTNSend.Text = "Send";
            this.mBTNSend.UseVisualStyleBackColor = true;
            this.mBTNSend.Click += new System.EventHandler(this.mBTNSend_Click);
            // 
            // mCBAppendNewline
            // 
            this.mCBAppendNewline.AutoSize = true;
            this.mCBAppendNewline.Checked = true;
            this.mCBAppendNewline.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mCBAppendNewline.Location = new System.Drawing.Point(16, 53);
            this.mCBAppendNewline.Name = "mCBAppendNewline";
            this.mCBAppendNewline.Size = new System.Drawing.Size(104, 17);
            this.mCBAppendNewline.TabIndex = 4;
            this.mCBAppendNewline.Text = "Append Newline";
            this.mCBAppendNewline.UseVisualStyleBackColor = true;
            // 
            // SendDebugCharactersForm
            // 
            this.AcceptButton = this.mBTNSend;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.mBTNClose;
            this.ClientSize = new System.Drawing.Size(382, 84);
            this.Controls.Add(this.mCBAppendNewline);
            this.Controls.Add(this.mBTNSend);
            this.Controls.Add(this.mBTNClose);
            this.Controls.Add(this.mTBDebugText);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SendDebugCharactersForm";
            this.Text = "SendDebugCharactersForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox mTBDebugText;
        private System.Windows.Forms.Button mBTNClose;
        private System.Windows.Forms.Button mBTNSend;
        private System.Windows.Forms.CheckBox mCBAppendNewline;
    }
}