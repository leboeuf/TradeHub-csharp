﻿using System;
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
            DoubleBuffered = true;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            var stockData = await YahooHistoricalDataProvider.DownloadHistoricalData("MSFT", new DateTime(2020, 01, 02), DateTime.Now);

            _chart = new StaticChart
            {
                Width = 640,
                BackgroundColor = Color.Beige,
                StockData = stockData
            };

            BackgroundImage = _chart.Draw();
            ClientSize = new Size(_chart.Width, _chart.Height);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (_chart == null || ClientSize.Width <= 0 || ClientSize.Height <= 0)
            {
                return;
            }

            // Redraw chart on resize
            _chart.Width = ClientSize.Width;
            BackgroundImage = _chart.Draw();
        }
    }
}
