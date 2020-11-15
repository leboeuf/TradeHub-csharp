using System;
using System.Collections.Generic;
using System.Linq;
using TradeHub.Backtesting.Framework;
using TradeHub.Backtesting.Framework.Interfaces;
using TradeHub.Core.Model;
using TradeHub.Core.Model.Enums;

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
    public class DailyMomentumBacktestingStrategy : IBacktestingStrategy
    {
        public string Name => "Daily Momentum";

        public void Run(BacktestingContext context)
        {
            var currentStockTick = context.StockData[context.CurrentIteration];

            if (currentStockTick.Close > currentStockTick.Open)
            {
                if (context.Portfolio.Positions.Any(x =>
                    x.Symbol == context.Symbol &&
                    x.PositionType == PositionType.Long))
                {
                    // Asset already in portfolio
                    return;
                }

                // Buy asset
                BacktestingPortfolioManager.ExecuteTradeOrder(new TradeOrder
                {
                    Symbol = context.Symbol,
                    Action = TradeOrderAction.Buy,
                    LimitPrice = currentStockTick.Close,
                    Quantity = 1000,
                    Timestamp = currentStockTick.Timestamp
                }, context.Portfolio);
            }

            else if (currentStockTick.Close < currentStockTick.Open)
            {
                if (!context.Portfolio.Positions.Any(x =>
                    x.Symbol == context.Symbol &&
                    x.PositionType == PositionType.Long))
                {
                    // Asset not in portfolio
                    return;
                }

                // Sell asset
                BacktestingPortfolioManager.ExecuteTradeOrder(new TradeOrder
                {
                    Symbol = context.Symbol,
                    Action = TradeOrderAction.Sell,
                    LimitPrice = currentStockTick.Close,
                    Quantity = 1000,
                    Timestamp = currentStockTick.Timestamp
                }, context.Portfolio);
            }
        }
    }
}
