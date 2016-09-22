using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Logger;
using Marketplace.Admin.App_Start;
using Marketplace.Admin.Filters;
//using Marketplace.Admin.App_Start;

namespace Marketplace.Admin
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
           // UnityConfig.RegisterComponents();
           Bootstrapper.Run();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var raisedException = Server.GetLastError();

            var exceptionIdentifier = Guid.NewGuid();

            LogManager.Instance.Log(CustomExceptionFilterAttribute.ComposeExceptionLog(raisedException, exceptionIdentifier),
                                        raisedException, LogLevelEnum.Error);

            System.Diagnostics.Trace.WriteLine(raisedException.ToString());

            
        }
    }
}
