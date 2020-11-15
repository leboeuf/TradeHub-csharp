using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TradeHub.Core.Model;
using TradeHub.Core.Model.Enums;

namespace TradeHub.Backtesting.Framework.Tests
{
    [TestClass]
    public class BacktestingPortfolioManagerTests
    {
        const decimal InitialCashBalance = 100000;
        const decimal TestCommission = 15;
        const string TestSymbol = "GOOG";

        [TestMethod]
        public void ExecuteTradeOrder_ValidBuyOrderWithoutCommission_ReturnsTrue()
        {
            var portfolio = new Portfolio
            {
                CashBalance = InitialCashBalance
            };

            var validBuyOrder = new TradeOrder
            {
                Action = TradeOrderAction.Buy,
                Quantity = 100,
                Symbol = TestSymbol,
                LimitPrice = 500,
                Timestamp = DateTime.Now
            };

            // Test that the order is valid
            var result = BacktestingPortfolioManager.ExecuteTradeOrder(validBuyOrder, portfolio);
            Assert.IsTrue(result);

            // Test that the portfolio now contains a position
            Assert.AreEqual(portfolio.Positions.Count, 1);

            // Test that cash balance has been updated
            Assert.AreEqual(portfolio.CashBalance, InitialCashBalance - (validBuyOrder.Quantity * validBuyOrder.LimitPrice));
        }

        public void ExecuteTradeOrder_ValidBuyOrderWithCommission_ReturnsTrue()
        {
            var portfolio = new Portfolio
            {
                CashBalance = InitialCashBalance
            };

            var validBuyOrder = new TradeOrder
            {
                Action = TradeOrderAction.Buy,
                Quantity = 100,
                Symbol = TestSymbol,
                LimitPrice = 500,
                Timestamp = DateTime.Now
            };

            // Test that the order is valid
            var result = BacktestingPortfolioManager.ExecuteTradeOrder(validBuyOrder, portfolio, TestCommission);
            Assert.IsTrue(result);

            // Test that the portfolio now contains a position
            Assert.AreEqual(portfolio.Positions.Count, 1);

            // Test that cash balance has been updated
            Assert.AreEqual(portfolio.CashBalance, InitialCashBalance - (validBuyOrder.Quantity * validBuyOrder.LimitPrice) - TestCommission);
        }

        [TestMethod]
        public void ExecuteTradeOrder_InvalidBuyOrderQuantity_ReturnsFalse()
        {
            var portfolio = new Portfolio
            {
                CashBalance = InitialCashBalance
            };

            var invalidBuyOrderNotEnoughFunds = new TradeOrder
            {
                Action = TradeOrderAction.Buy,
                Quantity = 9999999,
                Symbol = TestSymbol,
                LimitPrice = 500,
                Timestamp = DateTime.Now
            };

            // Test that the order is invalid
            var result = BacktestingPortfolioManager.ExecuteTradeOrder(invalidBuyOrderNotEnoughFunds, portfolio);
            Assert.IsFalse(result);

            // Test that the portfolio has not been modified
            Assert.AreEqual(portfolio.Positions.Count, 0);
            Assert.AreEqual(portfolio.CashBalance, InitialCashBalance);
        }

        [TestMethod]
        public void ExecuteTradeOrder_InvalidBuyOrderLimitPrice_ReturnsFalse()
        {
            var portfolio = new Portfolio
            {
                CashBalance = InitialCashBalance
            };

            var invalidBuyOrderNoLimitPrice = new TradeOrder
            {
                Action = TradeOrderAction.Buy,
                Quantity = 100,
                Symbol = TestSymbol,
                Timestamp = DateTime.Now
            };

            // Test that the order is invalid
            var result = BacktestingPortfolioManager.ExecuteTradeOrder(invalidBuyOrderNoLimitPrice, portfolio);
            Assert.IsFalse(result);

            // Test that the portfolio has not been modified
            Assert.AreEqual(portfolio.Positions.Count, 0);
            Assert.AreEqual(portfolio.CashBalance, InitialCashBalance);
        }

        [TestMethod]
        public void ExecuteTradeOrder_ValidFullSellOrderWithoutCommission_ReturnsTrue()
        {
            var portfolio = new Portfolio
            {
                CashBalance = InitialCashBalance
            };

            portfolio.Positions.Add(new Position
            {
                Quantity = 100,
                Symbol = TestSymbol,
                PositionType = PositionType.Long,
                AveragePrice = 500
            });

            var validFullSellOrder = new TradeOrder
            {
                Action = TradeOrderAction.Sell,
                Quantity = 100,
                Symbol = TestSymbol,
                LimitPrice = 500,
                Timestamp = DateTime.Now
            };

            // Test that the order is valid
            var result = BacktestingPortfolioManager.ExecuteTradeOrder(validFullSellOrder, portfolio);
            Assert.IsTrue(result);

            // Test that the position is not in the portfolio anymore
            Assert.AreEqual(portfolio.Positions.Count, 0);

            // Test that cash balance has been updated
            Assert.AreEqual(portfolio.CashBalance, InitialCashBalance + (validFullSellOrder.Quantity * validFullSellOrder.LimitPrice));
        }

        [TestMethod]
        public void ExecuteTradeOrder_ValidFullSellOrderWithCommission_ReturnsTrue()
        {
            var portfolio = new Portfolio
            {
                CashBalance = InitialCashBalance
            };

            portfolio.Positions.Add(new Position
            {
                Quantity = 100,
                Symbol = TestSymbol,
                PositionType = PositionType.Long,
                AveragePrice = 500
            });

            var validFullSellOrder = new TradeOrder
            {
                Action = TradeOrderAction.Sell,
                Quantity = 100,
                Symbol = TestSymbol,
                LimitPrice = 500,
                Timestamp = DateTime.Now
            };

            // Test that the order is valid
            var result = BacktestingPortfolioManager.ExecuteTradeOrder(validFullSellOrder, portfolio, TestCommission);
            Assert.IsTrue(result);

            // Test that the position is not in the portfolio anymore
            Assert.AreEqual(portfolio.Positions.Count, 0);

            // Test that cash balance has been updated
            Assert.AreEqual(portfolio.CashBalance, InitialCashBalance + (validFullSellOrder.Quantity * validFullSellOrder.LimitPrice) - TestCommission);
        }

        [TestMethod]
        public void ExecuteTradeOrder_ValidPartialSellOrder_ReturnsTrue()
        {
            var portfolio = new Portfolio
            {
                CashBalance = InitialCashBalance
            };

            portfolio.Positions.Add(new Position
            {
                Quantity = 100,
                Symbol = TestSymbol,
                PositionType = PositionType.Long,
                AveragePrice = 500
            });

            var validPartialSellOrder = new TradeOrder
            {
                Action = TradeOrderAction.Sell,
                Quantity = 50,
                Symbol = TestSymbol,
                LimitPrice = 500,
                Timestamp = DateTime.Now
            };

            // Test that the order is valid
            var result = BacktestingPortfolioManager.ExecuteTradeOrder(validPartialSellOrder, portfolio);
            Assert.IsTrue(result);

            // Test that the position has been reduced
            Assert.AreEqual(portfolio.Positions[0].Quantity, 50);

            // Test that the cash balance has been updated
            Assert.AreEqual(portfolio.CashBalance, InitialCashBalance + (validPartialSellOrder.Quantity * validPartialSellOrder.LimitPrice));
        }

        [TestMethod]
        public void ExecuteTradeOrder_InvalidSellOrderQuantity_ReturnsFalse()
        {
            var portfolio = new Portfolio
            {
                CashBalance = InitialCashBalance
            };

            portfolio.Positions.Add(new Position
            {
                Quantity = 100,
                Symbol = TestSymbol,
                PositionType = PositionType.Long,
                AveragePrice = 500
            });

            var invalidSellOrderQuantityMoreThanPosition = new TradeOrder
            {
                Action = TradeOrderAction.Sell,
                Quantity = 101,
                Symbol = TestSymbol,
                LimitPrice = 500,
                Timestamp = DateTime.Now
            };

            // Test that the order is invalid
            var result = BacktestingPortfolioManager.ExecuteTradeOrder(invalidSellOrderQuantityMoreThanPosition, portfolio);
            Assert.IsFalse(result);

            // Test that the portfolio has not been modified
            Assert.AreEqual(portfolio.Positions.Count, 1);
            Assert.AreEqual(portfolio.CashBalance, InitialCashBalance);
        }
    }
}
