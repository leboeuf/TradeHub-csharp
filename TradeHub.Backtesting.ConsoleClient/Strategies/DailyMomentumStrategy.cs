using System;
using System.Collections.Generic;
using TradeHub.Core.Model;
using TradeHub.Core.Model.Interfaces;

namespace TradeHub.Backtesting.ConsoleClient.Strategies
{
    /// <summary>
    /// Buy an asset when its closing price is higher than its open price. Hold if already in portfolio. 
    /// Sell if close price is lower than open price.
    /// </summary>
    /// <remarks>
    /// Idea taken from:
    /// http://www.philosophicaleconomics.com/2015/12/backtesting/
    /// </remarks>
    public class DailyMomentumStrategy : IStrategy
    {
        public string Name => "Daily Momentum";

        public void Run(List<StockTick> stockData, StockTick currentStockTick, int currentIteration)
        {
            throw new NotImplementedException();
        }
    }
}
