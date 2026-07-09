using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using iXnetManager.Theme;

namespace iXnetManager.Controls
{
    /// <summary>
    /// Gives stock WinForms controls (Button, Panel, GroupBox, ...) a flat,
    /// rounded, macOS-like appearance at runtime - without having to change
    /// their declared type in the designer files. This keeps every existing
    /// field name / event wiring in the generated Designer.cs files intact;
    /// only visuals change.
    /// </summary>
    public static class RoundedControls
    {
        // Tracks which controls already have their one-time structural setup
        // (rounded Region + ThemeChanged subscription) wired, so re-applying
        // styling (e.g. after a Light/Dark toggle) never stacks up duplicate
        // event subscriptions.
        private static readonly HashSet<Control> _wiredButtons = new HashSet<Control>();
        private static readonly HashSet<Control> _wiredCards = new HashSet<Control>();

        /// <summary>Applies rounded corners + flat colors to a push button.</summary>
        public static void StyleButton(Button button, bool primary)
        {
            if (button == null)
                return;

            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = primary ? 0 : 1;
            button.Cursor = Cursors.Hand;
            button.UseVisualStyleBackColor = false;

            ApplyButtonColors(button, primary);

            if (_wiredButtons.Add(button))
            {
                EventHandler themeHandler = delegate { ApplyButtonColors(button, primary); };
                ThemeManager.ThemeChanged += themeHandler;

                // A disabled button (e.g. "Apply Changes" when nothing has
                // changed) must look visibly different from an enabled one -
                // FlatStyle with custom colors bypasses Windows' automatic
                // disabled-graying, so we re-apply colors ourselves whenever
                // Enabled flips.
                button.EnabledChanged += delegate { ApplyButtonColors(button, primary); };

                // Dialogs (SecurityTokenForm, AddDeviceForm, ...) can be
                // instantiated many times during a session. Without this,
                // every closed dialog's buttons would stay referenced
                // forever via the static ThemeChanged subscription.
                button.Disposed += delegate
                {
                    ThemeManager.ThemeChanged -= themeHandler;
                    _wiredButtons.Remove(button);
                };

                // Deliberately NOT rounding corners here: Control.Region is
                // a hard pixel mask with no anti-aliasing, so "rounded"
                // corners came out visibly jagged/staircase-shaped. Flat
                // rectangular buttons with flat colors read as clean and
                // modern without that artifact.
            }
        }

        private static void ApplyButtonColors(Button button, bool primary)
        {
            ThemePalette palette = ThemeManager.Current;

            if (!button.Enabled)
            {
                button.BackColor = palette.DisabledButtonBackground;
                button.ForeColor = palette.DisabledButtonForeColor;
                button.FlatAppearance.BorderColor = palette.DisabledButtonBorder;
                button.FlatAppearance.MouseOverBackColor = palette.DisabledButtonBackground;
                button.FlatAppearance.MouseDownBackColor = palette.DisabledButtonBackground;
                return;
            }

            if (primary)
            {
                button.BackColor = palette.PrimaryButtonBackground;
                button.ForeColor = palette.PrimaryButtonText;
                button.FlatAppearance.MouseOverBackColor = palette.PrimaryButtonHoverBackground;
                button.FlatAppearance.MouseDownBackColor = palette.PrimaryButtonPressedBackground;
            }
            else
            {
                button.BackColor = palette.ButtonBackground;
                button.ForeColor = palette.ButtonText;
                button.FlatAppearance.BorderColor = palette.ButtonBorder;
                button.FlatAppearance.MouseOverBackColor = palette.ButtonHoverBackground;
                button.FlatAppearance.MouseDownBackColor = palette.ButtonPressedBackground;
            }
        }

        /// <summary>Applies a rounded "card" region + theme card colors to a panel-like container.</summary>
        public static void StyleCard(Control panel)
        {
            if (panel == null)
                return;

            ApplyCardColor(panel);

            if (_wiredCards.Add(panel))
            {
                EventHandler themeHandler = delegate { ApplyCardColor(panel); };
                ThemeManager.ThemeChanged += themeHandler;

                panel.Disposed += delegate
                {
                    ThemeManager.ThemeChanged -= themeHandler;
                    _wiredCards.Remove(panel);
                };

                ApplyRoundedRegion(panel, 10);
            }
        }

        private static void ApplyCardColor(Control panel)
        {
            panel.BackColor = ThemeManager.Current.CardBackground;
        }

        /// <summary>
        /// Sets (and keeps up to date on resize) a rounded-rectangle Region
        /// on the given control, producing rounded corners without needing
        /// a custom-drawn control.
        /// </summary>
        public static void ApplyRoundedRegion(Control control, int radius)
        {
            if (control == null)
                return;

            EventHandler update = null;
            update = delegate
            {
                if (control.Width <= 0 || control.Height <= 0)
                    return;

                using (GraphicsPath path = RoundedRectPath(new Rectangle(0, 0, control.Width, control.Height), radius))
                {
                    var oldRegion = control.Region;
                    control.Region = new Region(path);
                    if (oldRegion != null)
                        oldRegion.Dispose();
                }
            };

            control.Resize += update;
            update(null, EventArgs.Empty);
        }

        public static GraphicsPath RoundedRectPath(Rectangle bounds, int radius)
        {
            int diameter = Math.Min(radius * 2, Math.Min(bounds.Width, bounds.Height));
            var path = new GraphicsPath();

            if (diameter <= 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

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
    }
}
