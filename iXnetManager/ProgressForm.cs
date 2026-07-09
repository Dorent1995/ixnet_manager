using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace iXnetManager
{
    public partial class ProgressForm : Form
    {
        public ProgressForm()
        {
            InitializeComponent();

            mLBLDescription.Text = String.Empty;
            mProgressBar.Value = 0;
        }

        public void SetProgress(int percent, string description)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => this.SetProgress(percent, description)));
                return;
            }

            mProgressBar.Value = percent;
            if (description != null)
                mLBLDescription.Text = description;
        }

        public void SetProgress(int percent)
        {
            SetProgress(percent, null);
        }
    }
}
