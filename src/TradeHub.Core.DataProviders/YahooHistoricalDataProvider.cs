using System;
using System.Collections.Generic;
using System.Globalization;
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
        /// <returns>A dictionary of lists of Ticks for the requested symbols. The symbol is used as the dictionary key.</returns>
        public static async Task<Dictionary<string, List<Tick>>> DownloadHistoricalData(List<string> symbols, DateTime start, DateTime end, HistoricalFrequency frequency = HistoricalFrequency.Daily)
        {
            var result = new Dictionary<string, List<Tick>>();

            foreach (var symbol in symbols)
            {
                await DownloadHistoricalData(symbol, start, end, frequency);
            }

            return result;
        }

        /// <summary>
        /// Download historical data for one symbol between 2 dates.
        /// </summary>
        /// <returns>A list of Ticks for the requested symbol.</returns>
        public static async Task<List<Tick>> DownloadHistoricalData(string symbol, DateTime start, DateTime end, HistoricalFrequency frequency = HistoricalFrequency.Daily)
        {
            var startTimestamp = Math.Floor(start.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            var endTimestamp = Math.Floor(end.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);

            string url = string.Format("https://query1.finance.yahoo.com/v7/finance/download/{0}?period1={1}&period2={2}&interval={3}&events=history&includeAdjustedClose=true&nocache={4}",
                symbol,
                startTimestamp,
                endTimestamp,
                HistoricalFrequencyToYahooQueryString(frequency),
                DateTime.UtcNow.ToString("yyyyMMddHHmmssffff")
            );

            // Get data
            var client = new WebClient();
            string data = await client.DownloadStringTaskAsync(url);
            client.Dispose();

            if (data.Length > 0)
            {
                var result = new List<Tick>();

                var lines = data.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int indLine = 1; indLine < lines.Length; indLine++) // indLine = 1 to skip header row
                {
                    var splitted = lines[indLine].Replace("\"", "").Split(',');

                    Tick tick;
                    if (frequency == HistoricalFrequency.DividendsOnly)
                    {
                        tick = new Tick
                        {
                            Timestamp = DateTime.Parse(splitted[0]),
                            Dividend = decimal.Parse(splitted[1])
                        };
                    }
                    else
                    {
                        tick = new Tick
                        {
                            Timestamp = DateTime.Parse(splitted[0], CultureInfo.InvariantCulture),
                            Open = decimal.Parse(splitted[1], CultureInfo.InvariantCulture),
                            High = decimal.Parse(splitted[2], CultureInfo.InvariantCulture),
                            Low = decimal.Parse(splitted[3], CultureInfo.InvariantCulture),
                            Close = decimal.Parse(splitted[4], CultureInfo.InvariantCulture),
                            //AdjustedClose = decimal.Parse(splitted[5], CultureInfo.InvariantCulture), // Requires "includeAdjustedClose=true" in URL parameters
                            Volume = int.Parse(splitted[6])
                        };
                    }

                    result.Add(tick);
                }

                return result;
            }

            throw new Exception(string.Format("No historical data for symbol: {0}", symbol));
        }

        /// <summary>
        /// Maps the HistoricalFrequency enum to the corresponding Yahoo parameter.
        /// </summary>
        /// <param name="frequency">The frequency of ticks.</param>
        /// <returns>The GET historical frequency parameter value to use for Yahoo historical data URL.</returns>
        private static string HistoricalFrequencyToYahooQueryString(HistoricalFrequency frequency)
        {
            //  Valid intervals: [1m, 2m, 5m, 15m, 30m, 60m, 90m, 1h, 1d, 5d, 1wk, 1mo, 3mo]
            switch (frequency)
            {
                case HistoricalFrequency.Daily: return "1d";
                case HistoricalFrequency.Weekly: return "1d";
                case HistoricalFrequency.Monthly: return "1d";
                default: throw new NotImplementedException($"{nameof(HistoricalFrequency)} not supported: '{frequency}'.");
            }
        }
    }
}
