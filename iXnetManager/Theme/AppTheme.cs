using System;
using System.Drawing;

namespace iXnetManager.Theme
{
    /// <summary>
    /// Available visual themes. Mirrors macOS Light / Dark appearance.
    /// </summary>
    public enum ThemeMode
    {
        Light,
        Dark
    }

    /// <summary>
    /// A complete set of colors used to paint the UI. Two instances exist
    /// (Light / Dark) inside <see cref="ThemeManager"/>.
    /// </summary>
    public sealed class ThemePalette
    {
        public Color WindowBackground;
        public Color TitleBarBackground;
        public Color TitleBarText;
        public Color CardBackground;
        public Color CardBorder;
        public Color TextPrimary;
        public Color TextSecondary;
        public Color Accent;
        public Color AccentText;
        public Color ToggleOn;
        public Color ToggleOff;
        public Color ToggleKnob;
        public Color ControlBackground;
        public Color ControlBorder;
        public Color ButtonBackground;
        public Color ButtonBorder;
        public Color ButtonHoverBackground;
        public Color ButtonPressedBackground;
        public Color ButtonText;
        public Color PrimaryButtonBackground;
        public Color PrimaryButtonHoverBackground;
        public Color PrimaryButtonPressedBackground;
        public Color PrimaryButtonText;
        public Color DisabledButtonBackground;
        public Color DisabledButtonForeColor;
        public Color DisabledButtonBorder;
        public Color Divider;
        public Color TrafficRed;
        public Color TrafficYellow;
        public Color TrafficGreen;
        public Color TrafficGlyph;
        public Color TabSelectedBackground;
        public Color TabUnselectedText;
        public Color SliderTrackFilled;
        public Color SliderTrackEmpty;
        public Color SliderThumb;
        public Color SliderThumbBorder;
    }

    /// <summary>
    /// Central place that owns the current theme (Light/Dark), exposes the
    /// active color palette and notifies all open forms/controls when the
    /// user switches theme via the title bar toggle.
    /// </summary>
    public static class ThemeManager
    {
        public static event EventHandler ThemeChanged;

        public static ThemeMode CurrentMode { get; private set; } = ThemeMode.Light;

        public static readonly ThemePalette Light = new ThemePalette
        {
            WindowBackground = Color.FromArgb(246, 246, 248),
            TitleBarBackground = Color.FromArgb(236, 236, 240),
            TitleBarText = Color.FromArgb(40, 40, 45),
            CardBackground = Color.White,
            CardBorder = Color.FromArgb(226, 226, 230),
            TextPrimary = Color.FromArgb(30, 30, 34),
            TextSecondary = Color.FromArgb(110, 110, 118),
            Accent = Color.FromArgb(0, 122, 255),
            AccentText = Color.White,
            ToggleOn = Color.FromArgb(52, 199, 89),
            ToggleOff = Color.FromArgb(224, 224, 228),
            ToggleKnob = Color.White,
            ControlBackground = Color.White,
            ControlBorder = Color.FromArgb(210, 210, 216),
            ButtonBackground = Color.FromArgb(233, 233, 237),
            ButtonBorder = Color.FromArgb(210, 210, 216),
            ButtonHoverBackground = Color.FromArgb(222, 222, 227),
            ButtonPressedBackground = Color.FromArgb(206, 206, 212),
            ButtonText = Color.FromArgb(30, 30, 34),
            PrimaryButtonBackground = Color.FromArgb(0, 122, 255),
            PrimaryButtonHoverBackground = Color.FromArgb(20, 137, 255),
            PrimaryButtonPressedBackground = Color.FromArgb(0, 100, 220),
            PrimaryButtonText = Color.White,
            DisabledButtonBackground = Color.FromArgb(241, 241, 244),
            DisabledButtonForeColor = Color.FromArgb(175, 175, 181),
            DisabledButtonBorder = Color.FromArgb(230, 230, 234),
            Divider = Color.FromArgb(226, 226, 230),
            TrafficRed = Color.FromArgb(255, 95, 87),
            TrafficYellow = Color.FromArgb(255, 189, 46),
            TrafficGreen = Color.FromArgb(40, 200, 64),
            TrafficGlyph = Color.FromArgb(77, 0, 0),
            TabSelectedBackground = Color.White,
            TabUnselectedText = Color.FromArgb(110, 110, 118),
            SliderTrackFilled = Color.FromArgb(0, 122, 255),
            SliderTrackEmpty = Color.FromArgb(224, 224, 228),
            SliderThumb = Color.White,
            SliderThumbBorder = Color.FromArgb(200, 200, 206),
        };

        public static readonly ThemePalette Dark = new ThemePalette
        {
            WindowBackground = Color.FromArgb(30, 30, 32),
            TitleBarBackground = Color.FromArgb(38, 38, 40),
            TitleBarText = Color.FromArgb(235, 235, 240),
            CardBackground = Color.FromArgb(44, 44, 47),
            CardBorder = Color.FromArgb(58, 58, 62),
            TextPrimary = Color.FromArgb(240, 240, 242),
            TextSecondary = Color.FromArgb(160, 160, 168),
            Accent = Color.FromArgb(10, 132, 255),
            AccentText = Color.White,
            ToggleOn = Color.FromArgb(48, 209, 88),
            ToggleOff = Color.FromArgb(72, 72, 76),
            ToggleKnob = Color.White,
            ControlBackground = Color.FromArgb(54, 54, 58),
            ControlBorder = Color.FromArgb(70, 70, 75),
            ButtonBackground = Color.FromArgb(58, 58, 62),
            ButtonBorder = Color.FromArgb(75, 75, 80),
            ButtonHoverBackground = Color.FromArgb(70, 70, 75),
            ButtonPressedBackground = Color.FromArgb(48, 48, 52),
            ButtonText = Color.FromArgb(240, 240, 242),
            PrimaryButtonBackground = Color.FromArgb(10, 132, 255),
            PrimaryButtonHoverBackground = Color.FromArgb(35, 150, 255),
            PrimaryButtonPressedBackground = Color.FromArgb(0, 110, 225),
            PrimaryButtonText = Color.White,
            DisabledButtonBackground = Color.FromArgb(46, 46, 49),
            DisabledButtonForeColor = Color.FromArgb(105, 105, 110),
            DisabledButtonBorder = Color.FromArgb(56, 56, 60),
            Divider = Color.FromArgb(58, 58, 62),
            TrafficRed = Color.FromArgb(255, 95, 87),
            TrafficYellow = Color.FromArgb(255, 189, 46),
            TrafficGreen = Color.FromArgb(40, 200, 64),
            TrafficGlyph = Color.FromArgb(40, 0, 0),
            TabSelectedBackground = Color.FromArgb(44, 44, 47),
            TabUnselectedText = Color.FromArgb(160, 160, 168),
            SliderTrackFilled = Color.FromArgb(10, 132, 255),
            SliderTrackEmpty = Color.FromArgb(72, 72, 76),
            SliderThumb = Color.FromArgb(235, 235, 240),
            SliderThumbBorder = Color.FromArgb(20, 20, 22),
        };

        public static ThemePalette Current
        {
            get { return CurrentMode == ThemeMode.Light ? Light : Dark; }
        }

        public static void SetMode(ThemeMode mode)
        {
            if (CurrentMode == mode)
                return;

            CurrentMode = mode;
            var handler = ThemeChanged;
            if (handler != null)
                handler(null, EventArgs.Empty);
        }

        public static void Toggle()
        {
            SetMode(CurrentMode == ThemeMode.Light ? ThemeMode.Dark : ThemeMode.Light);
        }
    }
}
