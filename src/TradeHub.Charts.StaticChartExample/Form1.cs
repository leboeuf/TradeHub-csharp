using System;
using System.Drawing;
using System.Windows.Forms;
using TradeHub.Charts.Overlays;
using TradeHub.Core.DataProviders;
using TradeHub.Core.Model;

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
            var stockTicks = await YahooHistoricalDataProvider.DownloadHistoricalData("COST", DateTime.Today.AddDays(-202), DateTime.Now);
            var tickList = new TickList(stockTicks);

            _chart = new StaticChart
            {
                Width = 640,
                TickData = tickList,
                ChartStyleOptions = new ChartStyleOptions
                {
                    BackgroundColor = Color.FromArgb(25, 22, 52),
                    ModulesBorderColor = new Pen(Color.FromArgb(48, 48, 48)),
                    AxisColor = Pens.Gray,
                    DataColor = Pens.Silver,
                    YAxisLabelsColor = Brushes.Silver
                }
            };
            _chart.Modules[0].Overlays.Add(new LinearRegressionChartOverlay(_chart.Modules[0])
            {
                Color = Color.DarkGreen,
                Thickness = 2,
                //FirstValueIndex = stockData.Count - 101,
                //LastValueIndex = stockData.Count - 1,
                FromIndex = 0,
                ToIndex = 140,
                ExtendLine = true
            });

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
