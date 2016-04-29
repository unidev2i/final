using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class MaximumCP : Form
    {
        public MaximumCP()
        {
            InitializeComponent();
            foreach (var a in Database.GetListRequest("competence", new[] { "idCompetence" }))
                comboBox1.Items.Add(a);
            //comboBox1.Text = comboBox1.Items[0].ToString();
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericUpDown1.Value = Database.getMaxCP(comboBox1.SelectedItem.ToString());

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
           Database.setMaxCP(comboBox1.Text, numericUpDown1.Value);
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            //
        }

        private void numericUpDown1_Leave(object sender, EventArgs e)
        {
            if(numericUpDown1.Value != null)
                Database.setMaxCP(comboBox1.Text, numericUpDown1.Value);
            else
                Database.setMaxCP(comboBox1.Text, 0);
        }
    }
}
