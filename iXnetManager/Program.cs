using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace iXnetManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Must be the very first call: without this the app is not
            // marked DPI-aware, so on scaled displays Windows renders it
            // at 96 DPI and then bitmap-stretches the result to fit -
            // which is what causes blurry text and oversized controls.
            try { Application.SetHighDpiMode(HighDpiMode.SystemAware); } catch { }

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                try
                {
                    var ex = e.ExceptionObject as Exception;
                    System.IO.File.WriteAllText(
                        System.IO.Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                            "ixnet_crash.log"),
                        (ex != null ? ex.ToString() : "Unknown error") +
                        "\n--- Unhandled at " + DateTime.Now + " ---");
                }
                catch { }
            };
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
