using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace iXnet.LedPort
{
    [Serializable]
    public struct LedColor
    {
        public byte R;
        public byte G;
        public byte B;

        public LedColor(byte red, byte green, byte blue)
        {
            R = red;
            G = green;
            B = blue;
        }

        public KnownLedColor ToKnownColor()
        {
            int max = Enum.GetNames(typeof(KnownLedColor)).Length - 1;   // reduced by unknown entry
            for (int i = 0; i < max; ++i)
            {
                if (Object.Equals(this, FromKnownLedColor((KnownLedColor)i)))
                    return (KnownLedColor)i;
            }

            return KnownLedColor.Unknown;
        }

        public static LedColor FromKnownLedColor(KnownLedColor color)
        {
            switch (color)
            {
                case KnownLedColor.Red:
                    return Red;
                case KnownLedColor.Green:
                    return Green;
                case KnownLedColor.Blue:
                    return Blue;
                case KnownLedColor.Yellow:
                    return Yellow;
                case KnownLedColor.White:
                    return White;
                case KnownLedColor.Cyan:
                    return Cyan;
                case KnownLedColor.Pink:
                    return Pink;
            }
            return Black;
        }

        /// <summary>
        /// Parses the color as either color name or hex value
        /// </summary>
        /// <param name="colorValue"></param>
        /// <returns></returns>
        public static LedColor Parse(string colorValue)
        {
            LedColor color;
            if (!TryParse(colorValue, out color))
                throw new FormatException(String.Format("Value {0} is not a valid led color", colorValue));

            return color;
        }

        public static bool TryParse(string colorValue, out LedColor color)
        {
            KnownLedColor knownColor;
            if (Enum.TryParse<KnownLedColor>(colorValue, true, out knownColor))
            {
                color = FromKnownLedColor(knownColor);
                return true;
            }

            if (colorValue.Trim().StartsWith("#"))
                colorValue = colorValue.Trim().Substring(1);

            int colorAsInt;
            if (int.TryParse(colorValue, System.Globalization.NumberStyles.HexNumber, CultureInfo.CurrentCulture, out colorAsInt))
            {
                byte red = (byte)((colorAsInt >> 16) & 0xff);
                byte green = (byte)((colorAsInt >> 8) & 0xff);
                byte blue = (byte)((colorAsInt) & 0xff);

                color = new LedColor() { R = red, G = green, B = blue };
                return true;
            }

            color = White;
            return false;
        }

        public string ToHtml()
        {
            return System.Drawing.ColorTranslator.ToHtml(ToColor());
        }

        public System.Drawing.Color ToColor()
        {
            return System.Drawing.Color.FromArgb(R, G, B);
        }

        public static bool operator ==(LedColor a, LedColor b)
        {
            return Object.Equals(a, b);
        }

        public static bool operator !=(LedColor a, LedColor b)
        {
            return !Object.Equals(a, b);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return String.Format("({0},{1},{2})", R, G, B);
        }

        public static readonly LedColor White = new LedColor(255, 255, 255);
        public static readonly LedColor Black = new LedColor(0, 0, 0);
        public static readonly LedColor Red = new LedColor(255, 0, 0);
        public static readonly LedColor Green = new LedColor(0, 255, 0);
        public static readonly LedColor Blue = new LedColor(0, 0, 255);
        public static readonly LedColor Yellow = new LedColor(255, 255, 0);
        public static readonly LedColor Cyan = new LedColor(0, 255, 255);
        public static readonly LedColor Pink = new LedColor(255, 0, 255);
    }
}
