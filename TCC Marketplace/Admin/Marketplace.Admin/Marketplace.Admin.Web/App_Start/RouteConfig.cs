using System.Web.Mvc;
using System.Web.Routing;

namespace Marketplace.Admin
{
    /// <summary>
    /// Handles default routes.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Register default routes.
        /// </summary>
        /// <param name="routes">RouteCollection</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Services", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
