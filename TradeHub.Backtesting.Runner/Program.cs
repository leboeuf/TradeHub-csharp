using System.Threading.Tasks;
using TradeHub.StockScreener;

namespace TradeHub.Backtesting.Runner
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Get symbols
            var stockScreener = new Screener();
            var stocks = await stockScreener.Find();
        }
    }
}
