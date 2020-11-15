using System.Collections.Generic;
using TradeHub.Core.Model;

namespace TradeHub.Backtesting.Framework.Interfaces
{
    /// <summary>
    /// Defines a trading strategy.
    /// </summary>
    public interface IBacktestingStrategy
    {
        /// <summary>
        /// The name of the strategy.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Runs the strategy on the given input data for the current step.
        /// </summary>
        /// <param name="context">The context on which to run the strategy.</param>
        void Run(BacktestingContext context);
    }
}
