using System;
using TradeHub.Core.Model.Enums;

namespace TradeHub.Core.Model
{
    /// <summary>
    /// Represents a trade order for a stock.
    /// </summary>
    public class TradeOrder
    {
        /// <summary>
        /// The date/time at which the order was sent.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Determines whether the order is buy or sell.
        /// </summary>
        public TradeOrderAction Action { get; set; }

        /// <summary>
        /// The symbol targeted by the order.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// The amount of shares targeted by the order.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The price at which to execute the trade.
        /// Null if market order
        /// </summary>
        public decimal? LimitPrice { get; set; }
    }
}
