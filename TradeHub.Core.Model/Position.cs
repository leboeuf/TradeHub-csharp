using TradeHub.Core.Model.Enums;

namespace TradeHub.Core.Model
{
    /// <summary>
    /// An amount of a certain security owned.
    /// </summary>
    public class Position
    {
        /// <summary>
        /// The type of position.
        /// </summary>
        public PositionType PositionType { get; set; }

        /// <summary>
        /// The symbol representing the financial instrument of this position.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// The amount owned.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The average price paid for this position.
        /// </summary>
        public decimal AveragePrice { get; set; }
    }
}
