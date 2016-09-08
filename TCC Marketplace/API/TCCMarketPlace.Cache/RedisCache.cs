using System;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Configuration;
using Extensions;
using TCCMarketPlace.Model.ExceptionHandlers;
using System.Net;

namespace TCCMarketPlace.Cache
{
    /// <summary>
    /// Handles Redis cache
    /// </summary>
    internal class RedisCache : ICache
    {
        private static ConnectionMultiplexer connection;
        private string connectionString = string.Empty;

        /// <summary>
        /// Constructor sets Redis cache connection string.
        /// </summary>
        public RedisCache()
        {
            connectionString = ConfigurationManager.ConnectionStrings["AzureRedisCache"].ConnectionString;
            if (!string.IsNullOrEmpty(connectionString))
            {

                // try and get maxRetryCount, maxDelay from Application settings. If not possible default value is taken. 
                int maxRetry, maxTimeout;
                maxRetry = int.TryParse(ConfigurationManager.AppSettings[CacheConstants.RETRYCOUNT], out maxRetry) ? maxRetry : 3;
                maxTimeout = int.TryParse(ConfigurationManager.AppSettings[CacheConstants.TIMEOUT], out maxTimeout) ? maxTimeout : 2000;

                var configOptions = new ConfigurationOptions
                {
                    ConnectRetry = maxRetry,
                    ConnectTimeout = maxTimeout  // The maximum waiting time (ms), not the delay for retries.
                };
              
                var redisConnectionWithConfig = connectionString + "," + configOptions.ToString();

                connection = ConnectionMultiplexer.Connect(redisConnectionWithConfig);
            }
            else
                throw new Exception("Redis connection not configured");
        }

        /// <summary>
        /// Get cached item by key.
        /// </summary>
        /// <typeparam name="T"> Object type. </typeparam>
        /// <param name="key"> Key.  </param>
        /// <returns> Caches object. </returns>
        public T GetItem<T>(string key) where T : class
        {
            try
            {
                key = CacheUtils.GetCacheKey<T>(key);
                IDatabase database = connection.GetDatabase();
                return database.StringGet(key).ToString().JsonDeserializeObject<T>();
            }
            catch (Exception ex)
            {
                throw new RedisCacheException("Redis exception occurred while reading from cache.", ex);
            }
        }

        /// <summary>
        /// Cache an object by key
        /// </summary>
        /// <typeparam name="T"> Object type.</typeparam>
        /// <param name="key">Key. </param>
        /// <param name="value"> Object to cache.</param>
        /// <param name="overwrite"> Enable overwrite flag. </param>
        /// <returns></returns>
        public bool PutItem<T>(string key, object value, bool overwrite = true)
        {
            return PutItem(CacheUtils.GetCacheKey<T>(key), value, GetDefaultCacheExpiry(), overwrite);
        }

        /// <summary>
        /// Cache an  object by key and lifetime.
        /// </summary>
        /// <typeparam name="T"> Object type. </typeparam>
        /// <param name="key"> Key. </param>
        /// <param name="value"> Object to cache. </param>
        /// <param name="timeSpan"> Lifetime of the cached object.</param>
        /// <param name="overwrite">  Enable overwrite flag. </param>
        /// <returns></returns>
        public bool PutItem<T>(string key, object value, TimeSpan timeSpan, bool overwrite = true)
        {
            return PutItem(CacheUtils.GetCacheKey<T>(key), value, timeSpan, overwrite);
        }

        /// <summary>
        /// Invalidate an existing cached object by key.
        /// </summary>
        /// <typeparam name="T"> Object type.</typeparam>
        /// <param name="key"> Key.</param>
        public bool Invalidate<T>(string key)
        {
            bool returnVal = false;
            key = CacheUtils.GetCacheKey<T>(key);
            IDatabase database = connection.GetDatabase();
            if (database.KeyExists(key))
            {
                database.KeyDelete(key);
                returnVal = true;
            }
            return returnVal;
        }

        /// <summary>
        /// Invalidate an existing cached object by object type.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        public bool Invalidate<T>()
        {
            string key = CacheUtils.GetCacheKey<T>();
            return Flush(key);
        }

        /// <summary>
        /// Invalidate an existing cached object by cache prefix.
        /// </summary>
        public bool Invalidate()
        {
            string key = CacheUtils.GetCacheKey();
            return Flush(key);
        }

        /// <summary>
        /// Remove the specified cache from cache storage.
        /// </summary>
        /// <param name="keyValue"> Key. </param>
        private bool Flush(string keyValue)
        {
            var endpoints = connection.GetEndPoints();
            IDatabase database = connection.GetDatabase();
            var transaction = database.CreateTransaction();
            foreach (var endpoint in endpoints)
            {
                var server = connection.GetServer(endpoint);
                var keys = server.Keys(pattern: keyValue + "*");
                Parallel.ForEach(keys, key => transaction.KeyDeleteAsync(key));
            }
            return transaction.Execute();
        }

        /// <summary>
        /// Cache an  object by key and lifetime.
        /// </summary>
        private bool PutItem(string key, object value, TimeSpan timespan, bool overwrite)
        {
            try {
                bool retValue = false;
                IDatabase database = connection.GetDatabase();
                if (!database.KeyExists(key) || overwrite)
                {
                    database.StringSet(key, value.JsonSerializeObject(), timespan);
                    retValue = true;
                }
                return retValue;
            }
            catch(Exception ex)
            {
                throw new RedisCacheException("Redis exception occurred while writing to cache.", ex);
            }
        }

        /// <summary>
        /// Gets the default cache expiry.
        /// </summary>
        private TimeSpan GetDefaultCacheExpiry()
        {
            TimeSpan timespan = new TimeSpan(0, CacheUtils.DefaultCacheDuration, 0);
            return timespan;
        }
    }
}
