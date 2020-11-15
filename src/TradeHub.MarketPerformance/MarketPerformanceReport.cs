using System.Threading.Tasks;
using TradeHub.Core.Model.Enums.Indices;
using TradeHub.Database;
using TradeHub.MarketPerformance.ETL;
using System.Diagnostics;

namespace TradeHub.MarketPerformance
{
    public class MarketPerformanceReport
    {
        private readonly TsxListingDownloader _tsxListingDownloader = new TsxListingDownloader();

        public async Task Run()
        {
            // Create database table
            await DatabaseHelper.ExecuteNonQuery("DROP TABLE IF EXISTS listing");
            await DatabaseHelper.ExecuteNonQuery("CREATE TABLE listing (indice TEXT, symbol TEXT)"); // Cannot use reserved word "index"

            // Download listing
            var s = new Stopwatch();
            s.Start();
            var tasks = new Task[Tsx.Capped.Length];
            for (int i = 0; i < Tsx.Capped.Length; i++)
            {
                var index = Tsx.Capped[i];
                tasks[i] = DownloadAndInsertConstituents(index);
            }

            await Task.WhenAll(tasks);
            s.Stop();

            var data = await DatabaseHelper.ExecuteQuery("SELECT * FROM listing");

        }

        private async Task DownloadAndInsertConstituents(string index)
        {
            var symbols = await _tsxListingDownloader.Download(index);

            // Insert listing data
            foreach (var symbol in symbols)
            {
                await DatabaseHelper.ExecuteNonQuery(string.Format("INSERT INTO listing (indice, symbol) VALUES (\"{0}\", \"{1}\")", index, symbol)); // TODO: bind parameters
            }
        }
    }
}
