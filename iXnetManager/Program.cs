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
