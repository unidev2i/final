using System;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class ImportTpInfo : Form
    {
        #region Public Fields

        public static string message = string.Empty;

        #endregion Public Fields

        #region Public Constructors

        public ImportTpInfo()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void ImportTpInfo_Load(object sender, EventArgs e)
        {
            //FormBorderStyle = FormBorderStyle.None;
            webBrowser1.AllowNavigation = false;
            webBrowser1.AllowWebBrowserDrop = false;
            //webBrowser1.CanGoBack = false;
            webBrowser1.DocumentText = message;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        }

        #endregion Private Methods
    }
}
