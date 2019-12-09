using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WatsonTruckV2.Startup))]
namespace WatsonTruckV2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
