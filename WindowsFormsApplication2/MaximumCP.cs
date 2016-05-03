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
            foreach (var a in Database.GetListRequest("competence", new[] {"idCompetence"}))
                comboBox1.Items.Add(a);
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
            numericUpDown1.Value = Database.getMaxCP(comboBox1.SelectedItem.ToString());
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            //
        }

        private void numericUpDown1_Leave(object sender, EventArgs e)
        {
            if (numericUpDown1.Value != null)
                Database.setMaxCP(comboBox1.Text, numericUpDown1.Value);
            else
                Database.setMaxCP(comboBox1.Text, 0);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        #endregion Private Methods
    }
}
