using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using iXnetManager.Theme;

namespace iXnetManager.Controls
{
    internal enum CaptionButtonKind
    {
        Close,
        Minimize,
        Maximize
    }

    /// <summary>
    /// Custom title bar: centered window title, minimalist monochrome
    /// window-control buttons (close/minimize/maximize - no colored
    /// "traffic light" circles) on the left, and a small Light/Dark theme
    /// toggle on the right. Meant to be docked to the top of a
    /// <see cref="BaseChromeForm"/> whose native FormBorderStyle is None.
    /// </summary>
    public class MacTitleBar : Control
    {
        public const int PreferredHeight = 36;

        private const int ButtonWidth = 34;

        private readonly Form _owner;
        private readonly bool _showMinimize;
        private readonly bool _showMaximize;
        private readonly bool _showThemeToggle;
        private readonly List<CaptionButtonKind> _buttons;

        private int _hoverButtonIndex = -1;
        private bool _themeHover;
        private bool _sidebarHover;
        private Action _onSidebarToggle;
        private Func<bool> _isSidebarExpanded;
        private readonly EventHandler _themeChangedHandler;

        public MacTitleBar(Form owner, bool showMinimize, bool showMaximize, bool showThemeToggle)
        {
            _owner = owner;
            _showMinimize = showMinimize;
            _showMaximize = showMaximize;
            _showThemeToggle = showThemeToggle;

            _buttons = new List<CaptionButtonKind>();
            if (_showMinimize)
                _buttons.Add(CaptionButtonKind.Minimize);
            if (_showMaximize)
                _buttons.Add(CaptionButtonKind.Maximize);
            _buttons.Add(CaptionButtonKind.Close);

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                      ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            Height = PreferredHeight;
            Dock = DockStyle.Top;

            _themeChangedHandler = delegate { Invalidate(); };
            ThemeManager.ThemeChanged += _themeChangedHandler;
            _owner.TextChanged += (s, e) => Invalidate();

            if (_showMaximize)
                _owner.Resize += (s, e) => Invalidate(); // maximize glyph flips to "restore"
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                ThemeManager.ThemeChanged -= _themeChangedHandler;

            base.Dispose(disposing);
        }

        /// <summary>
        /// Adds a small "toggle side panel" icon button, positioned just
        /// left of the close/minimize/maximize cluster. Only forms that
        /// have such a panel (currently just MainForm's Properties grid)
        /// call this; other forms simply never show it.
        /// </summary>
        public void EnableSidebarToggle(Action onToggle, Func<bool> isExpanded)
        {
            _onSidebarToggle = onToggle;
            _isSidebarExpanded = isExpanded;
            Invalidate();
        }

        // Window-control buttons live on the RIGHT (standard Windows
        // convention - minimize, maximize, close from left to right within
        // the cluster, close outermost/closest to the screen edge), so the
        // cluster is anchored from the right edge of the title bar.
        private Rectangle ButtonRect(int visibleIndex)
        {
            int x = Width - (_buttons.Count - visibleIndex) * ButtonWidth;
            return new Rectangle(x, 0, ButtonWidth, Height);
        }

        private Rectangle ButtonClusterRect()
        {
            int width = _buttons.Count * ButtonWidth;
            return new Rectangle(Width - width, 0, width, Height);
        }

        private Rectangle ThemeToggleRect()
        {
            int size = 20;
            int x = 12;
            int y = (Height - size) / 2;
            return new Rectangle(x, y, size, size);
        }

        private Rectangle SidebarToggleRect()
        {
            int width = 28;
            int height = 20;
            int x = ButtonClusterRect().Left - 8 - width;
            int y = (Height - height) / 2;
            return new Rectangle(x, y, width, height);
        }

        private bool ShowSidebarToggle
        {
            get { return _onSidebarToggle != null; }
        }

        /// <summary>True when the given (control-relative) point is over an
        /// interactive element, so the owning form must NOT treat it as a
        /// draggable caption area.</summary>
        public bool HitTestButton(Point clientPoint)
        {
            if (ButtonClusterRect().Contains(clientPoint))
                return true;

            if (_showThemeToggle && ThemeToggleRect().Contains(clientPoint))
                return true;

            if (ShowSidebarToggle && SidebarToggleRect().Contains(clientPoint))
                return true;

            return false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            int prevButton = _hoverButtonIndex;
            bool wasThemeHover = _themeHover;

            _hoverButtonIndex = -1;
            for (int i = 0; i < _buttons.Count; i++)
            {
                if (ButtonRect(i).Contains(e.Location))
                    _hoverButtonIndex = i;
            }

            _themeHover = _showThemeToggle && ThemeToggleRect().Contains(e.Location);

            bool wasSidebarHover = _sidebarHover;
            _sidebarHover = ShowSidebarToggle && SidebarToggleRect().Contains(e.Location);

            if (prevButton != _hoverButtonIndex || wasThemeHover != _themeHover || wasSidebarHover != _sidebarHover)
                Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hoverButtonIndex = -1;
            _themeHover = false;
            _sidebarHover = false;
            Invalidate();
        }

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HTCAPTION = 0x2;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button != MouseButtons.Left)
                return;

            // Buttons/theme toggle handle their own click in OnMouseClick -
            // don't start a window drag on top of them.
            if (HitTestButton(e.Location))
                return;

            if (e.Clicks >= 2 && _showMaximize)
            {
                _owner.WindowState = _owner.WindowState == FormWindowState.Maximized
                    ? FormWindowState.Normal
                    : FormWindowState.Maximized;
                return;
            }

            // Dragging a child control's own window handle would only move
            // the title bar strip within its parent, not the actual Form -
            // so instead we tell Windows the mouse-down happened on the
            // PARENT FORM's caption, which makes the OS run its normal
            // native window-drag loop (also gets Aero Snap for free). This
            // is the standard, reliable technique for custom title bars.
            // PostMessage (not SendMessage) so we don't block this control's
            // message pump while the OS runs its native move loop - a
            // synchronous SendMessage here re-entered our own WndProc mid
            // mouse-down and produced a stale/duplicate "ghost" repaint of
            // the title bar and list beneath it on some machines.
            ReleaseCapture();
            PostMessage(_owner.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button != MouseButtons.Left)
                return;

            for (int i = 0; i < _buttons.Count; i++)
            {
                if (!ButtonRect(i).Contains(e.Location))
                    continue;

                switch (_buttons[i])
                {
                    case CaptionButtonKind.Close:
                        _owner.Close();
                        break;
                    case CaptionButtonKind.Minimize:
                        _owner.WindowState = FormWindowState.Minimized;
                        break;
                    case CaptionButtonKind.Maximize:
                        _owner.WindowState = _owner.WindowState == FormWindowState.Maximized
                            ? FormWindowState.Normal
                            : FormWindowState.Maximized;
                        break;
                }
                return;
            }

            if (_showThemeToggle && ThemeToggleRect().Contains(e.Location))
            {
                ThemeManager.Toggle();
                return;
            }

            if (ShowSidebarToggle && SidebarToggleRect().Contains(e.Location))
                _onSidebarToggle();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            ThemePalette palette = ThemeManager.Current;

            using (SolidBrush bg = new SolidBrush(palette.TitleBarBackground))
                g.FillRectangle(bg, ClientRectangle);

            using (Pen bottomLine = new Pen(palette.Divider))
                g.DrawLine(bottomLine, 0, Height - 1, Width, Height - 1);

            for (int i = 0; i < _buttons.Count; i++)
                PaintCaptionButton(g, i, _buttons[i], palette);

            if (_showThemeToggle)
                PaintThemeToggle(g, palette);

            if (ShowSidebarToggle)
                PaintSidebarToggle(g, palette);

            PaintTitle(g, palette);
        }

        private void PaintCaptionButton(Graphics g, int index, CaptionButtonKind kind, ThemePalette palette)
        {
            Rectangle rect = ButtonRect(index);
            bool hovered = _hoverButtonIndex == index;
            bool isClose = kind == CaptionButtonKind.Close;

            if (hovered)
            {
                Color hoverColor = isClose ? Color.FromArgb(232, 17, 35) : Color.FromArgb(24, palette.TextPrimary);
                using (SolidBrush hoverBrush = new SolidBrush(hoverColor))
                    g.FillRectangle(hoverBrush, rect);
            }

            Color glyphColor = hovered && isClose ? Color.White : palette.TextSecondary;
            if (hovered && !isClose)
                glyphColor = palette.TextPrimary;

            using (Pen pen = new Pen(glyphColor, 1.2f))
            {
                pen.StartCap = LineCap.Flat;
                pen.EndCap = LineCap.Flat;

                int cx = rect.X + rect.Width / 2;
                int cy = rect.Y + rect.Height / 2;
                int half = 5;

                switch (kind)
                {
                    case CaptionButtonKind.Close:
                        g.DrawLine(pen, cx - half, cy - half, cx + half, cy + half);
                        g.DrawLine(pen, cx - half, cy + half, cx + half, cy - half);
                        break;

                    case CaptionButtonKind.Minimize:
                        g.DrawLine(pen, cx - half, cy, cx + half, cy);
                        break;

                    case CaptionButtonKind.Maximize:
                        if (_owner.WindowState == FormWindowState.Maximized)
                        {
                            // "restore" glyph: two overlapping outlined squares
                            g.DrawRectangle(pen, cx - half + 2, cy - half, (half - 1) * 2, (half - 1) * 2);
                            g.DrawRectangle(pen, cx - half, cy - half + 2, (half - 1) * 2, (half - 1) * 2);
                        }
                        else
                        {
                            g.DrawRectangle(pen, cx - half, cy - half, half * 2, half * 2);
                        }
                        break;
                }
            }
        }

        private void PaintThemeToggle(Graphics g, ThemePalette palette)
        {
            Rectangle rect = ThemeToggleRect();

            if (_themeHover)
            {
                using (SolidBrush hoverBrush = new SolidBrush(Color.FromArgb(28, palette.TextPrimary)))
                using (GraphicsPath path = RoundedControls.RoundedRectPath(rect, rect.Width / 2))
                    g.FillPath(hoverBrush, path);
            }

            string glyph = ThemeManager.CurrentMode == ThemeMode.Light ? "\u263D" : "\u2600";

            using (Font glyphFont = new Font(AppFonts.FamilyName, 11f, FontStyle.Regular))
            using (SolidBrush textBrush = new SolidBrush(palette.TextSecondary))
            {
                SizeF textSize = g.MeasureString(glyph, glyphFont);
                PointF pos = new PointF(
                    rect.X + (rect.Width - textSize.Width) / 2f,
                    rect.Y + (rect.Height - textSize.Height) / 2f);
                g.DrawString(glyph, glyphFont, textBrush, pos);
            }
        }

        private void PaintSidebarToggle(Graphics g, ThemePalette palette)
        {
            Rectangle rect = SidebarToggleRect();

            if (_sidebarHover)
            {
                using (SolidBrush hoverBrush = new SolidBrush(Color.FromArgb(28, palette.TextPrimary)))
                using (GraphicsPath path = RoundedControls.RoundedRectPath(rect, 5))
                    g.FillPath(hoverBrush, path);
            }

            // Classic "toggle side panel" glyph: an outlined rectangle with
            // a vertical divider near the right third, that third shaded to
            // read as the collapsible panel.
            int iconW = 16;
            int iconH = 12;
            int ix = rect.X + (rect.Width - iconW) / 2;
            int iy = rect.Y + (rect.Height - iconH) / 2;
            Rectangle iconRect = new Rectangle(ix, iy, iconW, iconH);
            int dividerX = iconRect.Right - 5;

            Color lineColor = _sidebarHover ? palette.TextPrimary : palette.TextSecondary;

            bool expanded = _isSidebarExpanded == null || _isSidebarExpanded();
            if (expanded)
            {
                Rectangle panelRect = new Rectangle(dividerX, iconRect.Y + 1, iconRect.Right - dividerX - 1, iconRect.Height - 2);
                using (SolidBrush panelBrush = new SolidBrush(Color.FromArgb(90, lineColor)))
                    g.FillRectangle(panelBrush, panelRect);
            }

            using (Pen pen = new Pen(lineColor, 1.2f))
            {
                g.DrawRectangle(pen, iconRect);
                g.DrawLine(pen, dividerX, iconRect.Y, dividerX, iconRect.Bottom);
            }
        }

        private void PaintTitle(Graphics g, ThemePalette palette)
        {
            string title = _owner.Text;
            if (string.IsNullOrEmpty(title))
                return;

            using (Font titleFont = new Font(AppFonts.FamilyName, 9.5f, FontStyle.Bold))
            {
                SizeF textSize = g.MeasureString(title, titleFont);

                int leftReserved = _showThemeToggle ? ThemeToggleRect().Right + 8 : 8;
                int rightBoundary = ShowSidebarToggle ? SidebarToggleRect().Left : ButtonClusterRect().Left;
                int rightReserved = Width - rightBoundary + 10;

                float x = (Width - textSize.Width) / 2f;
                if (x < leftReserved)
                    x = leftReserved;
                if (x + textSize.Width > Width - rightReserved)
                    x = Math.Max(leftReserved, Width - rightReserved - textSize.Width);

                float y = (Height - textSize.Height) / 2f;

                using (SolidBrush textBrush = new SolidBrush(palette.TitleBarText))
                    g.DrawString(title, titleFont, textBrush, x, y);
            }
        }
    }
}
