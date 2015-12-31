using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TradeHub.Charts.GDI;
using TradeHub.Charts.Interfaces;
using TradeHub.Charts.Modules;
using TradeHub.Core.Model;

namespace TradeHub.Charts
{
    /// <summary>
    /// Generates static drawings of financial charts. 
    /// </summary>
    public class StaticChart
    {
        private const int DEFAULT_WIDTH = 640;
        private const int SPACE_BETWEEN_CHART_MODULES = 2;

        /// <summary>
        /// The Bitmap object to draw to.
        /// </summary>
        private Bitmap bitmap { get; set; }

        /// <summary>
        /// The stock data to plot on the chart.
        /// </summary>
        public List<StockTick> StockData { get; set; }

        #region Dimensions
        /// <summary>
        /// The desired width of the chart.
        /// </summary>
        public int Width = DEFAULT_WIDTH;

        /// <summary>
        /// The calculated height of the chart. This can't be set manually because the height depends on the components on the chart.
        /// </summary>
        public int Height
        {
            // The "- SPACE_BETWEEN_CHART_MODULES" is because the first module doesn't have a top margin.
            get { return Modules.Sum(module => (module.Height + SPACE_BETWEEN_CHART_MODULES)) - SPACE_BETWEEN_CHART_MODULES; }
        }
        #endregion

        #region Background

        /// <summary>
        /// The background color of the chart.
        /// </summary>
        public Color BackgroundColor = Color.Transparent;
        #endregion

        #region Border
        /// <summary>
        /// The border color surrounding each module. Set to null for no border.
        /// </summary>
        public Pen ModulesBorderColor = Pens.Black;

        /// <summary>
        /// The border width for each module border.
        /// </summary>
        public int ModulesBorderWidth = 1;
        #endregion

        #region Modules
        /// <summary>
        /// List of the chart modules (components) composing this chart.
        /// </summary>
        public List<IStaticChartModule> Modules { get; set; }
        #endregion

        public StaticChart()
        {
            Modules = new List<IStaticChartModule>
            {
                new PriceStaticChartModule(this),
                new VolumeStaticChartModule(this)
            };
        }

        /// <summary>
        /// Draws the chart to a Bitmap.
        /// </summary>
        public Bitmap Draw()
        {
            bitmap = new Bitmap(Width, Height);
            DrawingHelper.BitmapToDebug = bitmap;
            var g = Graphics.FromImage(bitmap);
            g.SetClip(new Rectangle(0, 0, Width, Height));
            g.Clear(BackgroundColor);

            // Keep track of the height of modules to set each one's drawing area.
            var drawingArea = new Rectangle();
            foreach (var module in Modules)
            {
                drawingArea = GetModuleDrawingArea(module.Height, drawingArea);
                g.SetClip(drawingArea);
                module.Draw(g);
            }

            return bitmap;
        }

        /// <summary>
        /// Calculates the rectangle to which the drawing surface will be clipped.
        /// </summary>
        /// <param name="moduleHeight">The height of the module that will be drawn.</param>
        /// <param name="previousDrawingArea">Rectangle of the previously drawn area. Used to know the position of the next module.</param>
        /// <returns>The position and dimensions where the module will be drawn.</returns>
        /// <remarks>The previousDrawingArea Rectangle is used in order to not create a new Rectangle object for each module every time the chart is drawn.</remarks>
        private Rectangle GetModuleDrawingArea(int moduleHeight, Rectangle previousDrawingArea)
        {
            var topMargin = previousDrawingArea.Bottom != 0 ? SPACE_BETWEEN_CHART_MODULES : 0; // Add top margin except for the first module

            var previousModuleBottom = previousDrawingArea.Bottom;
            previousDrawingArea.X = 0;
            previousDrawingArea.Y = previousModuleBottom + topMargin;
            previousDrawingArea.Height = moduleHeight;
            previousDrawingArea.Width = Width;
            return previousDrawingArea;
        }
    }
}
