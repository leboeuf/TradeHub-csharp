using System;
using System.Drawing;
using System.Windows.Forms;
using TradeHub.Core.DataProviders;

namespace TradeHub.Charts.StaticChartExample
{
    public partial class Form1 : Form
    {
        private StaticChart _chart;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            var stockData = await YahooHistoricalDataProvider.DownloadHistoricalData("YHOO", new DateTime(2015, 01, 02), DateTime.Now);

            _chart = new StaticChart
            {
                Width = 640,
                BackgroundColor = Color.Beige,
                StockData = stockData
            };

            this.BackgroundImage = _chart.Draw();
            this.ClientSize = new Size(_chart.Width, _chart.Height);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.ClientSize.Width <= 0 || this.ClientSize.Height <= 0)
            {
                return;
            }

            // Redraw chart on resize
            _chart.Width = this.ClientSize.Width;
            this.BackgroundImage = _chart.Draw();
        }
    }
}
