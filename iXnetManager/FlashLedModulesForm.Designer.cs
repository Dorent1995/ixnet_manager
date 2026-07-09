namespace iXnetManager
{
    partial class FlashLedModulesForm
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
            this.mRBFull = new System.Windows.Forms.RadioButton();
            this.mRBIncremental = new System.Windows.Forms.RadioButton();
            this.mBTNStartUpdate = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.mLBLLedCount = new System.Windows.Forms.Label();
            this.mBTNClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Update Mode:";
            // 
            // mRBFull
            // 
            this.mRBFull.AutoSize = true;
            this.mRBFull.Location = new System.Drawing.Point(95, 16);
            this.mRBFull.Name = "mRBFull";
            this.mRBFull.Size = new System.Drawing.Size(41, 17);
            this.mRBFull.TabIndex = 1;
            this.mRBFull.TabStop = true;
            this.mRBFull.Text = "Full";
            this.mRBFull.UseVisualStyleBackColor = true;
            // 
            // mRBIncremental
            // 
            this.mRBIncremental.AutoSize = true;
            this.mRBIncremental.Location = new System.Drawing.Point(95, 39);
            this.mRBIncremental.Name = "mRBIncremental";
            this.mRBIncremental.Size = new System.Drawing.Size(80, 17);
            this.mRBIncremental.TabIndex = 2;
            this.mRBIncremental.TabStop = true;
            this.mRBIncremental.Text = "Incremental";
            this.mRBIncremental.UseVisualStyleBackColor = true;
            // 
            // mBTNStartUpdate
            // 
            this.mBTNStartUpdate.Location = new System.Drawing.Point(195, 13);
            this.mBTNStartUpdate.Name = "mBTNStartUpdate";
            this.mBTNStartUpdate.Size = new System.Drawing.Size(75, 23);
            this.mBTNStartUpdate.TabIndex = 3;
            this.mBTNStartUpdate.Text = "Start Update";
            this.mBTNStartUpdate.UseVisualStyleBackColor = true;
            this.mBTNStartUpdate.Click += new System.EventHandler(this.mBTNStartUpdate_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Led Modules:";
            // 
            // mLBLLedCount
            // 
            this.mLBLLedCount.AutoSize = true;
            this.mLBLLedCount.Location = new System.Drawing.Point(92, 66);
            this.mLBLLedCount.Name = "mLBLLedCount";
            this.mLBLLedCount.Size = new System.Drawing.Size(47, 13);
            this.mLBLLedCount.TabIndex = 5;
            this.mLBLLedCount.Text = "<Count>";
            // 
            // mBTNClose
            // 
            this.mBTNClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.mBTNClose.Location = new System.Drawing.Point(195, 60);
            this.mBTNClose.Name = "mBTNClose";
            this.mBTNClose.Size = new System.Drawing.Size(75, 23);
            this.mBTNClose.TabIndex = 6;
            this.mBTNClose.Text = "Close";
            this.mBTNClose.UseVisualStyleBackColor = true;
            // 
            // FlashLedModulesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.mBTNClose;
            this.ClientSize = new System.Drawing.Size(282, 93);
            this.Controls.Add(this.mBTNClose);
            this.Controls.Add(this.mLBLLedCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.mBTNStartUpdate);
            this.Controls.Add(this.mRBIncremental);
            this.Controls.Add(this.mRBFull);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FlashLedModulesForm";
            this.ShowInTaskbar = false;
            this.Text = "Update Led Modules";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton mRBFull;
        private System.Windows.Forms.RadioButton mRBIncremental;
        private System.Windows.Forms.Button mBTNStartUpdate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label mLBLLedCount;
        private System.Windows.Forms.Button mBTNClose;
    }
}