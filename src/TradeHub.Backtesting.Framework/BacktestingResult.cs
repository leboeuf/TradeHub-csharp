using System.Collections.Generic;
using TradeHub.Core.Model;

namespace TradeHub.Backtesting.Framework
{
    /// <summary>
    /// The result of a backtesting run.
    /// </summary>
    public class BacktestingResult
    {
        /// <summary>
        /// Whether the strategy was profitable for the tested stock. To
        /// be deemed profitable, a signal must have ocurred (if the strategy 
        /// didn't buy or sell, it cannot be considered profitable).
        /// </summary>
        public bool IsProfitable { get; init; }

        /// <summary>
        /// The cash balance at the start of the simulation, before running the first iteration.
        /// </summary>
        public decimal StartingCashBalance { get; init; }

        /// <summary>
        /// The total value of positions held at the start of the simulation, before running the first iteration.
        /// </summary>
        public decimal StartingMarketValue { get; init; }

        /// <summary>
        /// The cash balance at the end of the simulation.
        /// </summary>
        public decimal EndingCashBalance { get; init; }

        /// <summary>
        /// The total value of positions held at the end of the simulation.
        /// </summary>
        public decimal EndingMarketValue { get; init; }

        /// <summary>
        /// The list of trades executed during the simulation.
        /// </summary>
        public List<ExecutedTradeOrder> TradesExecuted { get; init; }
    }
}
