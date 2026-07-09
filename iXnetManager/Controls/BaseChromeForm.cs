using System;
using System.Collections.Generic;
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
        private TableLayoutPanel _chromeLayout;

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

            // Rather than adding the title bar as a Dock=Top sibling next
            // to whatever Dock=Fill content InitializeComponent already
            // built (which depended on the two controls' add-order/timing
            // and was still leaving the content's top rows rendered behind
            // the title bar on some forms/machines), move all pre-existing
            // top-level content into its own host panel and place both the
            // title bar and that host into a 2-row TableLayoutPanel with a
            // fixed-height row for the title bar and a 100% row for the
            // content. A TableLayoutPanel's row sizes are resolved from its
            // RowStyles up front, independent of child add-order, so this
            // can't leave the content overlapping the title bar.
            List<Control> originalControls = new List<Control>();
            foreach (Control c in Controls)
                originalControls.Add(c);
            Controls.Clear();

            Panel contentHost = new Panel();
            contentHost.Dock = DockStyle.Fill;
            contentHost.Margin = Padding.Empty;
            foreach (Control c in originalControls)
                contentHost.Controls.Add(c);

            _titleBar = new MacTitleBar(this, showMinimize, showMaximize, showThemeToggle);
            _titleBar.Dock = DockStyle.Fill;

            _chromeLayout = new TableLayoutPanel();
            _chromeLayout.Dock = DockStyle.Fill;
            _chromeLayout.Margin = Padding.Empty;
            _chromeLayout.Padding = Padding.Empty;
            _chromeLayout.ColumnCount = 1;
            _chromeLayout.RowCount = 2;
            _chromeLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            _chromeLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, MacTitleBar.PreferredHeight));
            _chromeLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            _chromeLayout.Controls.Add(_titleBar, 0, 0);
            _chromeLayout.Controls.Add(contentHost, 0, 1);

            Controls.Add(_chromeLayout);

            ClientSize = new Size(originalClientSize.Width, originalClientSize.Height + MacTitleBar.PreferredHeight);
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
