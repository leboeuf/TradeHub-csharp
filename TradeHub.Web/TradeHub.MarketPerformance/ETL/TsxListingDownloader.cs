using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TradeHub.MarketPerformance.ETL
{
    public class TsxListingDownloader
    {
        public async Task<List<string>> Download(string index)
        {
            var url = string.Format("http://web.tmxmoney.com/constituents_data.php?index={0}", index);

            var client = new WebClient();
            string data = await client.DownloadStringTaskAsync(url);
            client.Dispose();

            if (data.Length <= 0)
            {
                throw new Exception(string.Format("No listing for index: {0}", index));
            }

            var result = new List<string>();
            var lines = data.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int indLine = 4; indLine < lines.Length; indLine++) // indLine = 4 to skip header rows
            {
                var splitted = lines[indLine].Split(',');
                result.Add(splitted[1]);
            }

            return result;
        }
    }
}
