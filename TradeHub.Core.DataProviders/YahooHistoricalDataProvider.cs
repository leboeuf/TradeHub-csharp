using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TradeHub.Core.Model;
using TradeHub.Core.Model.Enums;

namespace TradeHub.Core.DataProviders
{
    public static class YahooHistoricalDataProvider
    {
        /// <summary>
        /// Get historical data between 2 dates for a list of symbols.
        /// </summary>
        /// <returns>A dictionary of lists of StockTicks for the requested symbols. The symbol is used as the dictionary key.</returns>
        public static async Task<Dictionary<string, List<StockTick>>> DownloadHistoricalData(List<string> symbols, DateTime start, DateTime end, HistoricalFrequency frequency = HistoricalFrequency.Daily)
        {
            var result = new Dictionary<string, List<StockTick>>();

            foreach (var symbol in symbols)
            {
                await DownloadHistoricalData(symbol, start, end, frequency);
            }

            return result;
        }

        /// <summary>
        /// Download historical data for one symbol between 2 dates.
        /// </summary>
        /// <returns>A list of StockTicks for the requested symbol.</returns>
        public static async Task<List<StockTick>> DownloadHistoricalData(string symbol, DateTime start, DateTime end, HistoricalFrequency frequency = HistoricalFrequency.Daily)
        {
            string url = string.Format("http://ichart.finance.yahoo.com/table.csv?s={0}&a={1}&b={2}&c={3}&d={4}&e={5}&f={6}&g={7}&ignore=.csv&nocache={8}",
                symbol,
                start.Month - 1,
                start.Day,
                start.Year,
                end.Month - 1,
                end.Day,
                end.Year,
                HistoricalFrequencyToYahooQueryString(frequency),
                DateTime.UtcNow.ToString("yyyyMMddHHmmssffff")
            );

            // Get data
            var client = new WebClient();
            string data = await client.DownloadStringTaskAsync(url);
            client.Dispose();

            if (data.Length > 0)
            {
                var result = new List<StockTick>();

                var lines = data.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int indLine = 1; indLine < lines.Length; indLine++) // indLine = 1 to skip header row
                {
                    var splitted = lines[indLine].Replace("\"", "").Split(',');

                    StockTick stockTick;
                    if (frequency == HistoricalFrequency.DividendsOnly)
                    {
                        stockTick = new StockTick
                        {
                            Timestamp = DateTime.Parse(splitted[0]),
                            Dividend = decimal.Parse(splitted[1])
                        };
                    }
                    else
                    {
                        stockTick = new StockTick
                        {
                            Timestamp = DateTime.Parse(splitted[0]),
                            Open = decimal.Parse(splitted[1]),
                            High = decimal.Parse(splitted[2]),
                            Low = decimal.Parse(splitted[3]),
                            Close = decimal.Parse(splitted[4]),
                            Volume = int.Parse(splitted[5])
                        };
                    }

                    result.Add(stockTick);
                }

                result.Reverse(); // Put in chronological order
                return result;
            }

            throw new Exception(string.Format("No historical data for symbol: {0}", symbol));
        }

        /// <summary>
        /// Maps the HistoricalFrequency enum to the corresponding Yahoo parameter.
        /// </summary>
        /// <param name="frequency">The frequency of ticks.</param>
        /// <returns>The GET historical frequency parameter value to use for Yahoo historical data URL.</returns>
        private static char HistoricalFrequencyToYahooQueryString(HistoricalFrequency frequency)
        {
            // HistoricalFrequency enum maps directly to Yahoo's values so simply return it.
            return (char)frequency;
        }
    }
}
