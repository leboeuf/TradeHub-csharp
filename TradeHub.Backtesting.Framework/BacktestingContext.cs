using System.Collections.Generic;
using TradeHub.Core.Model;

namespace TradeHub.Backtesting.Framework
{
    /// <summary>
    /// Contains the contextual data of a specific backtest.
    /// </summary>
    public class BacktestingContext
    {
        /// <summary>
        /// Current backtesting loop iteration index (i.e. the index of the day we currently are at in the simulation).
        /// </summary>
        public int CurrentIteration { get; set; }

        /// <summary>
        /// The historical data on which the simulation is based.
        /// </summary>
        public List<StockTick> StockData = new List<StockTick>();

        /// <summary>
        /// The list of transactions declared before running the simulation.
        /// These trades will be executed when their target date is reached in the simulation.
        /// </summary>
        public List<TradeOrder> InputTransactions = new List<TradeOrder>();

        public Portfolio Portfolio { get; set; }
    }
}
