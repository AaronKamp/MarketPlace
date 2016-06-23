using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace TCCMarketPlace.Apis.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetHeaderValue(this HttpRequest request, string name)
        {
            var values = request.Headers.GetValues(name);

            if (values!=null && values.Any())
            {
                return values.FirstOrDefault();
            }

            return null;
        }
    }
}