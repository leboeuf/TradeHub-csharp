using System.Drawing;

namespace TradeHub.Charts.Interfaces
{
    /// <summary>
    /// Defines an overlay that can appear on top of a module.
    /// </summary>
    public interface IChartOverlay
    {
        /// <summary>
        /// Draws the overlay on the Graphics drawing surface.
        /// </summary>
        void Draw(Graphics g);
    }
}
