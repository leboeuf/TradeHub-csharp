﻿using System;
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
                (int)(g.ClipBounds.Width - RIGHT_LEGEND_WIDTH - RIGHT_LEGEND_DASH_LENGTH) / parent.TickData.Ticks.Count():
                (g.ClipBounds.Width - RIGHT_LEGEND_WIDTH - RIGHT_LEGEND_DASH_LENGTH) / parent.TickData.Ticks.Count();

            DrawBorder(g);

            DrawYAxis(g);
            DrawXAxis(g);

            DrawData(g);

            DrawOverlays(g);
        }

        public override void DrawXAxis(Graphics g)
        {
            var axisY = g.ClipBounds.Y + Height - BOTTOM_LEGEND_WIDTH - parent.ChartStyleOptions.ModulesBorderWidth;
            var axisLeftX = g.ClipBounds.X + parent.ChartStyleOptions.ModulesBorderWidth;
            var axisRightX = g.ClipBounds.X + g.ClipBounds.Width - parent.ChartStyleOptions.ModulesBorderWidth;
            DrawingHelper.DrawLine(g, parent.ChartStyleOptions.AxisColor, axisLeftX, axisY, axisRightX, axisY);

            // Draw divisions on the axis
            for (int i = 0; i < parent.TickData.Ticks.Count(); ++i)
            {
                var x = parent.ChartStyleOptions.ModulesBorderWidth + i * spaceBetweenDivX + spaceBetweenDivX / 2;
                DrawingHelper.DrawLine(g, parent.ChartStyleOptions.AxisColor, x, axisY - BOTTOM_LEGEND_DASH_LENGTH / 2, x, axisY + BOTTOM_LEGEND_DASH_LENGTH / 2);
            }
        }

        public override void DrawYAxis(Graphics g)
        {
            var font = new Font(parent.ChartStyleOptions.LabelsFont, parent.ChartStyleOptions.LabelsFontSize, FontStyle.Regular, GraphicsUnit.World);
            var modulesBorderWidth = parent.ChartStyleOptions.ModulesBorderWidth;

            var axisX = g.ClipBounds.Width - RIGHT_LEGEND_WIDTH - modulesBorderWidth;
            var axisTopY = g.ClipBounds.Y + modulesBorderWidth;
            var axisBottomY = g.ClipBounds.Y + Height - modulesBorderWidth - BOTTOM_LEGEND_WIDTH;
            DrawingHelper.DrawLine(g, parent.ChartStyleOptions.AxisColor, axisX, axisTopY, axisX, axisBottomY);

            var max = parent.TickData.Ticks.Max(s => s.High);
            var min = parent.TickData.Ticks.Min(s => s.Low);
            var range = max - min;
            var priceSteps = FindPriceScale(range);
            var nbSteps = range / priceSteps;

            // Draw divisions on the axis
            var plotAreaTop = (int)g.ClipBounds.Y + modulesBorderWidth;
            var plotAreaBottom = (int)g.ClipBounds.Y + Height - BOTTOM_LEGEND_WIDTH - modulesBorderWidth;
            var currentPrice = Math.Round(max / priceSteps) * priceSteps;
            for (int i = 0; i < nbSteps; ++i)
            {
                // Draw dash
                var y = WorldToScreen( parent.TickData, currentPrice, plotAreaTop, plotAreaBottom);
                DrawingHelper.DrawLine(g, parent.ChartStyleOptions.AxisColor, axisX - RIGHT_LEGEND_DASH_LENGTH, y, axisX + RIGHT_LEGEND_DASH_LENGTH, y);

                // Draw label
                var label = string.Format("{0:C}", currentPrice);
                var strSize = g.MeasureString(label, font);
                var msgPos = new PointF(axisX + SPACE_BETWEEN_RIGHT_LEGEND_AND_LABEL, y - strSize.Height / 2 + 1);
                DrawingHelper.DrawString(g, label, font, parent.ChartStyleOptions.YAxisLabelsColor, msgPos);

                currentPrice -= priceSteps;
            }
        }

        /// <summary>
        /// Draws the stock data as a price chart.
        /// </summary>
        public override void DrawData(Graphics g)
        {
            var modulesBorderWidth = parent.ChartStyleOptions.ModulesBorderWidth;

            var plotAreaTop = (int)g.ClipBounds.Y + modulesBorderWidth;
            var plotAreaBottom = (int)g.ClipBounds.Y + Height - BOTTOM_LEGEND_WIDTH - modulesBorderWidth;
            for (int i = 0; i < parent.TickData.Ticks.Count(); ++i)
            {
                var tick = parent.TickData.Ticks.ElementAt(i);

                // Price bar
                var yPosHigh = WorldToScreen(parent.TickData, tick.High, plotAreaTop, plotAreaBottom);
                var yPosLow = WorldToScreen(parent.TickData, tick.Low, plotAreaTop, plotAreaBottom);
                var yPosClose = WorldToScreen(parent.TickData, tick.Close, plotAreaTop, plotAreaBottom);

                var x = modulesBorderWidth + i * spaceBetweenDivX + spaceBetweenDivX / 2;
                DrawingHelper.DrawLine(g, parent.ChartStyleOptions.DataColor, x, yPosHigh, x, yPosLow);
                DrawingHelper.DrawLine(g, parent.ChartStyleOptions.DataColor, x, yPosClose, x + 2, yPosClose);

                if (parent.TransactionHistory != null)
                {
                    DrawTransactionHistory(g, tick, x, yPosHigh);
                }
            }
        }

        public override void DrawOverlays(Graphics g)
        {
            foreach (var overlay in Overlays)
            {
                overlay.Draw(g);
            }
        }

        private void DrawTransactionHistory(Graphics g, Tick tick, float x, float yPosHigh)
        {
            // TODO: handle multiple transactions for a single tick (e.g. daily chart with multiple intraday trades, do we want to see all trades or the still open trades?)
            var dailyTrade = parent.TransactionHistory.FirstOrDefault(t => t.Timestamp.Date == tick.Timestamp.Date && t.Symbol == parent.Symbol);
            if (dailyTrade == null)
            {
                return;
            }

            var brush = dailyTrade.Action == TradeOrderAction.Buy ? Brushes.Green : Brushes.DarkRed;

            DrawingHelper.FillEllipse(g, brush, x - 2, yPosHigh - SPACE_BETWEEN_TICK_HIGH_AND_INDICATOR, TRADE_HISTORY_INDICATOR_SIZE, TRADE_HISTORY_INDICATOR_SIZE);
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
