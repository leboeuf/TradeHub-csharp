using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using TradeHub.Backtesting.ConsoleClient.Strategies;
using TradeHub.Backtesting.Framework;
using TradeHub.Charts;
using TradeHub.Core.DataProviders;
using TradeHub.Core.Model;
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
            WriteTitle();
            WriteInfo("Setting up backtest...");

            var backtest = new Backtest
            {
                BacktestingStrategy = new DailyMomentumBacktestingStrategy()
            };

            var portfolio = new Portfolio
            {
                CashBalance = 100000
            };

            backtest.Context.Portfolio = portfolio;

            WriteInfo("Downloading historical data...");
            backtest.Context.StockData = await YahooHistoricalDataProvider.DownloadHistoricalData("YHOO", new DateTime(2015, 09, 02), DateTime.Now);

            WriteInfo("Placing input trade orders...");
            backtest.PlaceTradeOrder(TradeOrderAction.Buy, "YHOO", new DateTime(2015, 09, 02), 38.18m, 1000);

            WriteInfo("Backtest setup done.");
            WriteBacktestSetup(backtest);

            var stopwatch = new Stopwatch();
            WriteInfo("Backtest starts.");
            stopwatch.Start();

            backtest.Run();

            stopwatch.Stop();
            WriteInfo("Backtest finished.");
            Console.WriteLine("Elapsed: {0} seconds ({1})", stopwatch.Elapsed.TotalSeconds, stopwatch.Elapsed);

            WriteBacktestResults(backtest);
            GenerateChart(backtest);

            Console.ReadKey();
        }

        private static void WriteTitle()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("*************************** ");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Write("TradeHub Backtesting CLI");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" ***************************");
            Console.ResetColor();
        }

        private static void WriteBacktestSetup(Backtest backtest)
        {
            Console.WriteLine();
            Console.WriteLine("Backtest setup");
            Console.WriteLine(" Portfolio cash balance: {0}", backtest.Context.Portfolio.CashBalance);
            Console.WriteLine(" Portfolio positions: {0}", backtest.Context.Portfolio.Positions.Count);
            Console.WriteLine(" Days of historical data: {0}", backtest.Context.StockData.Count);
            Console.WriteLine(" Reference timeframe: {0} to {1}", backtest.Context.StockData[0].Timestamp.ToShortDateString(), backtest.Context.StockData[backtest.Context.StockData.Count - 1].Timestamp.ToShortDateString());

            Console.Write(" Strategy: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("{0}{1}", backtest.BacktestingStrategy.Name, Environment.NewLine);
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine();
        }

        private static void WriteBacktestResults(Backtest backtest)
        {
            Console.WriteLine();
            Console.WriteLine("Backtest results");
            Console.WriteLine(" Portfolio cash balance: {0}", backtest.Context.Portfolio.CashBalance);
            Console.WriteLine(" Portfolio positions: {0}", backtest.Context.Portfolio.Positions.Count);
            Console.WriteLine(" Number of trades done: {0}", backtest.Context.Portfolio.TransactionHistory.Count);

            var totalPortfolioValue = backtest.Context.Portfolio.CashBalance;
            foreach (var position in backtest.Context.Portfolio.Positions)
            {
                totalPortfolioValue += position.Quantity * backtest.Context.StockData[backtest.Context.StockData.Count - 1].Close;
            }
            Console.WriteLine(" Portfolio total value: {0}", totalPortfolioValue);

            Console.WriteLine();
        }

        private static void GenerateChart(Backtest backtest)
        {
            Console.WriteLine("Generating chart...");

            var path = string.Format("{0}\\tradehub-backtest-{1}-{2}.png", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), backtest.Context.Symbol, DateTime.Now.ToString("yyyyMMddHHmmss"));
            var chart = new StaticChart
            {
                Width = 640,
                BackgroundColor = Color.Beige,
                Symbol = backtest.Context.Symbol,
                StockData = backtest.Context.StockData,
                TransactionHistory = backtest.Context.Portfolio.TransactionHistory
            };

            var bitmap = chart.Draw();
            bitmap.Save(path, ImageFormat.Png);
            Console.WriteLine("Chart saved to: {0}", path);

            Console.WriteLine();
        }

        private static void WriteInfo(string message)
        {
            Console.WriteLine("{0:yyyy-MM-dd HH:mm:ss.fff}: {1}", DateTime.Now, message);
        }
    }   
}
