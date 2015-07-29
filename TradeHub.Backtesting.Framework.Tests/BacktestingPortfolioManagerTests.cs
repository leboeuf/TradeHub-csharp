using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeHub.Core.Model;
using TradeHub.Core.Model.Enums;

namespace TradeHub.Backtesting.Framework.Tests
{
    [TestClass]
    public class BacktestingPortfolioManagerTests
    {
        const decimal initialCashBalance = 100000;
        const decimal testCommission = 15;
        const string testSymbol = "GOOG";

        [TestMethod]
        public void ExecuteTradeOrder_ValidBuyOrderWithoutCommission_ReturnsTrue()
        {
            var portfolio = new Portfolio
            {
                CashBalance = initialCashBalance
            };

            var validBuyOrder = new TradeOrder
            {
                Action = TradeOrderAction.Buy,
                Quantity = 100,
                Symbol = testSymbol,
                LimitPrice = 500,
                Timestamp = DateTime.Now
            };

            // Test that the order is valid
            var result = BacktestingPortfolioManager.ExecuteTradeOrder(validBuyOrder, portfolio);
            Assert.IsTrue(result);

            // Test that the portfolio now contains a position
            Assert.AreEqual(portfolio.Positions.Count, 1);

            // Test that cash balance has been updated
            Assert.AreEqual(portfolio.CashBalance, initialCashBalance - (validBuyOrder.Quantity * validBuyOrder.LimitPrice));
        }

        public void ExecuteTradeOrder_ValidBuyOrderWithCommission_ReturnsTrue()
        {
            var portfolio = new Portfolio
            {
                CashBalance = initialCashBalance
            };

            var validBuyOrder = new TradeOrder
            {
                Action = TradeOrderAction.Buy,
                Quantity = 100,
                Symbol = testSymbol,
                LimitPrice = 500,
                Timestamp = DateTime.Now
            };

            // Test that the order is valid
            var result = BacktestingPortfolioManager.ExecuteTradeOrder(validBuyOrder, portfolio, testCommission);
            Assert.IsTrue(result);

            // Test that the portfolio now contains a position
            Assert.AreEqual(portfolio.Positions.Count, 1);

            // Test that cash balance has been updated
            Assert.AreEqual(portfolio.CashBalance, initialCashBalance - (validBuyOrder.Quantity * validBuyOrder.LimitPrice) - testCommission);
        }

        [TestMethod]
        public void ExecuteTradeOrder_InvalidBuyOrderQuantity_ReturnsFalse()
        {
            var portfolio = new Portfolio
            {
                CashBalance = initialCashBalance
            };

            var invalidBuyOrderNotEnoughFunds = new TradeOrder
            {
                Action = TradeOrderAction.Buy,
                Quantity = 9999999,
                Symbol = testSymbol,
                LimitPrice = 500,
                Timestamp = DateTime.Now
            };

            // Test that the order is invalid
            var result = BacktestingPortfolioManager.ExecuteTradeOrder(invalidBuyOrderNotEnoughFunds, portfolio);
            Assert.IsFalse(result);

            // Test that the portfolio has not been modified
            Assert.AreEqual(portfolio.Positions.Count, 0);
            Assert.AreEqual(portfolio.CashBalance, initialCashBalance);
        }

        [TestMethod]
        public void ExecuteTradeOrder_InvalidBuyOrderLimitPrice_ReturnsFalse()
        {
            var portfolio = new Portfolio
            {
                CashBalance = initialCashBalance
            };

            var invalidBuyOrderNoLimitPrice = new TradeOrder
            {
                Action = TradeOrderAction.Buy,
                Quantity = 100,
                Symbol = testSymbol,
                Timestamp = DateTime.Now
            };

            // Test that the order is invalid
            var result = BacktestingPortfolioManager.ExecuteTradeOrder(invalidBuyOrderNoLimitPrice, portfolio);
            Assert.IsFalse(result);

            // Test that the portfolio has not been modified
            Assert.AreEqual(portfolio.Positions.Count, 0);
            Assert.AreEqual(portfolio.CashBalance, initialCashBalance);
        }

        [TestMethod]
        public void ExecuteTradeOrder_ValidFullSellOrderWithoutCommission_ReturnsTrue()
        {
            var portfolio = new Portfolio
            {
                CashBalance = initialCashBalance
            };

            portfolio.Positions.Add(new Position
            {
                Quantity = 100,
                Symbol = testSymbol,
                PositionType = PositionType.Long,
                AveragePrice = 500
            });

            var validFullSellOrder = new TradeOrder
            {
                Action = TradeOrderAction.Sell,
                Quantity = 100,
                Symbol = testSymbol,
                LimitPrice = 500,
                Timestamp = DateTime.Now
            };

            // Test that the order is valid
            var result = BacktestingPortfolioManager.ExecuteTradeOrder(validFullSellOrder, portfolio);
            Assert.IsTrue(result);

            // Test that the position is not in the portfolio anymore
            Assert.AreEqual(portfolio.Positions.Count, 0);

            // Test that cash balance has been updated
            Assert.AreEqual(portfolio.CashBalance, initialCashBalance + (validFullSellOrder.Quantity * validFullSellOrder.LimitPrice));
        }

        [TestMethod]
        public void ExecuteTradeOrder_ValidFullSellOrderWithCommission_ReturnsTrue()
        {
            var portfolio = new Portfolio
            {
                CashBalance = initialCashBalance
            };

            portfolio.Positions.Add(new Position
            {
                Quantity = 100,
                Symbol = testSymbol,
                PositionType = PositionType.Long,
                AveragePrice = 500
            });

            var validFullSellOrder = new TradeOrder
            {
                Action = TradeOrderAction.Sell,
                Quantity = 100,
                Symbol = testSymbol,
                LimitPrice = 500,
                Timestamp = DateTime.Now
            };

            // Test that the order is valid
            var result = BacktestingPortfolioManager.ExecuteTradeOrder(validFullSellOrder, portfolio, testCommission);
            Assert.IsTrue(result);

            // Test that the position is not in the portfolio anymore
            Assert.AreEqual(portfolio.Positions.Count, 0);

            // Test that cash balance has been updated
            Assert.AreEqual(portfolio.CashBalance, initialCashBalance + (validFullSellOrder.Quantity * validFullSellOrder.LimitPrice) - testCommission);
        }

        [TestMethod]
        public void ExecuteTradeOrder_ValidPartialSellOrder_ReturnsTrue()
        {
            var portfolio = new Portfolio
            {
                CashBalance = initialCashBalance
            };

            portfolio.Positions.Add(new Position
            {
                Quantity = 100,
                Symbol = testSymbol,
                PositionType = PositionType.Long,
                AveragePrice = 500
            });

            var validPartialSellOrder = new TradeOrder
            {
                Action = TradeOrderAction.Sell,
                Quantity = 50,
                Symbol = testSymbol,
                LimitPrice = 500,
                Timestamp = DateTime.Now
            };

            // Test that the order is valid
            var result = BacktestingPortfolioManager.ExecuteTradeOrder(validPartialSellOrder, portfolio);
            Assert.IsTrue(result);

            // Test that the position has been reduced
            Assert.AreEqual(portfolio.Positions[0].Quantity, 50);

            // Test that the cash balance has been updated
            Assert.AreEqual(portfolio.CashBalance, initialCashBalance + (validPartialSellOrder.Quantity * validPartialSellOrder.LimitPrice));
        }

        [TestMethod]
        public void ExecuteTradeOrder_InvalidSellOrderQuantity_ReturnsFalse()
        {
            var portfolio = new Portfolio
            {
                CashBalance = initialCashBalance
            };

            portfolio.Positions.Add(new Position
            {
                Quantity = 100,
                Symbol = testSymbol,
                PositionType = PositionType.Long,
                AveragePrice = 500
            });

            var invalidSellOrderQuantityMoreThanPosition = new TradeOrder
            {
                Action = TradeOrderAction.Sell,
                Quantity = 101,
                Symbol = testSymbol,
                LimitPrice = 500,
                Timestamp = DateTime.Now
            };

            // Test that the order is invalid
            var result = BacktestingPortfolioManager.ExecuteTradeOrder(invalidSellOrderQuantityMoreThanPosition, portfolio);
            Assert.IsFalse(result);

            // Test that the portfolio has not been modified
            Assert.AreEqual(portfolio.Positions.Count, 1);
            Assert.AreEqual(portfolio.CashBalance, initialCashBalance);
        }
    }
}
