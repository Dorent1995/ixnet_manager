using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iXnetManager.Controls;
using iXnetManager.Theme;

namespace iXnetManager
{
    public partial class SecurityTokenForm : BaseChromeForm
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
            InstallChrome(resizable: false, showMinimize: false, showMaximize: false, showThemeToggle: false);
            ThemeApplier.Apply(this, mBTNOk);
        }

        private void mBTNOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
