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
                new StaticChartModule()
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
                module.Draw(g, Width);
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
    }
}
