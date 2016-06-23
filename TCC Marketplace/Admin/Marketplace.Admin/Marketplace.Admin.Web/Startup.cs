using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Marketplace.Admin.Startup))]
namespace Marketplace.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
