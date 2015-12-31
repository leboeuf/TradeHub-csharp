using System.Drawing;

namespace TradeHub.Charts.Interfaces
{
    interface IStaticChartModule
    {
        /// <summary>
        /// Draws the module on the Graphics drawing surface.
        /// </summary>
        void Draw(Graphics g);
    }
}
