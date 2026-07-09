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
    public partial class SecurityTokenForm : Form
    {
        public string Password
        {
            get
            {
                return mTBPassword.Text;
            }
        }

        public SecurityTokenForm()
        {
            InitializeComponent();
        }

        private void mBTNOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
