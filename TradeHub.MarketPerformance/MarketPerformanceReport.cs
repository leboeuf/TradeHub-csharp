using System.Threading.Tasks;
using TradeHub.Database;
using TradeHub.MarketPerformance.ETL;

namespace TradeHub.MarketPerformance
{
    public class MarketPerformanceReport
    {
        public async Task Run()
        {
            // Create database table
            DatabaseHelper.ExecuteNonQuery("DROP TABLE IF EXISTS listing");
            DatabaseHelper.ExecuteNonQuery("CREATE TABLE listing (index TEXT, symbol TEXT)");

            // Download listing
            var index = "^TTMN";
            var t = new TsxListingDownloader();
            var symbols = await t.Download(index);

            // Insert listing data
            foreach (var symbol in symbols)
            {
                DatabaseHelper.ExecuteNonQuery(string.Format("INSERT INTO listing (index, symbol) VALUES ({0}, {1})", index, symbol));
            }

            var data = DatabaseHelper.ExecuteQuery("SELECT * FROM listing");

        }
    }
}
