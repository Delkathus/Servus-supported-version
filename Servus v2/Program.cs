using Servus_v2.Views;
using System;
using System.Windows.Forms;

namespace Servus_v2
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(@"fatal error during Application startup... {0}", ex));
            }
        }
    }
}