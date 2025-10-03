using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace NOTAM_Browser
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine($"Program: An error occurred during application startup: {ex}");
#endif
#if EXCEPT
                throw ex;
#else
                System.IO.File.WriteAllText("crash.log", $"{ex.Message}\n| {ex.StackTrace}");
#endif
            }
        }
    }
}
