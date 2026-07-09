using System.Drawing;
using System.Drawing.Text;

namespace iXnetManager.Theme
{
    /// <summary>
    /// Resolves the best available "Apple-like" system font at runtime.
    /// Prefers Segoe UI Variable (Windows 11) and falls back to Segoe UI
    /// (Windows 7/8/10) so the app always renders with a clean, modern
    /// typeface regardless of the OS version it runs on.
    /// </summary>
    public static class AppFonts
    {
        private static readonly string _familyName;

        static AppFonts()
        {
            _familyName = "Segoe UI";

            try
            {
                using (var installed = new InstalledFontCollection())
                {
                    bool hasVariableText = false;
                    bool hasVariable = false;

                    foreach (FontFamily family in installed.Families)
                    {
                        if (family.Name == "Segoe UI Variable Text")
                            hasVariableText = true;
                        else if (family.Name == "Segoe UI Variable")
                            hasVariable = true;
                    }

                    if (hasVariableText)
                        _familyName = "Segoe UI Variable Text";
                    else if (hasVariable)
                        _familyName = "Segoe UI Variable";
                }
            }
            catch
            {
                _familyName = "Segoe UI";
            }
        }

        public static string FamilyName
        {
            get { return _familyName; }
        }

        public static Font Regular(float size)
        {
            return new Font(_familyName, size, FontStyle.Regular, GraphicsUnit.Point);
        }

        public static Font Semibold(float size)
        {
            return new Font(_familyName, size, FontStyle.Bold, GraphicsUnit.Point);
        }

        public static Font Bold(float size)
        {
            return new Font(_familyName, size, FontStyle.Bold, GraphicsUnit.Point);
        }

        /// <summary>Returns a font of the same size/style as the source font, using the resolved family.</summary>
        public static Font Rescale(Font source)
        {
            if (source == null)
                return Regular(9f);

            return new Font(_familyName, source.Size, source.Style, GraphicsUnit.Point);
        }
    }
}
