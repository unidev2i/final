using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    internal static class Program
    {
        #region Public Fields

        public static AssistantConnexion ac;

        #endregion Public Fields

        #region Private Methods

        //public static string repoPath;
        /// <summary>
        ///     Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            try
            {
                ProcessStartInfo startServ = new ProcessStartInfo("mysql\\start.exe");
                Process.Start(startServ);                                           //A décommenter avant mise en oeuvre
                System.Threading.Thread.Sleep(1000);

                Database.Connect();
            }
            catch
            {
                MessageBox.Show("Impossible de se connecter à la BDD");
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ac = new AssistantConnexion();
            Application.Run(ac);

            ProcessStartInfo stopServ = new ProcessStartInfo("mysql\\stop.exe");
            Process.Start(stopServ);                                            //A décommenter avant mise en oeuvre
            System.Threading.Thread.Sleep(1000);
        }

        #endregion Private Methods
    }
}
