using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iXnetManager.Controls;
using iXnetManager.Theme;

namespace iXnetManager
{
    public partial class LedTestForm : BaseChromeForm
    {
        public LedTestForm()
        {
            InitializeComponent();
            InstallChrome(resizable: true, showMinimize: true, showMaximize: true, showThemeToggle: false);
            ThemeApplier.Apply(this, button1);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
