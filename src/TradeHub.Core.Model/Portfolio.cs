using System.Collections.Generic;

namespace TradeHub.Core.Model
{
    /// <summary>
    /// A grouping of financial assets.
    /// </summary>
    public class Portfolio
    {
        /// <summary>
        /// Gets or sets the available cash balance.
        /// </summary>
        public decimal CashBalance { get; set; }

        /// <summary>
        /// The list of securities owned in the portfolio.
        /// </summary>
        public List<Position> Positions { get; set; }

        /// <summary>
        /// The list of transactions made in the portfolio.
        /// TODO: TradeOrder is not sufficient, we need to know how much was paid in commission.
        /// </summary>
        public List<TradeOrder> TransactionHistory { get; set; }

        public Portfolio()
        {
            Positions = new List<Position>();
            TransactionHistory = new List<TradeOrder>();
        }
    }
}
