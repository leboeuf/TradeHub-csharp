using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TradeHub.Charts
{
    /// <summary>
    /// Generates static drawings of financial charts. 
    /// </summary>
    public class StaticChart
    {
        private const int DEFAULT_WIDTH = 640;

        /// <summary>
        /// The Bitmap object to draw to.
        /// </summary>
        private Bitmap bitmap { get; set; }

        #region Dimensions
        /// <summary>
        /// The desired width of the chart.
        /// </summary>
        public int Width = DEFAULT_WIDTH;

        /// <summary>
        /// The calculated height of the chart. This can't be set manually because the height depends on the components on the chart.
        /// </summary>
        public int Height {
            get { return Modules.Sum(module => module.Height); }
            private set {  }
        }
        #endregion

        #region Background
        /// <summary>
        /// The background color of the chart. Set to null for transparent.
        /// </summary>
        public Brush BackgroundColor { get; set; }
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
        public List<StaticChartModule> Modules { get; set; }
        #endregion

        public StaticChart()
        {
            Modules = new List<StaticChartModule>
            {
                new StaticChartModule(this)
            };
        }

        /// <summary>
        /// Draws the chart to a Bitmap.
        /// </summary>
        /// <returns></returns>
        public Bitmap Draw()
        {
            bitmap = new Bitmap(Width, Height);
            var g = Graphics.FromImage(bitmap);

            DrawChartBackground(g);

            foreach (var module in Modules)
            {
                module.Draw(g);
            }

            return bitmap;
        }

        private void DrawChartBackground(Graphics g)
        {
            if (BackgroundColor != null)
            {
                g.FillRectangle(BackgroundColor, g.ClipBounds);
            }
        }

        /// <summary>
        /// Returns the X and Y coordinates of the top-left corner of the given module.
        /// </summary>
        internal Point GetModulePosition(StaticChartModule staticChartModule)
        {
            // Add modules heights until we find the given module.
            var height = 0;

            foreach (var module in Modules)
            {
                if (module == staticChartModule)
                {
                    return new Point(0, height);
                }

                height += module.Height;
            }

            throw new Exception("StaticChartModule not found inside of StaticChart. The given StaticChartModule is not a children of the current StaticChart instance.");
        }
    }
}
