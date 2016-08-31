using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Marketplace.Admin.Startup))]
namespace Marketplace.Admin
{
    /// <summary>
    /// Owin startup.
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Configurations.
        /// </summary>
        public void Configuration(IAppBuilder app)
        {
            // Configure authorization.
            ConfigureAuth(app);
        }
    }
}
