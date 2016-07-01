using System;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class MaximumCP : Form
    {
        #region Public Constructors

        public MaximumCP()
        {
            InitializeComponent();
           /* foreach (var a in Database.GetListRequest("competence", new[] { "idCompetence" }))
                comboBox1.Items.Add(a); */
            try
            {
                comboBox1.Items.Clear();
                var getidpromo = Database.GetidClasse(Program.ac.graphic.promotionSelected);
                string reqIdClasse = "idClasse ='" + getidpromo + "' ORDER BY idCompetence";
                foreach (var a in Database.GetDistinctRequest("competence", "idCompetence", new[] { "idCompetence" }, reqIdClasse))
                    comboBox1.Items.Add(a.Substring(0,a.Length - 1));

                if (comboBox1.Text == "")
                    comboBox1.Text = comboBox1.Items[0].ToString();
            }
            catch
            {
                MessageBox.Show("Problème lors de la détection du Maximum des notes");
                this.Close();
            }
            //comboBox1.Text = comboBox1.Items[0].ToString();
        }

        #endregion Public Constructors

        #region Private Methods

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Database.setMaxCP(comboBox1.Text, numericUpDown1.Value);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDown1.Value = Database.GetMaxCp(comboBox1.SelectedItem.ToString(),Program.ac.graphic.promotionSelected);
        }

        private void numericUpDown1_Leave(object sender, EventArgs e)
        {
            if (numericUpDown1.Value != null)
                Database.setMaxCP(comboBox1.Text, numericUpDown1.Value);
            else
                Database.setMaxCP(comboBox1.Text, 0);
        }

        #endregion Private Methods

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value != null)
                Database.setMaxCP(comboBox1.Text, numericUpDown1.Value);
            else
                Database.setMaxCP(comboBox1.Text, 0);
        }
    }
}
