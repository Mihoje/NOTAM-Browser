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
                Debug.WriteLine($"Program: An error occurred during application startup: {ex}");
                // Optionally log the error or handle it as needed
                System.IO.File.WriteAllText("crash.log", $"{ex.Message}\n| {ex.StackTrace}");
            }
        }
    }
}
