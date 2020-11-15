using System.Linq;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TradeHub.Core.DataProviders
{
    public static class IEXSymbolsDataProvider
    {
        /// <summary>
        /// Get the full list of available symbols on IEX.
        /// </summary>
        public static async Task<string[]> GetAvailableSymbols()
        {
            var url = "https://iextrading.com/api/mobile/refdata";

            // Get data
            var client = new WebClient();
            var json = await client.DownloadStringTaskAsync(url);
            client.Dispose();

            // Parse JSON
            var result = JsonSerializer.Deserialize<IexEligibleSymbol[]>(json);

            // Remove disabled symbols, listings with empty names and symbols with special characters
            return result
                .Where(x => 
                    x.IsEnabled == "1" 
                    && !string.IsNullOrEmpty(x.Issuer) 
                    && x.Symbol.All(c => char.IsLetter(c)))
                .Select(x => x.Symbol)
                .ToArray();
        }
    }

    internal class IexEligibleSymbol
    {
        public string Symbol { get; set; }

        public string Issuer { get; set; }

        [JsonPropertyNameAttribute("isEnabled")]
        public string IsEnabled { get; set; }
    }
}
