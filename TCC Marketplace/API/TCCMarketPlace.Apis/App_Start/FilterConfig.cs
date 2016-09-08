using System.Web.Http;
using TCCMarketPlace.Apis.Filters;

namespace TCCMarketPlace.Apis
{
    /// <summary>
    /// Filters configuration.
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Register custom filters.
        /// </summary>
        public static void Register(HttpConfiguration config)
        {
            var filters = config.Filters;
            filters.Add(new CustomExceptionFilterAttribute());
            filters.Add(new LogActionFilter());

        }
    }
}
