using System.Drawing;

namespace TradeHub.Charts
{
    public class StaticChartModule
    {
        /// <summary>
        /// The height of the module.
        /// </summary>
        public int Height { get; set; }

        public StaticChartModule()
        {
            Height = 100;
        }

        /// <summary>
        /// Draw the module to a Graphics object.
        /// </summary>
        public void Draw(Graphics g, int width)
        {

        }
    }
}
