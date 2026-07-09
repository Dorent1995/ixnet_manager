using System.Drawing;

namespace iXnetManager.Theme
{
    /// <summary>
    /// The app-wide UI font. Deliberately just the classic, GDI-native
    /// "Segoe UI" - not "Segoe UI Variable". Variable fonts are a
    /// DirectWrite-era technology; GDI+/GDI text rendering (what WinForms
    /// uses) can render them inconsistently or blurrily depending on the
    /// Windows version, so we stick to the well-supported static family
    /// that has rendered crisply via ClearType since Windows Vista.
    /// </summary>
    public static class AppFonts
    {
        private const string FamilyNameValue = "Segoe UI";

        public static string FamilyName
        {
            get { return FamilyNameValue; }
        }

        public static Font Regular(float size)
        {
            return new Font(FamilyNameValue, size, FontStyle.Regular, GraphicsUnit.Point);
        }

        public static Font Semibold(float size)
        {
            return new Font(FamilyNameValue, size, FontStyle.Bold, GraphicsUnit.Point);
        }

        public static Font Bold(float size)
        {
            return new Font(FamilyNameValue, size, FontStyle.Bold, GraphicsUnit.Point);
        }

        /// <summary>Returns a font of the same size/style as the source font, using the app family.</summary>
        public static Font Rescale(Font source)
        {
            if (source == null)
                return Regular(9f);

            return new Font(FamilyNameValue, source.Size, source.Style, GraphicsUnit.Point);
        }
    }
}
