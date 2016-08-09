using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using System.Configuration;

namespace TCCMarketPlace.Cache
{
    internal class InMemoryCache : ICache 
    {
        private static ObjectCache privateCache;
        private static Object privateCacheLock = new object();

        public InMemoryCache()
        {
            privateCache = MemoryCache.Default;
        }

        public T GetItem<T>(string key) where T:class
        {
            return privateCache.Get(CacheUtils.GetCacheKey<T>(key)) as T;
        }

        public bool PutItem<T>(string key, object value, bool overwrite = true)
        {
            CacheItemPolicy policy = GetDefaultCachePolicy();
            return this.PutItem(CacheUtils.GetCacheKey<T>(key), value, policy, overwrite);
        }

        public bool PutItem<T>(string key, object value, TimeSpan timeSpan, bool overwrite = true)
        {
            CacheItemPolicy policy = GetCachePolicy(timeSpan);
            return this.PutItem(CacheUtils.GetCacheKey<T>(key), value, policy, overwrite);
        }

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

        private CacheItemPolicy GetDefaultCachePolicy()
        {
            CacheItemPolicy defaultCachePolicy = new CacheItemPolicy();
            defaultCachePolicy.AbsoluteExpiration = DateTime.Now.AddMinutes(CacheUtils.DefaultCacheDuration);
            return defaultCachePolicy;
        }

        private CacheItemPolicy GetCachePolicy(TimeSpan timeSpan)
        {
            CacheItemPolicy defaultCachePolicy = new CacheItemPolicy();
            defaultCachePolicy.AbsoluteExpiration = DateTime.Now.Add(timeSpan);
            return defaultCachePolicy;
        }

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
