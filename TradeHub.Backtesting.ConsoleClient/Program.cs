using System;
using System.Threading.Tasks;
using TradeHub.Backtesting.Framework;
using TradeHub.Core.DataProviders;
using TradeHub.Core.Model.Enums;

namespace TradeHub.Backtesting.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Not allowed to declare async Main, so run program in a Task.
            MainAsync(args).GetAwaiter().GetResult(); // StackOverflow #9208921
        }

        static async Task MainAsync(string[] args)
        {
            var backtest = new Backtest();
            backtest.SetStockData(await YahooHistoricalDataProvider.DownloadHistoricalData("YHOO", new DateTime(2015, 01, 02), DateTime.Now));

            backtest.PlaceTradeOrder(TradeOrderAction.Buy, "YHOO", new DateTime(2015, 01, 02), 38.18m, 100);

            backtest.Run();
        }
    }   
}
