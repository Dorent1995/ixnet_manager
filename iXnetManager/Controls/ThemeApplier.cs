using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using iXnetManager.Theme;

namespace iXnetManager.Controls
{
    /// <summary>
    /// Recursively applies the current theme's colors and the app font to
    /// every control on a form (buttons, labels, text boxes, combo boxes,
    /// tabs, the property grid, ...). Call once after InitializeComponent().
    /// Re-runs automatically whenever the user toggles Light/Dark from the
    /// title bar, so every open window updates live.
    /// </summary>
    public static class ThemeApplier
    {
        public static void Apply(Form form, params Control[] primaryButtons)
        {
            if (form == null)
                return;

            HashSet<Control> primary = new HashSet<Control>(primaryButtons ?? new Control[0]);

            EventHandler apply = null;
            apply = delegate
            {
                form.Font = AppFonts.Regular(9f);

                // BaseChromeForm uses its own BackColor as the 1px window
                // border color (see InstallChrome); the actual background
                // is provided by the docked content panel/table layout
                // beneath the title bar, which is themed below. Plain
                // forms (e.g. ProgressForm) have no such border trick, so
                // their BackColor IS the visible window background.
                if (!(form is BaseChromeForm))
                    form.BackColor = ThemeManager.Current.WindowBackground;

                ApplyRecursive(form, primary);
            };

            apply(null, EventArgs.Empty);

            ThemeManager.ThemeChanged += apply;
            form.FormClosed += delegate { ThemeManager.ThemeChanged -= apply; };
        }

        private static void ApplyRecursive(Control root, HashSet<Control> primaryButtons)
        {
            foreach (Control c in root.Controls)
            {
                StyleControl(c, primaryButtons);

                if (c.HasChildren)
                    ApplyRecursive(c, primaryButtons);
            }
        }

        private static void StyleControl(Control c, HashSet<Control> primaryButtons)
        {
            if (c is MacTitleBar || c is ToggleSwitch || c is ModernSlider)
                return; // these paint themselves fully based on ThemeManager.Current

            ThemePalette p = ThemeManager.Current;
            c.Font = AppFonts.Rescale(c.Font);

            if (c is Button)
            {
                RoundedControls.StyleButton((Button)c, primaryButtons.Contains(c));
            }
            else if (c is TextBox)
            {
                c.BackColor = p.ControlBackground;
                c.ForeColor = p.TextPrimary;
                ((TextBox)c).BorderStyle = BorderStyle.FixedSingle;
            }
            else if (c is ComboBox)
            {
                c.BackColor = p.ControlBackground;
                c.ForeColor = p.TextPrimary;
                ((ComboBox)c).FlatStyle = FlatStyle.Flat;
            }
            else if (c is NumericUpDown)
            {
                c.BackColor = p.ControlBackground;
                c.ForeColor = p.TextPrimary;
            }
            else if (c is TabControl)
            {
                StyleTabControl((TabControl)c);
            }
            else if (c is TabPage)
            {
                c.BackColor = p.WindowBackground;
                c.ForeColor = p.TextPrimary;
            }
            else if (c is PropertyGrid)
            {
                StylePropertyGrid((PropertyGrid)c);
            }
            else if (c is GroupBox)
            {
                c.ForeColor = p.TextPrimary;
                c.BackColor = Color.Transparent;
            }
            else if (c is CheckBox)
            {
                c.ForeColor = p.TextPrimary;
                c.BackColor = Color.Transparent;
            }
            else if (c is RadioButton)
            {
                c.ForeColor = p.TextPrimary;
                c.BackColor = Color.Transparent;
            }
            else if (c is ListView)
            {
                c.BackColor = p.ControlBackground;
                c.ForeColor = p.TextPrimary;
            }
            else if (c is TreeView)
            {
                c.BackColor = p.ControlBackground;
                c.ForeColor = p.TextPrimary;
            }
            else if (c is ProgressBar)
            {
                // ProgressBar visuals are owner-drawn separately where needed.
            }
            else if (c is TableLayoutPanel)
            {
                c.BackColor = p.WindowBackground;
            }
            else if (c is Panel)
            {
                c.BackColor = p.WindowBackground;
            }
            else if (c is Label)
            {
                c.ForeColor = p.TextPrimary;
                c.BackColor = Color.Transparent;
            }
        }

        private static void StyleTabControl(TabControl tab)
        {
            tab.DrawMode = TabDrawMode.OwnerDrawFixed;
            tab.SizeMode = TabSizeMode.Fixed;
            tab.Padding = new Point(14, 6);
            tab.ItemSize = new Size(Math.Max(120, tab.ItemSize.Width), 30);

            // -= before += so repeated Apply() calls (theme toggles) never
            // stack duplicate handlers.
            tab.DrawItem -= TabControl_DrawItem;
            tab.DrawItem += TabControl_DrawItem;
        }

        private static void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            TabControl tab = (TabControl) sender;
            ThemePalette p = ThemeManager.Current;

            if (e.Index < 0 || e.Index >= tab.TabPages.Count)
                return;

            TabPage page = tab.TabPages[e.Index];
            Rectangle bounds = tab.GetTabRect(e.Index);
            bool selected = tab.SelectedIndex == e.Index;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (SolidBrush backBrush = new SolidBrush(selected ? p.TabSelectedBackground : p.WindowBackground))
                e.Graphics.FillRectangle(backBrush, bounds);

            Color textColor = selected ? p.Accent : p.TabUnselectedText;
            using (Font tabFont = AppFonts.Semibold(9f))
            using (SolidBrush textBrush = new SolidBrush(textColor))
            {
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString(page.Text, tabFont, textBrush, bounds, sf);
            }

            if (selected)
            {
                using (SolidBrush accentBrush = new SolidBrush(p.Accent))
                {
                    Rectangle underline = new Rectangle(bounds.Left + 10, bounds.Bottom - 3, bounds.Width - 20, 3);
                    e.Graphics.FillRectangle(accentBrush, underline);
                }
            }
        }

        private static void StylePropertyGrid(PropertyGrid grid)
        {
            ThemePalette p = ThemeManager.Current;

            grid.BackColor = p.WindowBackground;
            grid.ViewBackColor = p.ControlBackground;
            grid.ViewForeColor = p.TextPrimary;
            grid.ViewBorderColor = p.ControlBorder;
            grid.LineColor = p.Divider;
            grid.CategoryForeColor = p.TextSecondary;
            grid.CategorySplitterColor = p.Divider;
            grid.HelpBackColor = p.CardBackground;
            grid.HelpForeColor = p.TextSecondary;
            grid.HelpBorderColor = p.Divider;
            grid.CommandsBackColor = p.CardBackground;
            grid.CommandsForeColor = p.Accent;
            grid.CommandsBorderColor = p.Divider;
            grid.SelectedItemWithFocusBackColor = p.Accent;
            grid.SelectedItemWithFocusForeColor = p.AccentText;
        }
    }
}
