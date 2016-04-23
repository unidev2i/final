using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        #region Public Constructors

        public Form1(Chart a, int type)
        {
            InitializeComponent();
            if (type == 0)
            {
                chart1.Series[0].Points.Clear();
                chart1.Series[0].ChartType = a.Series[0].ChartType;

                foreach (var b in a.Series[0].Points)
                {
                    chart1.Series[0].Points.Add(b);
                }
            }
            if(type==1)
            {
                chart1.Series[0].Points.Clear();
                chart1.Series[0].ChartType = a.Series[0].ChartType;

                foreach (var b in a.Series[0].Points)
                {
                    chart1.Series[0].Points.Add(b);
                }

                chart1.Series.Add("Series2");
                chart1.Series[1]["BorderWidth"] = "2";
                chart1.Series[1]["RadarDrawingStyle"] = "Line";
                chart1.Series[1]["Color"] = "Red";
                chart1.Series[1].Points.Clear();
                chart1.Series[1].ChartType = a.Series[1].ChartType;
                foreach (var c in a.Series[1].Points)
                {
                    chart1.Series[1].Points.Add(c);
                }     
            }
        }

        #endregion Public Constructors

        #region Private Methods

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        #endregion Private Methods

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }
}