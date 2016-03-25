using System;
using System.Drawing;
using System.Linq;
using TradeHub.Charts.GDI;
using TradeHub.Core.Model;
using TradeHub.Core.Model.Enums;

namespace TradeHub.Charts.Modules
{
    public class PriceStaticChartModule : StaticChartModule
    {
        /// <summary>
        /// The minimum number of price lines to show on the Y axis of a price chart.
        /// </summary>
        private const int MIN_PRICE_INCREMENTS_NB = 3;

        /// <summary>
        /// The maximum number of price lines to show on the Y axis of a price chart.
        /// </summary>
        private const int MAX_PRICE_INCREMENTS_NB = 10;

        private const int SPACE_BETWEEN_TICK_HIGH_AND_INDICATOR = 8;
        private const int TRADE_HISTORY_INDICATOR_SIZE = 4;

        private const int SPACE_BETWEEN_RIGHT_LEGEND_AND_LABEL = 6;
        
        public PriceStaticChartModule(StaticChart parent)
        {
            this.parent = parent;
            Height = 400;
        }

        /// <summary>
        /// Draws the module inside the ClipBounds of a Graphics object.
        /// </summary>
        public override void Draw(Graphics g)
        {
            spaceBetweenDivX = ROUND_SPACE_BETWEEN_DIV_X ?
                (int)(g.ClipBounds.Width - RIGHT_LEGEND_WIDTH - RIGHT_LEGEND_DASH_LENGTH) / parent.StockData.Count:
                (g.ClipBounds.Width - RIGHT_LEGEND_WIDTH - RIGHT_LEGEND_DASH_LENGTH) / parent.StockData.Count;

            DrawBorder(g);

            DrawYAxis(g);
            DrawXAxis(g);

            DrawData(g);
        }

        public override void DrawXAxis(Graphics g)
        {
            var axisY = g.ClipBounds.Y + Height - BOTTOM_LEGEND_WIDTH - parent.ModulesBorderWidth;
            var axisLeftX = g.ClipBounds.X + parent.ModulesBorderWidth;
            var axisRightX = g.ClipBounds.X + g.ClipBounds.Width - parent.ModulesBorderWidth;
            DrawingHelper.DrawLine(g, Pens.BlueViolet, axisLeftX, axisY, axisRightX, axisY);

            // Draw divisions on the axis
            for (int i = 0; i < parent.StockData.Count; ++i)
            {
                var x = parent.ModulesBorderWidth + i * spaceBetweenDivX + spaceBetweenDivX / 2;
                DrawingHelper.DrawLine(g, Pens.BlueViolet, x, axisY - BOTTOM_LEGEND_DASH_LENGTH / 2, x, axisY + BOTTOM_LEGEND_DASH_LENGTH / 2);
            }
        }

        public override void DrawYAxis(Graphics g)
        {
            var axisX = g.ClipBounds.Width - RIGHT_LEGEND_WIDTH - parent.ModulesBorderWidth;
            var axisTopY = g.ClipBounds.Y + parent.ModulesBorderWidth;
            var axisBottomY = g.ClipBounds.Y + Height - parent.ModulesBorderWidth - BOTTOM_LEGEND_WIDTH;
            DrawingHelper.DrawLine(g, Pens.Purple, axisX, axisTopY, axisX, axisBottomY);

            var max = parent.StockData.Max(s => s.High);
            var min = parent.StockData.Min(s => s.Low);
            var range = max - min;
            var priceSteps = FindPriceScale(range);
            var nbSteps = range / priceSteps;

            // Draw divisions on the axis
            var plotAreaTop = (int)g.ClipBounds.Y + parent.ModulesBorderWidth;
            var plotAreaBottom = (int)g.ClipBounds.Y + Height - BOTTOM_LEGEND_WIDTH - parent.ModulesBorderWidth;
            var currentPrice = Math.Round(max / priceSteps) * priceSteps;
            for (int i = 0; i < nbSteps; ++i)
            {
                // Draw dash
                var y = WorldToScreen(currentPrice, plotAreaTop, plotAreaBottom);
                DrawingHelper.DrawLine(g, Pens.BlueViolet, axisX - RIGHT_LEGEND_DASH_LENGTH, y, axisX + RIGHT_LEGEND_DASH_LENGTH, y);

                // Draw label
                var f = new Font("Arial", 14, FontStyle.Regular, GraphicsUnit.World);
                var label = String.Format("{0:C}", currentPrice);
                var strSize = g.MeasureString(label, f);
                var msgPos = new PointF(axisX + SPACE_BETWEEN_RIGHT_LEGEND_AND_LABEL, y - strSize.Height / 2 + 1);
                DrawingHelper.DrawString(g, label, f, Brushes.Red, msgPos);

                currentPrice -= priceSteps;
            }
        }

        /// <summary>
        /// Draws the stock data as a price chart.
        /// </summary>
        public override void DrawData(Graphics g)
        {
            var plotAreaTop = (int)g.ClipBounds.Y + parent.ModulesBorderWidth;
            var plotAreaBottom = (int)g.ClipBounds.Y + Height - BOTTOM_LEGEND_WIDTH - parent.ModulesBorderWidth;
            for (int i = 0; i < parent.StockData.Count; ++i)
            {
                var tick = parent.StockData[i];

                // Price bar
                var yPosHigh = WorldToScreen(tick.High, plotAreaTop, plotAreaBottom);
                var yPosLow = WorldToScreen(tick.Low, plotAreaTop, plotAreaBottom);
                var yPosClose = WorldToScreen(tick.Close, plotAreaTop, plotAreaBottom);

                var x = parent.ModulesBorderWidth + i * spaceBetweenDivX + spaceBetweenDivX / 2;
                DrawingHelper.DrawLine(g, Pens.Black, x, yPosHigh, x, yPosLow);
                DrawingHelper.DrawLine(g, Pens.Black, x, yPosClose, x + 2, yPosClose);

                if (parent.TransactionHistory != null)
                {
                    DrawTransactionHistory(g, tick, x, yPosHigh);
                }
            }
        }

        private void DrawTransactionHistory(Graphics g, StockTick tick, float x, float yPosHigh)
        {
            // TODO: handle multiple transactions for a single tick (e.g. daily chart with multiple intradat trades, do we want to see all trades or the still open trades?)
            var dailyTrade = parent.TransactionHistory.FirstOrDefault(t => t.Timestamp.Date == tick.Timestamp.Date && t.Symbol == parent.Symbol);
            if (dailyTrade == null)
            {
                return;
            }

            var brush = dailyTrade.Action == TradeOrderAction.Buy ? Brushes.Green : Brushes.DarkRed;

            DrawingHelper.FillEllipse(g, brush, x - 2, yPosHigh - SPACE_BETWEEN_TICK_HIGH_AND_INDICATOR, TRADE_HISTORY_INDICATOR_SIZE, TRADE_HISTORY_INDICATOR_SIZE);
        }

        /// <summary>
        /// Translate a price into a vertical screen coordinate.
        /// </summary>
        /// <param name="price">The price.</param>
        /// <param name="yMin">The top of the drawing area.</param>
        /// <param name="yMax">The bottom of the drawing area.</param>
        /// <returns>Y coordinate to which the given price corresponds.</returns>
        private int WorldToScreen(decimal price, int yMin, int yMax)
        {
            // TODO: can be optimized by storing these values (implement StockDataList)
            var min = parent.StockData.Min(s => s.Low);
            var max = parent.StockData.Max(s => s.High);
            var range = max - min;

            var yProp = 1 - ((price - min) / range);
            var yOffset = yProp * (yMax - yMin);


            return yMin + (int)yOffset;
        }

        /// <summary>
        /// Find the best price increments for the Y axis of this chart.
        /// </summary>
        /// <param name="priceRange">The max price minus the min price that will be shown on the chart.</param>
        private decimal FindPriceScale(decimal priceRange)
        {
            var nbSteps = 0m;
            for (var increment = 0.0001m; increment <= 100000; increment *= 10)
            {
                nbSteps = priceRange / increment;
                if ((nbSteps >= MIN_PRICE_INCREMENTS_NB) && (nbSteps <= MAX_PRICE_INCREMENTS_NB)) return increment;
            }

            for (var increment = 0.00025m; increment <= 250000; increment *= 10)
            {
                nbSteps = priceRange / increment;
                if ((nbSteps >= MIN_PRICE_INCREMENTS_NB) && (nbSteps <= MAX_PRICE_INCREMENTS_NB)) return increment;
            }

            for (var increment = 0.0005m; increment <= 500000; increment *= 10)
            {
                nbSteps = priceRange / increment;
                if ((nbSteps >= MIN_PRICE_INCREMENTS_NB) && (nbSteps <= MAX_PRICE_INCREMENTS_NB)) return increment;
            }

            throw new Exception("No increment found for price axis.");

        }
    }
}
