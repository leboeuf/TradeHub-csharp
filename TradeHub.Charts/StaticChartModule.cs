using System.Drawing;

namespace TradeHub.Charts
{
    public class StaticChartModule
    {
        /// <summary>
        /// The StaticChart object hosting this module. Used to access chart properties such as width and style.
        /// </summary>
        private StaticChart parent;

        /// <summary>
        /// The height of the module.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets the X and Y coordinates of this module inside the StaticChart.
        /// </summary>
        public Point Position
        {
            get { return parent.GetModulePosition(this); } //TODO: fix this leak, we create a Point each time this is called.
            private set { }
        }

        public StaticChartModule(StaticChart parent)
        {
            this.parent = parent;
            Height = 100;
        }

        /// <summary>
        /// Draws the module to a Graphics object.
        /// </summary>
        public void Draw(Graphics g)
        {
            DrawBorder(g);
        }

        private void DrawBorder(Graphics g)
        {
            if (parent.ModulesBorderWidth == 0)
            {
                return;
            }

            g.DrawRectangle(parent.ModulesBorderColor, 0, Position.Y, parent.Width - 1, Position.Y + Height - 1);
        }
    }
}
