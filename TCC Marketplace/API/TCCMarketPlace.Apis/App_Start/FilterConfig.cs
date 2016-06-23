using System.Web.Http;
using TCCMarketPlace.Apis.Filters;

namespace TCCMarketPlace.Apis
{
    public class FilterConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var filters = config.Filters;
            filters.Add(new CustomExceptionFilterAttribute());
        }
    }
}
