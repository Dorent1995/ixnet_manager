using System;
using System.Drawing;
using System.Windows.Forms;
using iXnetManager.Theme;

namespace iXnetManager.Controls
{
    /// <summary>
    /// Base class for every window in the application. Removes the native
    /// Windows title bar (FormBorderStyle = None) and replaces it with a
    /// <see cref="MacTitleBar"/> (traffic-light buttons, centered title,
    /// Light/Dark toggle), while still supporting dragging, resizing and
    /// double-click-to-maximize through a WM_NCHITTEST override - exactly
    /// like native custom-chrome apps (Chrome, VS Code, Windows Terminal).
    /// All existing forms/controls/event handlers are unaffected; only the
    /// window frame and coloring change.
    /// </summary>
    public class BaseChromeForm : Form
    {
        private const int ResizeBorderThickness = 6;

        private const int WM_NCHITTEST = 0x0084;
        private const int HTCLIENT = 1;
        private const int HTCAPTION = 2;
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;

        private MacTitleBar _titleBar;
        private bool _resizable = true;

        public BaseChromeForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            BackColor = ThemeManager.Current.CardBorder;
            Padding = new Padding(1);
            DoubleBuffered = true;

            ThemeManager.ThemeChanged += ThemeManager_ThemeChanged;
            FormClosed += (s, e) => ThemeManager.ThemeChanged -= ThemeManager_ThemeChanged;
        }

        private void ThemeManager_ThemeChanged(object sender, EventArgs e)
        {
            BackColor = ThemeManager.Current.CardBorder;
            Invalidate(true);
        }

        /// <summary>
        /// Call once from the derived form's constructor, right after
        /// InitializeComponent(), to attach the custom title bar and
        /// configure resize/minimize/maximize behavior. Also grows the
        /// form's client area by the title bar height so all originally
        /// designed content keeps its exact size.
        /// </summary>
        protected void InstallChrome(bool resizable, bool showMinimize, bool showMaximize, bool showThemeToggle = true)
        {
            _resizable = resizable;

            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = showMaximize;
            MinimizeBox = showMinimize;

            Size originalClientSize = ClientSize;
            ClientSize = new Size(originalClientSize.Width, originalClientSize.Height + MacTitleBar.PreferredHeight);

            _titleBar = new MacTitleBar(this, showMinimize, showMaximize, showThemeToggle);
            Controls.Add(_titleBar);
            _titleBar.BringToFront();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCHITTEST && _titleBar != null)
            {
                base.WndProc(ref m);

                if ((int)m.Result == HTCLIENT)
                {
                    Point screenPoint = new Point(m.LParam.ToInt32());
                    Point clientPoint = PointToClient(screenPoint);

                    if (_resizable && WindowState == FormWindowState.Normal)
                    {
                        int ht = HitTestResizeBorder(clientPoint);
                        if (ht != HTCLIENT)
                        {
                            m.Result = (IntPtr)ht;
                            return;
                        }
                    }

                    if (clientPoint.Y >= 0 && clientPoint.Y < _titleBar.Height &&
                        !_titleBar.HitTestButton(clientPoint))
                    {
                        m.Result = (IntPtr)HTCAPTION;
                        return;
                    }
                }

                return;
            }

            base.WndProc(ref m);
        }

        private int HitTestResizeBorder(Point p)
        {
            bool left = p.X <= ResizeBorderThickness;
            bool right = p.X >= ClientSize.Width - ResizeBorderThickness;
            bool top = p.Y <= ResizeBorderThickness;
            bool bottom = p.Y >= ClientSize.Height - ResizeBorderThickness;

            if (top && left) return HTTOPLEFT;
            if (top && right) return HTTOPRIGHT;
            if (bottom && left) return HTBOTTOMLEFT;
            if (bottom && right) return HTBOTTOMRIGHT;
            if (top) return HTTOP;
            if (bottom) return HTBOTTOM;
            if (left) return HTLEFT;
            if (right) return HTRIGHT;

            return HTCLIENT;
        }
    }
}
