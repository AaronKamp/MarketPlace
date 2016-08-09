using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCMarketPlace.Cache
{
    internal class CacheUtils
    {
        private static int defaultCacheDuration = 30;

        internal static int DefaultCacheDuration
        {
            get
            {
                var duration = ConfigurationManager.AppSettings[CacheConstants.DURATION];
                Int32.TryParse(duration, out defaultCacheDuration);
                return defaultCacheDuration;
            }
        }

        private static string GetCacheKeyPrefix()
        {            
            string ClientName = ConfigurationManager.AppSettings[CacheConstants.CLIENT].ToString();
            string EnvironmentName = ConfigurationManager.AppSettings[CacheConstants.ENVIRONMENT].ToString();
            
            if (string.IsNullOrEmpty(ClientName) || string.IsNullOrEmpty(EnvironmentName))
                throw new Exception("Clientname/ Environment not configured for cache");

            return string.Concat(ClientName, "_", EnvironmentName);
        }
        internal static string GetCacheKey<T>(string key)
        {            
            string typeName = typeof(T).ToString();
            string[] redisKey = new string[] { GetCacheKeyPrefix(), "_", typeName, "_", key };
            return string.Concat(redisKey);
        }

        internal static string GetCacheKey<T>()
        {
            string typeName = typeof(T).ToString();
            string[] redisKey = new string[] { GetCacheKeyPrefix(), "_", typeName };
            return string.Concat(redisKey);
        }

        internal static string GetCacheKey()
        {
            return GetCacheKeyPrefix();
        }
    }
}
