using System;
using System.Windows.Forms;
/*Amuni ang app entry point, ga initialize sya sang windows forms or ang `ApplicationConfiguration.Initialize()`
kag mag start message loop by running */
namespace BugTrackerApp
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            var args = Environment.GetCommandLineArgs();

            if (args.Contains("--benchmark"))
            {
                CollectionBenchmark.Run();
                return;
            }

            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
