using System;
using System.Collections.Generic;
using System.Linq;
using TradeHub.Core.Model;
using TradeHub.Core.Model.Enums;

namespace TradeHub.Backtesting.Framework
{
    public class Backtest
    {
        /// <summary>
        /// The context data for this backtest.
        /// </summary>
        private BacktestingContext context = new BacktestingContext();

        /// <summary>
        /// Set the stock data that will be used for the backtest.
        /// </summary>
        /// <param name="StockData"></param>
        public void SetStockData(List<StockTick> StockData)
        {
            context.StockData = StockData;
            context.Portfolio = new Portfolio
            {
                CashBalance = 100000
            };
        }

        /// <summary>
        /// Save a trade order to be executed at a certain date in the simulation.
        /// </summary>
        public void PlaceTradeOrder(TradeOrderAction action, string symbol, DateTime timestamp, decimal price, int quantity)
        {
            context.InputTransactions.Add(new TradeOrder
            {
                Action = action,
                Symbol = symbol,
                Timestamp = timestamp,
                LimitPrice = price,
                Quantity = quantity
            });
        }

        /// <summary>
        /// Run the simulation.
        /// </summary>
        /// <remarks>
        /// The "current" word in variables names refers to the current iteration step in the simulation.
        /// </remarks>
        public void Run()
        {
            for (context.CurrentIteration = 0; context.CurrentIteration < context.StockData.Count(); ++context.CurrentIteration)
            {
                var currentStockTick = context.StockData[context.CurrentIteration];

                // Check if there is any transaction pending
                var currentDate = currentStockTick.Timestamp.Date;
                var transactionsForCurrentDate = context.InputTransactions.Where(t => t.Timestamp.Date == currentDate).ToList();
                if (transactionsForCurrentDate.Any())
                {
                    ExecuteTradeOrders(transactionsForCurrentDate);
                }

                // Other backtesting logic / strategy execution here
            }
        }

        private void ExecuteTradeOrders(IEnumerable<TradeOrder> transactionsForCurrentDate)
        {
            foreach (var tradeOrder in transactionsForCurrentDate)
            {
                ExecuteTradeOrder(tradeOrder);
            }
        }

        private void ExecuteTradeOrder(TradeOrder tradeOrder)
        {
            BacktestingPortfolioManager.ExecuteTradeOrder(tradeOrder, context.Portfolio);
        }
    }
}
