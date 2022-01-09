using System;
using System.Drawing;
using System.Linq;
using TradeHub.Charts.GDI;
using TradeHub.Charts.Interfaces;
using TradeHub.Charts.Modules;
using TradeHub.Core.Math;

namespace TradeHub.Charts.Overlays
{
    /// <summary>
    /// Draws a linear regression line.
    /// </summary>
    public class LinearRegressionChartOverlay : IChartOverlay
    {
        /// <summary>
        /// The module hosting this overlay.
        /// </summary>
        private StaticChartModule _parentModule;

        /// <summary>
        /// The intercept of the regression line (the value of Y at index 0).
        /// </summary>
        private decimal _intercept;

        /// <summary>
        /// The slope the regression line.
        /// </summary>
        private decimal _slope;

        /// <summary>
        /// The color of the regression line.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// The thickness of the regression line.
        /// </summary>
        public float Thickness { get; set; }

        /// <summary>
        /// Whether to extend the line to the right past the last data point.
        /// </summary>
        public bool ExtendLine { get; set; }

        /// <summary>
        /// The index of the first data point to use.
        /// </summary>
        public int FromIndex { get; set; }

        /// <summary>
        /// The index of the last data point to use.
        /// </summary>
        public int ToIndex { get; set; }

        public LinearRegressionChartOverlay(StaticChartModule parentModule)
        {
            _parentModule = parentModule;
        }

        public void Draw(Graphics g)
        {
            if (_intercept == 0 && _slope == 0)
            {
                ComputeRegressionLine();
            }

            var pen = new Pen(Color, Thickness);

            var chartStyleOptions = _parentModule.parent.ChartStyleOptions;

            var plotAreaTop = (int)g.ClipBounds.Y + chartStyleOptions.ModulesBorderWidth;
            var plotAreaBottom = (int)g.ClipBounds.Y + _parentModule.Height - StaticChartModule.BOTTOM_LEGEND_WIDTH - chartStyleOptions.ModulesBorderWidth;

            var valuesCount = ToIndex - FromIndex + 1;

            var x1 = chartStyleOptions.ModulesBorderWidth + FromIndex * _parentModule.spaceBetweenDivX + _parentModule.spaceBetweenDivX / 2;
            var x2 = chartStyleOptions.ModulesBorderWidth + ToIndex * _parentModule.spaceBetweenDivX + _parentModule.spaceBetweenDivX / 2;
            var y1 = _parentModule.WorldToScreen(_parentModule.parent.TickData, _intercept, plotAreaTop, plotAreaBottom);
            var y2 = _parentModule.WorldToScreen(_parentModule.parent.TickData, _intercept + _slope * valuesCount, plotAreaTop, plotAreaBottom);
            DrawingHelper.DrawLine(g, pen, x1, y1, x2, y2); // TODO: handle ExtendLine
        }

        private void ComputeRegressionLine()
        {
            if (FromIndex > ToIndex)
            {
                throw new InvalidOperationException($"Property '{nameof(ToIndex)}' must be greater than property '{nameof(FromIndex)}'.");
            }

            var maxIndex = _parentModule.parent.TickData.Ticks.Count() - 1;
            if (FromIndex > maxIndex || ToIndex > maxIndex)
            {
                throw new InvalidOperationException($"Properties '{nameof(FromIndex)}' and '{nameof(ToIndex)}' must be less than or equal the number of ticks.");
            }

            var indexedTicks = _parentModule.parent.TickData.Ticks.Skip(FromIndex).Take(ToIndex - FromIndex).Select((tick, index) => new { index, tick.Close });
            var xValues = indexedTicks.Select(x => (decimal)x.index).ToArray();
            var yValues = indexedTicks.Select(x => x.Close).ToArray();

            MathHelper.LinearRegression(xValues, yValues, 0, xValues.Length, out _intercept, out _slope);
        }
    }
}
