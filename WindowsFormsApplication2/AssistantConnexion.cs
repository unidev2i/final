﻿using System;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class AssistantConnexion : Form
    {
        private Aide apropos;
        public PagePrincipal graphic;
        private Inscription graphic2;

        public AssistantConnexion()
        {
            InitializeComponent();
            verrouiller();
        }

        private void deverrouiller()
        {
            pictureBox2.Hide();
            pictureBox4.Show();
            textBox2.PasswordChar = (char) 0;
        }

        private void verrouiller()
        {
            pictureBox2.Show();
            pictureBox4.Hide();
            textBox2.PasswordChar = '*';
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            verrouiller();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            var login = textBox1.Text;
            var mdp = textBox2.Text;

            // lr -> login result
            var lr = Database.Login(login, mdp);

            if ((lr != 0) && (lr != 1))
                return;

            graphic = new PagePrincipal(this, textBox1.Text, lr == 1);
            graphic.Show();
            Hide();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            deverrouiller();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            apropos = new Aide();
            apropos.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Bouton s'inscrire appuyer
            graphic2 = new Inscription(this);
            graphic2.Show();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        private void AssistantConnexion_Load(object sender, EventArgs e)
        {

        }
    }
}