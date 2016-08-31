using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace TCCMarketPlace.Cache
{
    internal class InMemoryCache : ICache 
    {
        private static ObjectCache privateCache;
        private static object privateCacheLock = new object();

        public InMemoryCache()
        {
            privateCache = MemoryCache.Default;
        }

        /// <summary>
        /// Get cached item.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="key"> User provided cache key. </param>
        /// <returns> Cached object.</returns>
        public T GetItem<T>(string key) where T:class
        {
            return privateCache.Get(CacheUtils.GetCacheKey<T>(key)) as T;
        }

        /// <summary>
        /// Write an object to application cache.
        /// </summary>
        /// <typeparam name="T"> Object type.</typeparam>
        /// <param name="key"> User provider cache key.</param>
        /// <param name="value"> Object to be cached. </param>
        /// <param name="overwrite"> Enable overwrite flag.</param>
        public bool PutItem<T>(string key, object value, bool overwrite = true)
        {
            CacheItemPolicy policy = GetDefaultCachePolicy();
            return this.PutItem(CacheUtils.GetCacheKey<T>(key), value, policy, overwrite);
        }

        /// <summary>
        /// Write an object to application cache with lifetime.
        /// </summary>
        /// <typeparam name="T"> Object type.</typeparam>
        /// <param name="key"> User provided cache key. </param>
        /// <param name="value">Object to be cached.</param>
        /// <param name="timeSpan"> Cache lifetime. </param>
        /// <param name="overwrite"> Enable overwrite flag.</param>
        public bool PutItem<T>(string key, object value, TimeSpan timeSpan, bool overwrite = true)
        {
            CacheItemPolicy policy = GetCachePolicy(timeSpan);
            return this.PutItem(CacheUtils.GetCacheKey<T>(key), value, policy, overwrite);
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
            lock (privateCacheLock)
            {
                if (privateCache.Contains(key))
                {
                    privateCache.Remove(key);
                    returnVal = true;
                }
            }
            return returnVal;
        }

        /// <summary>
        /// Invalidate an existing cached object by object type.
        /// </summary>
        /// <typeparam name="T"> Object type.</typeparam>
        /// <param name="key"> Key.</param>
        public bool Invalidate<T>()
        {
            bool returnVal = false;
            string keyType = CacheUtils.GetCacheKey<T>();
            lock (privateCacheLock)
            {
                Flush(keyType);
                returnVal = true;
            }
            return returnVal;
        }

        /// <summary>
        /// Invalidate an existing cached object by cache prefix.
        /// </summary>
        public bool Invalidate()
        {
            bool returnVal = false;
            string key = CacheUtils.GetCacheKey();
            lock (privateCacheLock)
            {
                Flush(key);
                returnVal = true;
            }
            return returnVal;
        }

        private void Flush(string keyValue)
        {
            IEnumerable<string> keys = privateCache.Where(kv => kv.Key.StartsWith(keyValue))
                                        .Select(kv => kv.Key);
            Parallel.ForEach(keys, key => privateCache.Remove(key));
        }
        /// <summary>
        /// Gets cache settings with cache expiration time set as default cache lifetime.
        /// </summary>
        /// <param name="timeSpan"> Require cache lifetime.</param>
        /// <returns>Cache settings </returns>
        private CacheItemPolicy GetDefaultCachePolicy()
        {
            CacheItemPolicy defaultCachePolicy = new CacheItemPolicy();
            defaultCachePolicy.AbsoluteExpiration = DateTime.Now.AddMinutes(CacheUtils.DefaultCacheDuration);
            return defaultCachePolicy;
        }

        /// <summary>
        /// Gets cache settings with cache expiration time set as required by the caller.
        /// </summary>
        /// <param name="timeSpan"> Require cache lifetime.</param>
        /// <returns>Cache settings </returns>
        private CacheItemPolicy GetCachePolicy(TimeSpan timeSpan)
        {
            CacheItemPolicy defaultCachePolicy = new CacheItemPolicy();
            defaultCachePolicy.AbsoluteExpiration = DateTime.Now.Add(timeSpan);
            return defaultCachePolicy;
        }

        /// <summary>
        /// Writes object to application cache.
        /// </summary>
        private bool PutItem(string key, object value, CacheItemPolicy policy, bool overwrite)
        {
            bool retValue = false;
            lock (privateCacheLock)
            {
                if (privateCache.Contains(key) && overwrite)
                {
                    privateCache.Remove(key);
                }
                if (!privateCache.Contains(key))
                {
                    privateCache.Add(key, value, policy);
                    retValue = true;
                }
            }
            return retValue;
        }
    }
}
