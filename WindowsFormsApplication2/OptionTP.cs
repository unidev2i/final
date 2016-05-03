using System;
using System.Windows.Forms;
using WindowsFormsApplication2.Properties;

namespace WindowsFormsApplication2
{
    public partial class OptionTP : Form
    {
        #region Public Constructors

        public OptionTP()
        {
            InitializeComponent();
            if (Settings.Default.repoPath == string.Empty)
            {
                Settings.Default.repoPath = "Non défini!";
            }
            textBox1.Text = Settings.Default.repoPath;
            //WindowsFormsApplication2.Properties.Settings.Default.Save();
        }

        #endregion Public Constructors

        #region Private Methods

        private void button1_Click(object sender, EventArgs e)
        {
            var repo = new FolderBrowserDialog();
            repo.Description = "Sélectionnez le dossier contenant les promotions";
            var result = repo.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = repo.SelectedPath;
                Settings.Default.repoPath = repo.SelectedPath;
                Settings.Default.Save();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        #endregion Private Methods
    }
}
