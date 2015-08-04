using System;
using System.Drawing;
using System.Windows.Forms;

namespace TradeHub.Charts.StaticChartExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var chart = new StaticChart();
            chart.Width = 640;
            chart.BackgroundColor = Brushes.Beige;

            this.BackgroundImage = chart.Draw();
            this.Width = chart.Width;
            this.Height = chart.Height;
        }
    }
}
