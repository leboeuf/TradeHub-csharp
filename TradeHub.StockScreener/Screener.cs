using System.Threading.Tasks;
using TradeHub.Core.DataProviders;
using TradeHub.Database;

namespace TradeHub.StockScreener
{
    public class Screener
    {
        public async Task<string> Find()
        {
            if (!await SchemaExists())
            {
                await SetupDatabase();
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
