using System.Collections.Generic;

namespace TradeHub.Core.Model.Interfaces
{
    /// <summary>
    /// Defines a trading strategy.
    /// </summary>
    public interface IStrategy
    {
        /// <summary>
        /// The name of the strategy.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Runs the strategy on the given input data for the current step.
        /// </summary>
        /// <param name="stockData">The historical data to use to feed the strategy.</param>
        /// <param name="currentStockTick">The current data tick (also present in stockData at index currentIteration).</param>
        /// <param name="currentIteration">The current iteration of the simulation (in the case of a backtest) or the last stockData index available (in the case of live trading).</param>
        void Run(List<StockTick> stockData, StockTick currentStockTick, int currentIteration);
    }
}
