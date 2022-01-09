using System.Drawing;

namespace TradeHub.Charts
{
    public class ChartStyleOptions
    {
        /// <summary>
        /// The background color of the chart.
        /// </summary>
        public Color BackgroundColor = Color.White;
        
        /// <summary>
        /// The border color surrounding each module. Set to null for no border.
        /// </summary>
        public Pen ModulesBorderColor = Pens.Black;

        /// <summary>
        /// The default color of ticks. This will be ignored if a specific style is requested (e.g. red/green candles).
        /// </summary>
        public Pen DataColor = Pens.Black;

        /// <summary>
        /// The color of axis lines and their dashes.
        /// </summary>
        public Pen AxisColor = Pens.Gray;

        /// <summary>
        /// The color of Y-axis labels.
        /// </summary>
        public Brush YAxisLabelsColor = Brushes.Black;

        /// <summary>
        /// The font name used for labels.
        /// </summary>
        public string LabelsFont { get; set; } = "Arial";

        /// <summary>
        /// The font size of labels, in em.
        /// </summary>
        public float LabelsFontSize { get; set; } = 14;

        /// <summary>
        /// The border width for each module border.
        /// </summary>
        public int ModulesBorderWidth = 1;
    }
}
