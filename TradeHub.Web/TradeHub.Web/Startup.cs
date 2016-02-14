using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TradeHub.Web.Startup))]
namespace TradeHub.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
