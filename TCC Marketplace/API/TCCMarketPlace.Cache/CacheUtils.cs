using System;
using System.Configuration;

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
        /// <summary>
        /// Returns Cache key prefix.
        /// </summary>
        private static string GetCacheKeyPrefix()
        {            
            string ClientName = ConfigurationManager.AppSettings[CacheConstants.CLIENT].ToString();
            string EnvironmentName = ConfigurationManager.AppSettings[CacheConstants.ENVIRONMENT].ToString();
            
            if (string.IsNullOrEmpty(ClientName) || string.IsNullOrEmpty(EnvironmentName))
                throw new Exception("Clientname/ Environment not configured for cache");

            return string.Concat(ClientName, "_", EnvironmentName);
        }
        /// <summary>
        /// Returns Actual cache key stored.
        /// </summary>
        /// <typeparam name="T"> object type. </typeparam>
        /// <param name="key">User specified key. </param>
        /// <returns>Actual cache key stored.</returns>
        internal static string GetCacheKey<T>(string key)
        {            
            string typeName = typeof(T).ToString();
            string[] cacheKey = new string[] { GetCacheKeyPrefix(), "_", typeName, "_", key };
            return string.Concat(cacheKey);
        }
        /// <summary>
        /// Returns Actual cache key stored
        /// </summary>
        /// <typeparam name="T"> Object type.</typeparam>
        /// <returns>Actual cache key stored</returns>
        internal static string GetCacheKey<T>()
        {
            string typeName = typeof(T).ToString();
            string[] cacheKey = new string[] { GetCacheKeyPrefix(), "_", typeName };
            return string.Concat(cacheKey);
        }
        /// <summary>
        /// Returns cActual cache key stored.
        /// </summary>
        /// <returns>Actual cache key stored.</returns>
        internal static string GetCacheKey()
        {
            return GetCacheKeyPrefix();
        }
    }
}
