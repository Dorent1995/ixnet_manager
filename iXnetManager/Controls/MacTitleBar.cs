using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        private readonly EventHandler _themeChangedHandler;

        public MacTitleBar(Form owner, bool showMinimize, bool showMaximize, bool showThemeToggle)
        {
            _owner = owner;
            _showMinimize = showMinimize;
            _showMaximize = showMaximize;
            _showThemeToggle = showThemeToggle;

            _buttons = new List<CaptionButtonKind> { CaptionButtonKind.Close };
            if (_showMinimize)
                _buttons.Add(CaptionButtonKind.Minimize);
            if (_showMaximize)
                _buttons.Add(CaptionButtonKind.Maximize);

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

        private Rectangle ButtonRect(int visibleIndex)
        {
            return new Rectangle(visibleIndex * ButtonWidth, 0, ButtonWidth, Height);
        }

        private Rectangle ButtonClusterRect()
        {
            return new Rectangle(0, 0, _buttons.Count * ButtonWidth, Height);
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
            if (ButtonClusterRect().Contains(clientPoint))
                return true;

            if (_showThemeToggle && ThemeToggleRect().Contains(clientPoint))
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

            if (prevButton != _hoverButtonIndex || wasThemeHover != _themeHover)
                Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hoverButtonIndex = -1;
            _themeHover = false;
            Invalidate();
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
                ThemeManager.Toggle();
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

        private void PaintTitle(Graphics g, ThemePalette palette)
        {
            string title = _owner.Text;
            if (string.IsNullOrEmpty(title))
                return;

            using (Font titleFont = new Font(AppFonts.FamilyName, 9.5f, FontStyle.Bold))
            {
                SizeF textSize = g.MeasureString(title, titleFont);

                int leftReserved = ButtonClusterRect().Right + 10;
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
