using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
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
#if !EXCEPT
            try
            {
#endif
                if (Properties.Settings.Default.upgradeRequired)
                {
                    Properties.Settings.Default.Upgrade(); // Migrates settings from the previous version
                    Properties.Settings.Default.upgradeRequired = false; // Prevents re-running the upgrade on subsequent starts
                    Properties.Settings.Default.Save(); // Persists the new setting value
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var splash = new frmSplash();

                System.Threading.Thread splashThread = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(splash);
                }));
                splashThread.SetApartmentState(System.Threading.ApartmentState.STA);
                splashThread.Start();

                var nos = new Notams();

                var main = new frmMain(nos);

                var unused = main.Handle;

                splash.MainForm = main;

                Application.Run(main);

#if !EXCEPT
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine($"Program: An error occurred during application startup: {ex}");
#endif
                System.IO.File.WriteAllText("crash.log", $"{ex.Message}\n| {ex.StackTrace}");
            }
#endif
        }
    }
}
