using Marketplace.Admin.Filters;
using System.Web.Mvc;

namespace Marketplace.Admin
{
    /// <summary>
    /// Handles Filter configurations.
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Register global filters.
        /// </summary>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomExceptionFilterAttribute());
        }
    }
}
