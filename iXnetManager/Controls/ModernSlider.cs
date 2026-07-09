using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using iXnetManager.Theme;

namespace iXnetManager.Controls
{
    /// <summary>
    /// An Apple / macOS style slider: flat rounded track, accent-colored
    /// fill and a round thumb with a soft shadow. Exposes the same
    /// Minimum/Maximum/Value/Scroll surface as System.Windows.Forms.TrackBar
    /// so it is a drop-in replacement.
    /// </summary>
    public class ModernSlider : Control
    {
        private int _minimum = 0;
        private int _maximum = 100;
        private int _value = 0;
        private bool _dragging;
        private bool _hoverThumb;
        private readonly EventHandler _themeChangedHandler;

        [Category("Behavior")]
        public event EventHandler Scroll;

        [Category("Behavior")]
        public event EventHandler ValueChanged;

        public ModernSlider()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint |
                      ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw |
                      ControlStyles.SupportsTransparentBackColor, true);

            Size = new Size(150, 32);
            BackColor = Color.Transparent;

            _themeChangedHandler = delegate { Invalidate(); };
            ThemeManager.ThemeChanged += _themeChangedHandler;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                ThemeManager.ThemeChanged -= _themeChangedHandler;

            base.Dispose(disposing);
        }

        [Category("Behavior")]
        [DefaultValue(0)]
        public int Minimum
        {
            get { return _minimum; }
            set
            {
                _minimum = value;
                if (_maximum < _minimum)
                    _maximum = _minimum;
                Value = Clamp(_value);
                Invalidate();
            }
        }

        [Category("Behavior")]
        [DefaultValue(100)]
        public int Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;
                if (_minimum > _maximum)
                    _minimum = _maximum;
                Value = Clamp(_value);
                Invalidate();
            }
        }

        [Category("Behavior")]
        [DefaultValue(0)]
        public int Value
        {
            get { return _value; }
            set
            {
                int clamped = Clamp(value);
                if (clamped == _value)
                    return;

                _value = clamped;
                Invalidate();

                EventHandler handler = ValueChanged;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }
        }

        // Kept only for API-compatibility with TrackBar; not used for painting.
        [Category("Appearance")]
        [DefaultValue(TickStyle.None)]
        public TickStyle TickStyle { get; set; }

        private int Clamp(int v)
        {
            if (v < _minimum) return _minimum;
            if (v > _maximum) return _maximum;
            return v;
        }

        private Rectangle GetTrackRect()
        {
            int trackHeight = 4;
            int thumbRadius = ThumbRadius;
            int y = (Height - trackHeight) / 2;
            return new Rectangle(thumbRadius, y, Math.Max(0, Width - thumbRadius * 2), trackHeight);
        }

        private int ThumbRadius
        {
            get { return Math.Min(9, Height / 2 - 1); }
        }

        private float ValueToFraction()
        {
            if (_maximum == _minimum)
                return 0f;

            return (float)(_value - _minimum) / (_maximum - _minimum);
        }

        private int FractionToValue(float fraction)
        {
            if (fraction < 0f) fraction = 0f;
            if (fraction > 1f) fraction = 1f;
            return _minimum + (int)Math.Round(fraction * (_maximum - _minimum));
        }

        private PointF GetThumbCenter()
        {
            Rectangle track = GetTrackRect();
            float cx = track.X + track.Width * ValueToFraction();
            float cy = Height / 2f;
            return new PointF(cx, cy);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left)
                return;

            _dragging = true;
            UpdateValueFromMouse(e.X);
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            PointF center = GetThumbCenter();
            double dist = Math.Sqrt(Math.Pow(e.X - center.X, 2) + Math.Pow(e.Y - center.Y, 2));
            bool hover = dist <= ThumbRadius + 2;
            if (hover != _hoverThumb)
            {
                _hoverThumb = hover;
                Invalidate();
            }

            if (_dragging)
                UpdateValueFromMouse(e.X);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (!_dragging)
                return;

            _dragging = false;

            EventHandler handler = Scroll;
            if (handler != null)
                handler(this, EventArgs.Empty);

            Invalidate();
        }

        private void UpdateValueFromMouse(int x)
        {
            Rectangle track = GetTrackRect();
            float fraction = track.Width <= 0 ? 0f : (float)(x - track.X) / track.Width;
            int newValue = FractionToValue(fraction);

            if (newValue != _value)
            {
                Value = newValue;

                EventHandler handler = Scroll;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Left || keyData == Keys.Right)
                return true;

            return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Left)
            {
                Value = Clamp(_value - 1);
                RaiseScroll();
            }
            else if (e.KeyCode == Keys.Right)
            {
                Value = Clamp(_value + 1);
                RaiseScroll();
            }
        }

        private void RaiseScroll()
        {
            EventHandler handler = Scroll;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            ThemePalette palette = ThemeManager.Current;
            Rectangle track = GetTrackRect();

            using (GraphicsPath emptyPath = RoundedRect(track, track.Height / 2))
            using (SolidBrush emptyBrush = new SolidBrush(Enabled ? palette.SliderTrackEmpty : Color.FromArgb(200, palette.SliderTrackEmpty)))
            {
                g.FillPath(emptyBrush, emptyPath);
            }

            float fraction = ValueToFraction();
            int filledWidth = (int)(track.Width * fraction);
            if (filledWidth > 0)
            {
                Rectangle filledRect = new Rectangle(track.X, track.Y, Math.Max(track.Height, filledWidth), track.Height);
                using (GraphicsPath filledPath = RoundedRect(filledRect, track.Height / 2))
                using (SolidBrush filledBrush = new SolidBrush(Enabled ? palette.SliderTrackFilled : Color.FromArgb(150, palette.SliderTrackFilled)))
                {
                    g.FillPath(filledBrush, filledPath);
                }
            }

            PointF center = GetThumbCenter();
            int radius = ThumbRadius;
            if (_hoverThumb || _dragging)
                radius += 1;

            RectangleF thumbRect = new RectangleF(center.X - radius, center.Y - radius, radius * 2, radius * 2);

            using (var shadowPath = new GraphicsPath())
            {
                shadowPath.AddEllipse(thumbRect.X, thumbRect.Y + 0.8f, thumbRect.Width, thumbRect.Height);
                using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(50, 0, 0, 0)))
                    g.FillPath(shadowBrush, shadowPath);
            }

            using (SolidBrush thumbBrush = new SolidBrush(palette.SliderThumb))
                g.FillEllipse(thumbBrush, thumbRect);

            using (Pen borderPen = new Pen(palette.SliderThumbBorder, 1f))
                g.DrawEllipse(borderPen, thumbRect);
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            if (radius <= 0 || bounds.Width <= 0 || bounds.Height <= 0)
            {
                var rectPath = new GraphicsPath();
                if (bounds.Width > 0 && bounds.Height > 0)
                    rectPath.AddRectangle(bounds);
                return rectPath;
            }

            int diameter = radius * 2;
            diameter = Math.Min(diameter, Math.Min(bounds.Width, bounds.Height));

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
            get { return new Size(150, 32); }
        }
    }
}
