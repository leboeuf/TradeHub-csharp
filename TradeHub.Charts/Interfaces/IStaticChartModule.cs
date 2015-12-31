using System.Drawing;

namespace TradeHub.Charts.Interfaces
{
    /// <summary>
    /// Defines a static chart module.
    /// </summary>
    public interface IStaticChartModule
    {
        int Height { get; }

        /// <summary>
        /// Draws the module on the Graphics drawing surface.
        /// </summary>
        void Draw(Graphics g);
    }
}
