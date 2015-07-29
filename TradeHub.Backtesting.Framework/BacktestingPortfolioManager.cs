using System;
using System.Linq;
using TradeHub.Core.Model;
using TradeHub.Core.Model.Enums;

namespace TradeHub.Backtesting.Framework
{
    /// <summary>
    /// A portfolio manager that accepts all trades and assumes they got through "as is".
    /// Use for simulations only.
    /// </summary>
    /// <remarks>
    /// This PortfolioManager does not handle trade rejections and cancellations like a real
    /// market would do. It takes trade orders and applies them directly to the portfolio,
    /// without going through any market.
    /// </remarks>
    public static class BacktestingPortfolioManager
    {
        /// <summary>
        /// Execute a TradeOrder on a Portfolio.
        /// In this implementation, all orders are handled as if they were accepted by the market.
        /// </summary>
        /// <returns>Whether the trade was executed successfuly.</returns>
        public static bool ExecuteTradeOrder(TradeOrder tradeOrder, Portfolio portfolio, decimal commission = 0)
        {
            if (!tradeOrder.LimitPrice.HasValue)
            {
                // Can't execute the order because there is no price information in it
                return false;
            }

            if (portfolio.CashBalance < commission)
            {
                // Not enough funds to pay for the commission
                return false;
            }

            var tradeAmount = tradeOrder.Quantity * tradeOrder.LimitPrice.Value;
            var openPosition = portfolio.Positions.FirstOrDefault(p => p.Symbol == tradeOrder.Symbol);

            if (tradeOrder.Action == TradeOrderAction.Buy)
            {
                if (tradeAmount + commission > portfolio.CashBalance)
                {
                    // Not enough funds to execute the order
                    return false;
                }

                ApplyBuyOrder(tradeOrder, portfolio, openPosition);
            }
            else if (tradeOrder.Action == TradeOrderAction.Sell)
            {    
                if (openPosition == null)
                {
                    // Trying to sell a position we don't have
                    // TODO: support SHORT positions
                    return false;
                }

                if (tradeOrder.Quantity > openPosition.Quantity)
                {
                    // Trying to sell more than we have of this position
                    // TODO: support SHORT positions
                    return false;
                }

                ApplySellOrder(tradeOrder, portfolio, openPosition);
            }
            else
            {
                throw new Exception("Unknown TradeOrderAction");
            }

            portfolio.TransactionHistory.Add(tradeOrder);
            portfolio.CashBalance -= commission;
            return true;
        }

        /// <summary>
        /// Update the portfolio positions by applying the provided BUY TradeOrder.
        /// </summary>
        private static void ApplyBuyOrder(TradeOrder tradeOrder, Portfolio portfolio, Position openPosition)
        {
            if (!tradeOrder.LimitPrice.HasValue)
            {
                throw new ArgumentNullException("The TradeOrder has no LimitPrice");
            }
            
            var tradeOrderValue = tradeOrder.Quantity * tradeOrder.LimitPrice.Value;
            portfolio.CashBalance -= tradeOrderValue; // Commission deduction is handled by the caller of this method

            if (openPosition != null)
            {
                // Calculate average price
                // ((qty1 * price1) + (qty2 / price2)) / (qty1 + qty2)
                var newQuantity = openPosition.Quantity + tradeOrder.Quantity;
                var openPositionValue = openPosition.Quantity * openPosition.AveragePrice;
                var newAveragePrice = (openPositionValue + tradeOrderValue) / newQuantity;

                // Add to existing position
                openPosition.Quantity = newQuantity;
                openPosition.AveragePrice = newAveragePrice;
                return;
            }

            // Create new position
            portfolio.Positions.Add(new Position
            {
                AveragePrice = tradeOrder.LimitPrice.Value,
                PositionType = PositionType.Long,
                Quantity = tradeOrder.Quantity,
                Symbol = tradeOrder.Symbol
            });
        }

        /// <summary>
        /// Update the portfolio positions by applying the provided SELL TradeOrder.
        /// </summary>
        private static void ApplySellOrder(TradeOrder tradeOrder, Portfolio portfolio, Position openPosition)
        {
            if (!tradeOrder.LimitPrice.HasValue)
            {
                throw new ArgumentNullException("The TradeOrder has no LimitPrice");
            }

            var tradeOrderValue = tradeOrder.Quantity * tradeOrder.LimitPrice.Value;
            portfolio.CashBalance += tradeOrderValue; // Commission deduction is handled by the caller of this method

            if (tradeOrder.Quantity == openPosition.Quantity)
            {
                // Sell whole position
                portfolio.Positions.Remove(openPosition);
                return;
            }

            // Sell partial position
            openPosition.Quantity -= tradeOrder.Quantity;
        }
    }
}
