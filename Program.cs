using System;
using System.Windows.Forms;

namespace ScreenFX
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if ((Environment.OSVersion.Version.Major < 7))
            {
                MessageBox.Show(Properties.Resources.OSVersionError_Text, Properties.Resources.OSVersionError_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Application.Run(new MainWindow());
            }
        }
    }
}
