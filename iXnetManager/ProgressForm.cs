using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using iXnetManager.Controls;
using iXnetManager.Theme;

namespace iXnetManager
{
    public partial class ProgressForm : Form
    {
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        private const int PBM_SETBARCOLOR = 0x0409;
        private const int PBM_SETBKCOLOR = 0x2001;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, int lParam);

        public ProgressForm()
        {
            InitializeComponent();

            ThemeApplier.Apply(this);
            RoundedControls.ApplyRoundedRegion(this, 14);
            StyleProgressBar();

            mLBLDescription.Text = String.Empty;
            mProgressBar.Value = 0;
        }

        private void StyleProgressBar()
        {
            mProgressBar.Style = ProgressBarStyle.Continuous;

            EventHandler apply = null;
            apply = delegate
            {
                if (!mProgressBar.IsHandleCreated)
                    return;

                ThemePalette p = ThemeManager.Current;
                SetWindowTheme(mProgressBar.Handle, "", "");
                SendMessage(mProgressBar.Handle, PBM_SETBARCOLOR, IntPtr.Zero, ColorToInt(p.Accent));
                SendMessage(mProgressBar.Handle, PBM_SETBKCOLOR, IntPtr.Zero, ColorToInt(p.SliderTrackEmpty));
            };

            mProgressBar.HandleCreated += apply;
            ThemeManager.ThemeChanged += apply;
            FormClosed += delegate { ThemeManager.ThemeChanged -= apply; };

            if (mProgressBar.IsHandleCreated)
                apply(null, EventArgs.Empty);
        }

        private static int ColorToInt(Color c)
        {
            return c.R | (c.G << 8) | (c.B << 16);
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
