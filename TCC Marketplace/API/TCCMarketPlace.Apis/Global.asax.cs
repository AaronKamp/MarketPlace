using System.Web.Http;

namespace TCCMarketPlace.Apis
{
    /// <summary>
    /// Global.asax
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Application_Start event. This is fired on application start and initiates application configuration
        /// </summary>
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(FilterConfig.Register);
            SwaggerConfig.Register();
        }

    }
}
