using System.Threading.Tasks;
using System.Web.Mvc;
using TradeHub.MarketPerformance;
using TradeHub.MarketPerformance.ETL;

namespace TradeHub.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async Task<ActionResult> TestMarketPerformanceReport()
        {
            var m = new MarketPerformanceReport();
            await m.Run();
            return null;
        }
    }
}