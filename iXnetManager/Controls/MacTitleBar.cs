using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using iXnetManager.Theme;

namespace iXnetManager.Controls
{
    /// <summary>
    /// Custom macOS-style title bar: red/yellow/green traffic light buttons
    /// on the left, centered window title, and a small Light/Dark theme
    /// toggle on the right. Meant to be docked to the top of a
    /// <see cref="BaseChromeForm"/> whose native FormBorderStyle is None.
    /// </summary>
    public class MacTitleBar : Control
    {
        public const int PreferredHeight = 36;

        private const int DotDiameter = 12;
        private const int DotSpacing = 8;
        private const int LeftMargin = 14;

        private readonly Form _owner;
        private readonly bool _showMinimize;
        private readonly bool _showMaximize;
        private readonly bool _showThemeToggle;

        private bool _trafficHover;
        private int _hoverDotIndex = -1; // 0=close,1=min,2=max
        private bool _themeHover;
        private readonly EventHandler _themeChangedHandler;

        public MacTitleBar(Form owner, bool showMinimize, bool showMaximize, bool showThemeToggle)
        {
            _owner = owner;
            _showMinimize = showMinimize;
            _showMaximize = showMaximize;
            _showThemeToggle = showThemeToggle;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                      ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            Height = PreferredHeight;
            Dock = DockStyle.Top;

            _themeChangedHandler = delegate { Invalidate(); };
            ThemeManager.ThemeChanged += _themeChangedHandler;
            _owner.TextChanged += (s, e) => Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                ThemeManager.ThemeChanged -= _themeChangedHandler;

            base.Dispose(disposing);
        }

        private Rectangle DotRect(int index)
        {
            int x = LeftMargin + index * (DotDiameter + DotSpacing);
            int y = (Height - DotDiameter) / 2;
            return new Rectangle(x, y, DotDiameter, DotDiameter);
        }

        private Rectangle TrafficClusterRect()
        {
            Rectangle first = DotRect(0);
            Rectangle last = DotRect(2);
            return Rectangle.FromLTRB(first.Left - 6, 0, last.Right + 6, Height);
        }

        private Rectangle ThemeToggleRect()
        {
            int size = 20;
            int x = Width - size - 12;
            int y = (Height - size) / 2;
            return new Rectangle(x, y, size, size);
        }

        /// <summary>True when the given (control-relative) point is over an
        /// interactive element, so the owning form must NOT treat it as a
        /// draggable caption area.</summary>
        public bool HitTestButton(Point clientPoint)
        {
            if (TrafficClusterRect().Contains(clientPoint))
                return true;

            if (_showThemeToggle && ThemeToggleRect().Contains(clientPoint))
                return true;

            return false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            bool wasTrafficHover = _trafficHover;
            int prevDot = _hoverDotIndex;
            bool wasThemeHover = _themeHover;

            _trafficHover = TrafficClusterRect().Contains(e.Location);
            _hoverDotIndex = -1;
            for (int i = 0; i < 3; i++)
            {
                if (DotRect(i).Contains(e.Location))
                    _hoverDotIndex = i;
            }

            _themeHover = _showThemeToggle && ThemeToggleRect().Contains(e.Location);

            if (wasTrafficHover != _trafficHover || prevDot != _hoverDotIndex || wasThemeHover != _themeHover)
                Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _trafficHover = false;
            _hoverDotIndex = -1;
            _themeHover = false;
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button != MouseButtons.Left)
                return;

            if (DotRect(0).Contains(e.Location))
            {
                _owner.Close();
                return;
            }

            if (_showMinimize && DotRect(1).Contains(e.Location))
            {
                _owner.WindowState = FormWindowState.Minimized;
                return;
            }

            if (_showMaximize && DotRect(2).Contains(e.Location))
            {
                _owner.WindowState = _owner.WindowState == FormWindowState.Maximized
                    ? FormWindowState.Normal
                    : FormWindowState.Maximized;
                return;
            }

            if (_showThemeToggle && ThemeToggleRect().Contains(e.Location))
            {
                ThemeManager.Toggle();
            }
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

            PaintDot(g, 0, palette.TrafficRed, true, "\u00D7");
            PaintDot(g, 1, palette.TrafficYellow, _showMinimize, "\u2212");
            PaintDot(g, 2, palette.TrafficGreen, _showMaximize, "+");

            if (_showThemeToggle)
                PaintThemeToggle(g, palette);

            PaintTitle(g, palette);
        }

        private void PaintDot(Graphics g, int index, Color color, bool enabled, string glyph)
        {
            Rectangle rect = DotRect(index);
            Color drawColor = enabled ? color : ControlPaint.Light(ThemeManager.Current.TitleBarBackground, 0.02f);
            Color borderColor = enabled ? Color.FromArgb(40, 0, 0, 0) : ThemeManager.Current.Divider;

            using (SolidBrush brush = new SolidBrush(drawColor))
                g.FillEllipse(brush, rect);

            using (Pen pen = new Pen(borderColor))
                g.DrawEllipse(pen, rect);

            if (enabled && (_trafficHover))
            {
                using (Font glyphFont = new Font(AppFonts.FamilyName, 7f, FontStyle.Bold))
                using (SolidBrush glyphBrush = new SolidBrush(ThemeManager.Current.TrafficGlyph))
                {
                    SizeF textSize = g.MeasureString(glyph, glyphFont);
                    PointF pos = new PointF(
                        rect.X + (rect.Width - textSize.Width) / 2f,
                        rect.Y + (rect.Height - textSize.Height) / 2f - 1f);
                    g.DrawString(glyph, glyphFont, glyphBrush, pos);
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

        private void PaintTitle(Graphics g, ThemePalette palette)
        {
            string title = _owner.Text;
            if (string.IsNullOrEmpty(title))
                return;

            using (Font titleFont = new Font(AppFonts.FamilyName, 9.5f, FontStyle.Bold))
            {
                SizeF textSize = g.MeasureString(title, titleFont);

                int leftReserved = TrafficClusterRect().Right + 8;
                int rightReserved = _showThemeToggle ? Width - ThemeToggleRect().Left + 8 : 8;

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
