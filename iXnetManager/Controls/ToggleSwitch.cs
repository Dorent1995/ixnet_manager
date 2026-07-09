using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using iXnetManager.Theme;

namespace iXnetManager.Controls
{
    /// <summary>
    /// An Apple / macOS style toggle switch. Green when on, gray when off.
    /// Exposes a <see cref="Checked"/> property and <see cref="CheckedChanged"/>
    /// event so it is a drop-in replacement for a CheckBox with
    /// AutoCheck = false (the control never toggles itself - the owner sets
    /// Checked explicitly, exactly like the CheckBox it replaces).
    /// </summary>
    public class ToggleSwitch : Control
    {
        private bool _checked;
        private bool _hover;
        private float _knobPosition; // 0..1 animated position
        private readonly Timer _animationTimer;
        private readonly EventHandler _themeChangedHandler;

        [Category("Behavior")]
        public event EventHandler CheckedChanged;

        public ToggleSwitch()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                      ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                      ControlStyles.SupportsTransparentBackColor, true);

            Size = new Size(44, 24);
            Cursor = Cursors.Hand;
            BackColor = Color.Transparent;
            _knobPosition = 0f;

            _animationTimer = new Timer();
            _animationTimer.Interval = 12;
            _animationTimer.Tick += AnimationTimer_Tick;

            _themeChangedHandler = delegate { Invalidate(); };
            ThemeManager.ThemeChanged += _themeChangedHandler;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ThemeManager.ThemeChanged -= _themeChangedHandler;
                _animationTimer.Dispose();
            }

            base.Dispose(disposing);
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        public bool Checked
        {
            get { return _checked; }
            set
            {
                if (_checked == value)
                    return;

                _checked = value;
                StartAnimation();

                EventHandler handler = CheckedChanged;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }
        }

        private void StartAnimation()
        {
            if (!IsHandleCreated)
            {
                _knobPosition = _checked ? 1f : 0f;
                Invalidate();
                return;
            }

            _animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            float target = _checked ? 1f : 0f;
            float step = 0.28f;

            if (Math.Abs(_knobPosition - target) < 0.02f)
            {
                _knobPosition = target;
                _animationTimer.Stop();
            }
            else
            {
                _knobPosition += (target - _knobPosition) * step;
            }

            Invalidate();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            // Note: this control intentionally does NOT flip Checked itself.
            // The owning form decides (mirrors the original CheckBox with
            // AutoCheck = false) - it sets Checked only after the action
            // that the click triggers has actually succeeded.
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _hover = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hover = false;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            ThemePalette palette = ThemeManager.Current;

            int trackHeight = Math.Min(Height, 24);
            int trackWidth = Math.Min(Width, 44);
            int x = (Width - trackWidth) / 2;
            int y = (Height - trackHeight) / 2;

            Rectangle trackRect = new Rectangle(x, y, trackWidth, trackHeight);

            Color trackColor = _checked ? palette.ToggleOn : palette.ToggleOff;
            if (!Enabled)
                trackColor = Color.FromArgb(160, trackColor);

            if (_hover && Enabled)
                trackColor = ControlPaint.Light(trackColor, 0.05f);

            using (GraphicsPath path = RoundedRect(trackRect, trackHeight / 2))
            using (SolidBrush brush = new SolidBrush(trackColor))
            {
                g.FillPath(brush, path);
            }

            int knobDiameter = trackHeight - 4;
            float travel = trackWidth - knobDiameter - 4;
            float knobX = x + 2 + travel * _knobPosition;
            float knobY = y + 2;

            RectangleF knobRect = new RectangleF(knobX, knobY, knobDiameter, knobDiameter);

            using (var shadowPath = new GraphicsPath())
            {
                shadowPath.AddEllipse(knobRect.X, knobRect.Y + 0.6f, knobRect.Width, knobRect.Height);
                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(40, 0, 0, 0)))
                    g.FillPath(shadowBrush, shadowPath);
            }

            using (SolidBrush knobBrush = new SolidBrush(palette.ToggleKnob))
                g.FillEllipse(knobBrush, knobRect);
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            var path = new GraphicsPath();
            var arc = new Rectangle(bounds.Location, new Size(diameter, diameter));

            path.AddArc(arc, 180, 90);

            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        protected override Size DefaultSize
        {
            get { return new Size(44, 24); }
        }
    }
}
