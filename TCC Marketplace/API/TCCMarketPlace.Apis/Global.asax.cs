using System;
using System.Web.Http;

namespace TCCMarketPlace.Apis
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(FilterConfig.Register);
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            
        }

    }
}
