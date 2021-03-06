﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication2
{
    public partial class PagePrincipal : Form
    {
        #region Public Fields

        public bool isNameSelected = true;
        public string login;
        public string promotionSelected = "";
        public string idEleveSelected = "";
        #endregion Public Fields

        #region Private Fields

        private readonly BindingSource bindingSource1 = new BindingSource();
        private readonly AssistantConnexion form1;
        private DataGridDebug dataForm;
        private AjoutEleve graphic;
        private Inscription graphic2;
        private Suppresion_User graphic3;
        private ChangerLogin graphic4;
        private ChangerMdp graphic5;
        private DelEleve graphic7;

        private Size a;
        private Size b;
        private Size c;
        private Size d;

        private Point ef;
        private Point f;
        private Point g;
        private Point h;
        private Point i;
        private Point j;

        #endregion Private Fields

        #region Public Constructors

        public PagePrincipal()
        {
            InitializeComponent();
            dataGridView1.AutoResizeColumns();
        }

        public PagePrincipal(AssistantConnexion form1, string login, bool statut)
        {
            InitializeComponent();
            this.form1 = form1;
            if (statut == false)
            {
                aToolStripMenuItem.Visible = false;
            }

            HelloBox(login);
        }

        #endregion Public Constructors

        #region Public Methods

        public void HelloBox(string nom)
        {
            label4.Text = "Professeur connecté: " + nom;
            login = nom;
        }

        public void Majlog(string newlog)
        {
            login = newlog;
            HelloBox(login);
        }

        public void UpdateLogin(string login)
        {
            this.login = login;
            label4.Text = "Professeur connecté: " + login;
        }

        #endregion Public Methods

        #region Private Methods

        private void ajouterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            graphic2 = new Inscription();
            graphic2.ShowDialog();
        }


        private void ajouterToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //BOUTON POUR AJOUTER UN ELEVE
            graphic = new AjoutEleve();
            graphic.ShowDialog();
            comboBox1.Items.Clear();
            foreach (var a in Database.GetListRequest("eleve", new[] {"Prenom", "Nom"}))
                comboBox1.Items.Add(a);
        }

        public void refreshCombo()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();

            chart1.Hide();
            chart2.Hide();
            chart3.Hide();
            label2.Hide();
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";
            try
            {               
                //this.Refresh();
                foreach (var a in Database.GetListRequest("eleve", new[] { "Prenom", "Nom" }))
                    comboBox1.Items.Add(a);

                foreach (var a in Database.GetListRequest("classe", new[] { "Promotion" }))
                    comboBox3.Items.Add(a);
            }
            catch
            {
                //VIDE
            }
        }

        private void ajouterUnPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var b = new Thread(ImportTp.Go);
            b.Start();
            int test = 1;
            while(b.IsAlive && test < 5000)
            {
                Console.WriteLine(test);
                test++;
                Cursor = Cursors.WaitCursor;
            }
            Thread.Sleep(5000);
            refreshCombo();
            this.Invalidate();
            this.Refresh();
            comboBox1.Invalidate();
            comboBox2.Invalidate();
            comboBox3.Invalidate();
            Cursor = Cursors.Default;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var MaxCPForm = new MaximumCP();
                MaxCPForm.ShowDialog();
                var x = new List<Tuple<string, float>>();
                var y = Database.GetWebMax(Database.GetPromotionFromEleve(idEleveSelected));
                if (isNameSelected)
                    x = Database.GetWebRequest(checkBox1, idEleveSelected);
                if (!isNameSelected)
                    x = Database.GetWebClasseRequest(promotionSelected);
                drawWeb(x, 0);
                drawWeb(y, 1);
            }
            catch
            {
                return;
            }
        }

        private void changerDeLoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graphic4 = new ChangerLogin(login, this);
            graphic4.ShowDialog();
        }

        private void changerDeMotDePasseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graphic5 = new ChangerMdp(login);
            graphic5.ShowDialog();
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            //new Form1(chart1, 0).ShowDialog();
        }

        private void chart2_Click(object sender, EventArgs e)
        {
            new ZoomGraph(chart2, 0).ShowDialog();
        }

        private void chart3_Click(object sender, EventArgs e)
        {
            new ZoomGraph(chart3, 1).ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CHANGEMENT D'ELEVE, FAIRE LES MODIFICATIONS DES GRAPHIQUES ...
            var str = comboBox1.Text;
            var result = Regex.Split(str, " ");
            var prenom = result[0];
            var nom = result[1];
            var idClasse = "";
            var classe = new List<string>();

            dataGridView1.DataSource = bindingSource1;

            foreach (
                var a in
                    Database.GetListRequest("eleve", new[] { "idClasse" }, "Nom='" + nom + "' and Prenom='" + prenom + "'")
                )
                idClasse = a;
            comboBox3.Text = Database.Getpromo(idClasse);

            comboBox1.Select(50, 50);

            var str2 = Regex.Split(comboBox1.Text, " ");
            var idEleve = "";
            idEleve = Database.GetIdEleveFromName(str2[1], str2[0]);
            idEleve = idEleve.Substring(0, idEleve.Length - 1);
            idEleveSelected = idEleve;

            foreach (var a in Database.GetListRequest("eleve", new[] { "idEleve" }, "Nom='" + str2[1] + "' and Prenom='" + str2[0] + "'"))
                idEleve = a.Substring(0, a.Length - 1) ?? "1";

            GetData(
                "SELECT Prenom, Nom, idTp AS TP, date AS Date, idCompetence AS Competence, Note, maxNote AS 'Note Maximum' FROM eleve NATURAL JOIN tp NATURAL JOIN note WHERE idEleve='" +
                idEleve + "'");
            dataGridView1.AutoResizeColumns();
            label2.Show();
            label2.Text = "Vous observez les résultats de " + comboBox1.Text;

            isNameSelected = true;

            promotionSelected = comboBox3.Text;

            button1.Visible = true;

            // Draw graphics

            var w = Database.GetWtfRequest(idEleve);
            var z = Database.GetWebRequest(checkBox1, idEleve);
            var y = Database.GetWebMax(Program.ac.graphic.promotionSelected);
            var x = Database.GetCourbeRequest(idEleve, comboBox2.Text); //ADD_PLS

            drawGraph(w); //ADD_PLS
            drawWeb(z, 0);
            drawWeb(y, 1);
            drawCourbe(x);
            chart1.Visible = true;
            chart2.Visible = true;
            chart3.Visible = true;


            //Chargement ComboBox réalisées par l'élève
            comboBox2.Items.Clear();
            string reqIdEleve = "idEleve ='" + idEleveSelected + "' ORDER BY idCompetence"; 
            foreach (var a in Database.GetDistinctRequest("note NATURAL JOIN tp", "idCompetence", new[] { "idCompetence" }, reqIdEleve))
                comboBox2.Items.Add(a);

            if (comboBox2.Text == "")
                comboBox2.Text = comboBox2.Items[0].ToString();

        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //REFAIRE PAREIL QUE POUR LE NIVEAU
            var i = 0;
            var promo = comboBox3.Text;
            var eleve = new string[1000];
            label2.Show();
            label2.Text = "Vous observez les résultats de la promotion " + comboBox3.Text;
            isNameSelected = false;
            promotionSelected = comboBox3.Text;
            button1.Visible = true;
            try
            {
                eleve = Database.RecupEleveAvecPromo(promo);
                comboBox1.Items.Clear();
                if (eleve != null)
                {
                    for (i = 0; (i < eleve.Length) && (eleve[i] != null); i++)
                    {
                        //MessageBox.Show(eleve[i]);
                        comboBox1.Items.Add(eleve[i]);
                    }
                }
                var w = Database.GetWthRequest(promo);
                var z = Database.GetWebClasseRequest(promo);
                var y = Database.GetWebMax(Program.ac.graphic.promotionSelected);
                var x = Database.GetCourbeClasseRequest(comboBox2.Text, comboBox3.Text);

                drawGraph(w);
                drawWeb(z, 0);
                drawWeb(y, 1);
                drawCourbe(x);
                chart1.Visible = true;
                chart2.Visible = true;
                chart3.Visible = true;
            }
            catch
            {
                comboBox3.Items.Clear();
                foreach (var a in Database.GetListRequest("classe", new[] { "Promotion" }))
                    comboBox3.Items.Add(a);
            }

            //Changement en fonction de la promo
            comboBox2.Items.Clear();
            var getidpromo = Database.GetidClasse(promotionSelected);
            string reqIdClasse = "idClasse ='" + getidpromo + "' ORDER BY idCompetence"; 
            foreach (var a in Database.GetDistinctRequest("note NATURAL JOIN tp NATURAL JOIN eleve", "idCompetence", new[] { "idCompetence" }, reqIdClasse))
                comboBox2.Items.Add(a);

            if (comboBox2.Text == "")
                comboBox2.Text = comboBox2.Items[0].ToString();

        }

        private void comboBox3_TextUpdate(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            foreach (var a in Database.GetListRequest("classe", new[] {"numClasse"}))
                comboBox3.Items.Add(a);
            comboBox3.Select(50, 50);
        }

        private void dataGridDebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataForm = new DataGridDebug();
            dataForm.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void deconnexionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sgInfo = new ProcessStartInfo("PPM.exe");
            Process.Start(sgInfo);
            Close();
        }

        private void drawGraph(List<Tuple<string, float, int>> tuples)
        {
            var array = tuples.Select(a => a.Item1).ToList();
            var parray = tuples.Select(a => a.Item2).ToList();
            var xarray = tuples.Select(a => a.Item3).ToList();
            chart2.Show();
            //chart1.Palette = ChartColorPalette.Excel;
            /*chart1.Series.Clear();

            for (var a = 0; a != array.Count; a++)
            {
                chart1.Series.Add(new Series(array[a]));
                chart1.Series[a]["PointWidth"] = "1";
            }*/

            var i = 0;
            foreach (var tSeries in from tSeries in chart1.Series let a = tSeries select tSeries)
            {
                tSeries.Points.AddY(parray[i]);
                i++;
            }

            chart2.Series[0].Points.Clear();

            foreach (var a in tuples)
            {
                chart2.Series[0].Points.AddXY(a.Item1 + Environment.NewLine + a.Item3, a.Item3);
            }
        }


        private void drawCourbe(IOrderedEnumerable<Tuple<float, DateTime>> aTuples)
        {
            chart1.Show();
            chart1.Series[0].Points.Clear();
            chart1.Series[0].XValueType = ChartValueType.DateTime;
            chart1.Series[0].Name = comboBox2.Text;
            foreach (var a in aTuples)
            {
                var p = chart1.Series[0].Points.Add(a.Item1);
                p.Name = a.Item1.ToString();
                p.AxisLabel = a.Item2.ToString();
                //p.Label = a.Item1;
            }

        }

        private void drawWeb(List<Tuple<string, float>> aTuples, int serie)
        {
            var str = comboBox1.Text;
            var result = Regex.Split(str, " ");
            var prenom = result[0];
            var nom = result[1];
            chart3.Show();
            chart3.Series[1]["RadarDrawingStyle"] = "Line";
            chart3.Series[serie].Points.Clear();
            Database.removeCPFromWeb(aTuples, prenom, nom);
            foreach (var a in aTuples)
            {
                var p = chart3.Series[serie].Points.Add(a.Item2);
                p.Name = a.Item1;
                p.AxisLabel = a.Item1;
                //p.Label = a.Item1;
            }
        }

        private void exporterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Database.BackupDatabase();
        }

        private void Form3_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            form1.Close();
        }

        private void Form3_Load_1(object sender, EventArgs e)
        {
            dataGridView1.DataSource = bindingSource1;
            dataGridView1.RowHeadersVisible = false;
            label2.Text = "";

            foreach (var a in Database.GetListRequest("eleve", new[] {"Prenom", "Nom"}))
                comboBox1.Items.Add(a);
            foreach (var a in Database.GetListRequest("classe", new[] {"Promotion"}))
                comboBox3.Items.Add(a);
            //foreach (var a in Database.GetDistinctRequest("note", "idCompetence", new[] { "idCompetence" }))
            //   comboBox2.Items.Add(a);

        }

        private void GetData(string selectCommand)
        {
            var connectionString = "Server=" + Database.Server + ";Uid=" + Database.Username + ";Database=" +
                                   Database.DatabaseName + ";Password=" + Database.Password + ";";
            var connection = new MySqlConnection(connectionString);
            try
            {
                var dataAdapter = new MySqlDataAdapter(selectCommand, connectionString);
                var commandBuilder = new MySqlCommandBuilder(dataAdapter);

                var table = new DataTable();
                table.Locale = CultureInfo.InvariantCulture;
                dataAdapter.Fill(table);
                bindingSource1.DataSource = table;

                dataGridView1.AutoResizeColumns(
                    DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        private void importerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Database.RestoreDatabase() == 0)
            {
                var sgInfo = new ProcessStartInfo("WindowsFormsApplication2.exe");
                Process.Start(sgInfo);
                Close();
            }
        }

        private void importerTPToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void modifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void sélectionnerLeDossierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form OptionTP = new OptionTP();
            OptionTP.ShowDialog();
        }

        private void supprimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graphic3 = new Suppresion_User();
            graphic3.ShowDialog();
        }

        private void supprimerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            graphic7 = new DelEleve();
            graphic7.Show();
        }

        private void unEleveToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isNameSelected)
            {
                var x = Database.GetCourbeRequest(idEleveSelected, comboBox2.Text);
                drawCourbe(x);
                chart1.Visible = true;
            }
            else
            {
                var x = Database.GetCourbeClasseRequest(comboBox2.Text, promotionSelected);
                drawCourbe(x);
                chart1.Visible = true;
            }
        }

        #endregion Private Methods

        private void PagePrincipal_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                ef = dataGridView1.Location;
                f = chart2.Location;
                g= checkBox1.Location;
                h= chart3.Location;
                i= label2.Location;
                j= button1.Location;
                a = panel1.Size;
                b = chart1.Size;
                c = chart2.Size;
                d = chart3.Size;
                dataGridView1.Location = new Point(12, Screen.PrimaryScreen.Bounds.Height-390);
                panel1.Size = new Size(307,dataGridView1.Location.Y-10-42);
                chart1.Size = new Size((Screen.PrimaryScreen.Bounds.Width-307)/2,((Screen.PrimaryScreen.Bounds.Height-42)/2)-30);
                chart2.Size = new Size(Screen.PrimaryScreen.Bounds.Width-307-chart1.Size.Width-50, chart2.Size.Height+70);
                chart2.Location = new Point(panel1.Size.Width+chart1.Size.Width+50, chart2.Location.Y);
                checkBox1.Location = new Point(Screen.PrimaryScreen.Bounds.Width/2, Screen.PrimaryScreen.Bounds.Height - 100);
                chart3.Location = new Point(dataGridView1.Location.X+dataGridView1.Size.Width+30,chart1.Size.Height+40);
                chart3.Size = new Size(Screen.PrimaryScreen.Bounds.Width - dataGridView1.Size.Width - 30, (Screen.PrimaryScreen.Bounds.Height/2)-50);
                label2.Location = new Point(Screen.PrimaryScreen.Bounds.Width/2,50);
                panel1.Controls.Add(button1);
                button1.Location = new Point(label4.Location.X+50,label4.Location.Y+30);
            }
            else
            {
                dataGridView1.Location=ef;
                chart2.Location=f;
                checkBox1.Location=g;
                chart3.Location=h;
                label2.Location=i;
                button1.Location=j;
                panel1.Size=a;
                chart1.Size=b;
                chart2.Size=c;
                chart3.Size=d;
                panel1.Controls.Remove(button1);
                PagePrincipal.ActiveForm.Controls.Add(button1);
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("COUCOU");
        }
    }
}
