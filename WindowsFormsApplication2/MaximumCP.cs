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
            textBox1.Text = Database.getMaxCP(comboBox1.SelectedItem.ToString());
            Database.setMaxCP(comboBox1.Text, textBox1.Text);
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
            Database.setMaxCP(comboBox1.Text, textBox1.Text);
        }
    }
}
