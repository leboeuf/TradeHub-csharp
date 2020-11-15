using System.Drawing;
using TradeHub.Charts.GDI;
using TradeHub.Charts.Interfaces;

namespace TradeHub.Charts.Modules
{
    /// <summary>
    /// Base implementation of a static chart module. All static chart modules can derive from this class to benefit from its generic drawing methods.
    /// </summary>
    public class StaticChartModule : IStaticChartModule
    {
        /// <summary>
        /// If set to true, the space between each tick will be represented by an integer. Otherwise, float will be used.
        /// In practice, when set to true each tick position will be rounded to the nearest pixel and whitespace might appear to the left or right of the chart data.
        /// When set to false the tick position will use all the available space and might overlap with other data or chart components.
        /// </summary>
        public const bool ROUND_SPACE_BETWEEN_DIV_X = false;

        public const int RIGHT_LEGEND_WIDTH = 60;
        public const int RIGHT_LEGEND_DASH_LENGTH = 4;
        public const int BOTTOM_LEGEND_WIDTH = 20;
        public const int BOTTOM_LEGEND_DASH_LENGTH = 4;

        /// <summary>
        /// The StaticChart object hosting this module. Used to access chart properties such as width and style.
        /// </summary>
        public StaticChart parent;

        /// <summary>
        /// Spaces between divisions on the X axis. This is calculated using the number of ticks to display and the available width.
        /// </summary>
        public float spaceBetweenDivX;

        /// <summary>
        /// The height of the module.
        /// </summary>
        public int Height { get; set; }

        public virtual void Draw(Graphics g)
        {
            spaceBetweenDivX = ROUND_SPACE_BETWEEN_DIV_X ?
                (int)(g.ClipBounds.Width - RIGHT_LEGEND_WIDTH - RIGHT_LEGEND_DASH_LENGTH) / parent.StockData.Count :
                (g.ClipBounds.Width - RIGHT_LEGEND_WIDTH - RIGHT_LEGEND_DASH_LENGTH) / parent.StockData.Count;

            DrawBorder(g);

            DrawYAxis(g);
            DrawXAxis(g);

            DrawData(g);
        }

        public void DrawBorder(Graphics g)
        {
            if (parent.ModulesBorderWidth == 0)
            {
                return;
            }

            DrawingHelper.DrawRectangle(g, parent.ModulesBorderColor, g.ClipBounds.X, g.ClipBounds.Y, g.ClipBounds.Width - 1, g.ClipBounds.Y + Height - 1);
        }

        public virtual void DrawXAxis(Graphics g)
        {
            return;
        }

        public virtual void DrawYAxis(Graphics g)
        {
            return;
        }

        public virtual void DrawData(Graphics g)
        {
            return;
        }

        /// <summary>
        /// Prints an error message on the on the Graphics object.
        /// </summary>
        public virtual void DrawError(Graphics g, string message)
        {
            var f = new Font("Arial", 14, FontStyle.Regular, GraphicsUnit.World);
            var strSize = g.MeasureString(message, f);
            var msgPos = new PointF(g.ClipBounds.Width / 2 - strSize.Width / 2, g.ClipBounds.Height / 2 - strSize.Height / 2);
            DrawingHelper.DrawString(g, message, f, Brushes.Red, msgPos);
        }
    }
}
