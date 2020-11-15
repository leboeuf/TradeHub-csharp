using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TradeHub.Core.DataProviders;
using TradeHub.Database;

namespace TradeHub.StockScreener
{
    public class Screener
    {
        /// <summary>
        /// Finds symbols matching the given search options.
        /// </summary>
        /// <returns>An array of symbols.</returns>
        public async Task<string[]> Find(ScreenerOptions options = null)
        {
            if (!await SchemaExists())
            {
                await SetupDatabase();
            }

            if (options == null)
            {
                // Return the full list of symbols
                var dataTable = await DatabaseHelper.ExecuteQuery("SELECT symbol FROM symbols ORDER BY symbol");
                return dataTable.AsEnumerable().Select(r => r[0].ToString()).ToArray();
            }

            // TODO
            return null;
        }

        private async Task<bool> SchemaExists()
        {
            var result = await DatabaseHelper.ExecuteScalar("SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'symbols'");
            return result != null;
        }

        private async Task SetupDatabase()
        {
            await DatabaseHelper.ExecuteNonQuery("CREATE TABLE IF NOT EXISTS symbols (symbol TEXT PRIMARY KEY)");

            // Get all symbols
            var symbols = await IEXSymbolsDataProvider.GetAvailableSymbols();
            foreach (var symbol in symbols)
            {
                await DatabaseHelper.ExecuteNonQuery($"INSERT INTO symbols (symbol) VALUES ('{symbol}')");
            }
        }
    }
}
