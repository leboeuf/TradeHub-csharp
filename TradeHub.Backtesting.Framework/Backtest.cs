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
        /// Current backtesting loop iteration index (i.e. the index of the day we currently are at in the simulation).
        /// </summary>
        private int currentIteration;

        /// <summary>
        /// The list of transactions declared before running the simulation.
        /// These trades will be executed when their target date is reached in the simulation.
        /// </summary>
        private List<TradeOrder> inputTransactions;

        /// <summary>
        /// The historical price list on which the simulation is based.
        /// </summary>
        public List<StockTick> StockData;

        public Backtest()
        {
            this.inputTransactions = new List<TradeOrder>();
        }

        /// <summary>
        /// Saves a trade order to be executed at a certain date in the simulation.
        /// </summary>
        public void PlaceTradeOrder(TradeOrderAction action, string symbol, DateTime timestamp, decimal price, int quantity)
        {
            inputTransactions.Add(new TradeOrder
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
        public void Run()
        {
            for (currentIteration = 0; currentIteration < StockData.Count(); ++currentIteration)
            {
                var currentStockTick = StockData[currentIteration];

                // Check if there is any transaction pending
                var currentDate = currentStockTick.Timestamp.Date;
                var transactionsForCurrentDate = inputTransactions.Where(t => t.Timestamp.Date == currentDate).ToList();
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
            throw new NotImplementedException();
        }
    }
}
