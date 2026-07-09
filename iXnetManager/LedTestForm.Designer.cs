namespace iXnetManager
{
    partial class LedTestForm
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.LedID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TouchCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mContentPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.LedID,
            this.Status,
            this.TouchCount});
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(17, 165);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(540, 442);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // LedID
            // 
            this.LedID.Text = "Led ID";
            this.LedID.Width = 74;
            // 
            // Status
            // 
            this.Status.Text = "Status";
            this.Status.Width = 99;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(548, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "Click \"Start\" to test the touchsensors and buttons of the Pick2Light modules.\r\nAs" +
    " soon as the module is touched, it starts to blink.\r\n";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(17, 624);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(155, 35);
            this.button1.TabIndex = 2;
            this.button1.Text = "Start Touchsensor Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(13, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "WARNING:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(561, 40);
            this.label3.TabIndex = 4;
            this.label3.Text = "This immediately interrupts the input transmission to the ix.light-Server!\r\nInput" +
    "s may need to be reaquired afterwards in the ix.light-Server Webinterface!";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // TouchCount
            // 
            this.TouchCount.Text = "Touch Count";
            this.TouchCount.Width = 88;
            // 
            // LedTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 717);
            this.mContentPanel.Controls.Add(this.label3);
            this.mContentPanel.Controls.Add(this.label2);
            this.mContentPanel.Controls.Add(this.button1);
            this.mContentPanel.Controls.Add(this.label1);
            this.mContentPanel.Controls.Add(this.listView1);
            this.mContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mContentPanel.Location = new System.Drawing.Point(0, 0);
            this.mContentPanel.Name = "mContentPanel";
            this.mContentPanel.Size = new System.Drawing.Size(591, 681);
            this.mContentPanel.TabIndex = 0;
            this.Controls.Add(this.mContentPanel);
            this.Name = "LedTestForm";
            this.Text = "Touchsensor Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader LedID;
        private System.Windows.Forms.ColumnHeader Status;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ColumnHeader TouchCount;
        private System.Windows.Forms.Panel mContentPanel;
    }
}