using System;
using System.Collections.Generic;
using System.Linq;
using TradeHub.Backtesting.Framework.Interfaces;
using TradeHub.Core.Model;
using TradeHub.Core.Model.Enums;

namespace TradeHub.Backtesting.Framework
{
    public class Backtest
    {
        /// <summary>
        /// The context data for this backtest.
        /// </summary>
        public BacktestingContext Context = new BacktestingContext();

        /// <summary>
        /// The strategy to run at each step of the simulation.
        /// </summary>
        public IBacktestingStrategy BacktestingStrategy;

        /// <summary>
        /// Save a trade order to be executed at a certain date in the simulation.
        /// </summary>
        public void PlaceTradeOrder(TradeOrderAction action, string symbol, DateTime timestamp, decimal price, int quantity)
        {
            Context.InputTransactions.Add(new TradeOrder
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
            for (Context.CurrentIteration = 0; Context.CurrentIteration < Context.StockData.Count(); ++Context.CurrentIteration)
            {
                var currentStockTick = Context.StockData[Context.CurrentIteration];

                // Check if there is any transaction pending
                var currentDate = currentStockTick.Timestamp.Date;
                var transactionsForCurrentDate = Context.InputTransactions.Where(t => t.Timestamp.Date == currentDate).ToList();
                if (transactionsForCurrentDate.Any())
                {
                    ExecuteTradeOrders(transactionsForCurrentDate);
                }
                
                BacktestingStrategy?.Run(Context);
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
            BacktestingPortfolioManager.ExecuteTradeOrder(tradeOrder, Context.Portfolio);
        }
    }
}
