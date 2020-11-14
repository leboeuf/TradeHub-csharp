using System.Linq;
using TradeHub.Backtesting.Framework;
using TradeHub.Backtesting.Framework.Interfaces;
using TradeHub.Core.Model;
using TradeHub.Core.Model.Enums;

namespace TradeHub.Backtesting.ConsoleClient.Strategies
{
    /// <summary>
    /// Buy an asset every time a breakout occurs.
    /// </summary>
    public class BreakoutBuyBacktestingStrategy : IBacktestingStrategy
    {
        public string Name => "Breakout Buy";

        public void Run(BacktestingContext context)
        {
            if (context.CurrentIteration < 2)
            {
                return;
            }

            var previousStockTick = context.StockData[context.CurrentIteration- 1];
            var currentStockTick = context.StockData[context.CurrentIteration];

            if (currentStockTick.Close > 1.2m * previousStockTick.Close)
            {
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
        }
    }
}
