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

        private const int WS_MINIMIZEBOX = 0x00020000;
        private const int WS_MAXIMIZEBOX = 0x00010000;
        private const int WS_THICKFRAME = 0x00040000;

        private MacTitleBar _titleBar;
        private Control _content;
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

        // FormBorderStyle.None strips WS_THICKFRAME from the native window
        // style entirely. Our own WM_NCHITTEST override still lets the user
        // drag-resize the edges, but without WS_THICKFRAME Windows no longer
        // treats the window as "resizable" at the OS level, which silently
        // disables Aero Snap (drag-to-edge docking, Win+Arrow). Adding the
        // style bit back (without WS_CAPTION, so the native title bar stays
        // gone) restores Snap while keeping the custom chrome.
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (_resizable)
                    cp.Style |= WS_THICKFRAME;
                if (MinimizeBox)
                    cp.Style |= WS_MINIMIZEBOX;
                if (MaximizeBox)
                    cp.Style |= WS_MAXIMIZEBOX;
                return cp;
            }
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

            // Find the form's existing top-level content (the single
            // Dock=Fill panel/table InitializeComponent already built)
            // BEFORE adding the title bar as a sibling.
            _content = null;
            foreach (Control c in Controls)
            {
                if (c.Dock == DockStyle.Fill)
                {
                    _content = c;
                    break;
                }
            }

            Size originalClientSize = ClientSize;
            ClientSize = new Size(originalClientSize.Width, originalClientSize.Height + MacTitleBar.PreferredHeight);

            _titleBar = new MacTitleBar(this, showMinimize, showMaximize, showThemeToggle);
            // Relying on Dock=Top/Dock=Fill sibling add-order to reserve
            // the title bar's space was not reliable across forms/machines
            // - it repeatedly left the content's top rows rendered behind
            // the title bar no matter the add-order or re-dock "kick" used.
            // Take that ambiguity out of the picture entirely: both the
            // title bar and the content get Dock=None and explicit pixel
            // Bounds computed directly from ClientSize on every resize.
            _titleBar.Dock = DockStyle.None;
            Controls.Add(_titleBar);
            _titleBar.BringToFront();

            if (_content != null)
                _content.Dock = DockStyle.None;

            Resize += (s, e) => LayoutChrome();
            LayoutChrome();
        }

        private void LayoutChrome()
        {
            if (_titleBar == null)
                return;

            _titleBar.SetBounds(0, 0, ClientSize.Width, MacTitleBar.PreferredHeight);

            if (_content != null)
            {
                int contentHeight = ClientSize.Height - MacTitleBar.PreferredHeight;
                if (contentHeight < 0)
                    contentHeight = 0;
                _content.SetBounds(0, MacTitleBar.PreferredHeight, ClientSize.Width, contentHeight);
            }
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
