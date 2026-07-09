using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BrightIdeasSoftware;
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
                // IMPORTANT: never reassign form.Font here. These forms use
                // AutoScaleMode.Font (the WinForms default) - changing a
                // Form's own Font at runtime makes WinForms silently
                // re-run its design-time auto-scale pass, rescaling every
                // child control's Location/Size a second time against the
                // NEW font metrics. That is what caused the distorted /
                // overlapping layout. Individual child controls still get
                // AppFonts applied below (that's safe - it does not
                // trigger the Form-level rescale).

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

            // ListView/ObjectListView can cache row height from the Font
            // that was active when the control's handle was created;
            // reassigning Font afterwards has been observed to leave the
            // header/first row visually out of sync with the rest of the
            // list. Their design-time font already looks fine, so we only
            // touch their colors below, not the font.
            if (!(c is ListView))
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
                c.BackColor = ParentBackground(c, p);
            }
            else if (c is CheckBox)
            {
                c.ForeColor = p.TextPrimary;
                c.BackColor = ParentBackground(c, p);
            }
            else if (c is RadioButton)
            {
                c.ForeColor = p.TextPrimary;
                c.BackColor = ParentBackground(c, p);
            }
            else if (c is ObjectListView)
            {
                StyleObjectListView((ObjectListView)c);
            }
            else if (c is ListView)
            {
                StylePlainListView((ListView)c);
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
                c.BackColor = ParentBackground(c, p);
            }
        }

        /// <summary>
        /// Returns the effective background color the parent control is
        /// painted with, so children can match it exactly instead of
        /// relying on WinForms' BackColor = Transparent "parent paint"
        /// trick, which can leave stale white patches (especially in Dark
        /// mode, and around Region-rounded siblings).
        /// </summary>
        private static Color ParentBackground(Control c, ThemePalette p)
        {
            if (c.Parent == null)
                return p.WindowBackground;

            if (c.Parent is TabPage || c.Parent is TableLayoutPanel || c.Parent is Panel)
                return p.WindowBackground;

            return c.Parent.BackColor;
        }

        private static void StyleTabControl(TabControl tab)
        {
            tab.DrawMode = TabDrawMode.OwnerDrawFixed;

            // Deliberately NOT SizeMode.Fixed: a single fixed width for all
            // tabs either clips/wraps long labels ("iXnet Manager Settings")
            // or wastes space on short ones ("Logs"). Normal sizing lets the
            // native tab strip measure each tab against its own Font/Text,
            // same as before this control was owner-drawn - only colors and
            // the accent underline are custom, layout is untouched.
            tab.Padding = new Point(16, 6);

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

        private static void StyleObjectListView(ObjectListView olv)
        {
            ThemePalette p = ThemeManager.Current;

            olv.BackColor = p.ControlBackground;
            olv.ForeColor = p.TextPrimary;

            // ObjectListView draws its own header separately from the
            // BackColor above; without this it stays a native white/themed
            // header even in Dark mode.
            olv.HeaderUsesThemes = false;
            olv.HeaderWordWrap = false;

            HeaderFormatStyle style = new HeaderFormatStyle();
            style.Normal.BackColor = p.WindowBackground;
            style.Normal.ForeColor = p.TextPrimary;
            style.Hot.BackColor = p.ButtonHoverBackground;
            style.Hot.ForeColor = p.TextPrimary;
            style.Pressed.BackColor = p.ButtonPressedBackground;
            style.Pressed.ForeColor = p.TextPrimary;
            olv.HeaderFormatStyle = style;
        }

        private static void StylePlainListView(ListView listView)
        {
            ThemePalette p = ThemeManager.Current;

            listView.BackColor = p.ControlBackground;
            listView.ForeColor = p.TextPrimary;

            if (listView.View != View.Details)
                return;

            // Plain System.Windows.Forms.ListView column headers are drawn
            // by the OS and ignore BackColor entirely - owner-draw them so
            // Dark mode does not leave a white header bar.
            listView.OwnerDraw = true;

            listView.DrawColumnHeader -= ListView_DrawColumnHeader;
            listView.DrawColumnHeader += ListView_DrawColumnHeader;
            listView.DrawItem -= ListView_DrawItem;
            listView.DrawItem += ListView_DrawItem;
            listView.DrawSubItem -= ListView_DrawSubItem;
            listView.DrawSubItem += ListView_DrawSubItem;
        }

        private static void ListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            ThemePalette p = ThemeManager.Current;

            using (SolidBrush backBrush = new SolidBrush(p.WindowBackground))
                e.Graphics.FillRectangle(backBrush, e.Bounds);

            TextRenderer.DrawText(e.Graphics, e.Header.Text, e.Font, e.Bounds, p.TextPrimary,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

            using (Pen linePen = new Pen(p.Divider))
                e.Graphics.DrawLine(linePen, e.Bounds.Right - 1, e.Bounds.Top, e.Bounds.Right - 1, e.Bounds.Bottom);
        }

        private static void ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private static void ListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
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

            // Read-only properties get a visibly muted color so they
            // are clearly distinguishable from editable ones.
            grid.DisabledItemForeColor = p.TextSecondary;
        }
    }
}
