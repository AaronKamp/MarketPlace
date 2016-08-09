using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Configuration;
using Extensions;

namespace TCCMarketPlace.Cache
{
    internal class RedisCache : ICache
    {        
        private static ConnectionMultiplexer connection;        
        private string connectionString = string.Empty;

        public RedisCache()
        {
            connectionString = ConfigurationManager.ConnectionStrings["AzureRedisCache"].ConnectionString;
            if (!string.IsNullOrEmpty(connectionString))
                connection = ConnectionMultiplexer.Connect(connectionString);
            else
                throw new Exception("Redis connection not configured");
        }

        public T GetItem<T>(string key) where T : class
        {
            key = CacheUtils.GetCacheKey<T>(key);
            IDatabase database = connection.GetDatabase();
            return database.StringGet(key).ToString().JsonDeserializeObject<T>();  
        }

        public bool PutItem<T>(string key, object value, bool overwrite = true)
        {
            return PutItem(CacheUtils.GetCacheKey<T>(key), value, GetDefaultCacheExpiry(), overwrite);
        }

        public bool PutItem<T>(string key, object value, TimeSpan timeSpan, bool overwrite = true)
        {
            return PutItem(CacheUtils.GetCacheKey<T>(key), value, timeSpan, overwrite);
        }

        public bool Invalidate<T>(string key)
        {
            bool returnVal = false;
            key = CacheUtils.GetCacheKey<T>(key);
            IDatabase database = connection.GetDatabase();
            if(database.KeyExists(key))
            {
                database.KeyDelete(key);
                returnVal = true;
            }
            return returnVal;
        }

        public bool Invalidate<T>()
        {
            string key = CacheUtils.GetCacheKey<T>();
            return Flush(key);
        }

        public bool Invalidate()
        {
            string key = CacheUtils.GetCacheKey();
            return Flush(key);
        }

        private bool Flush(string keyValue)
        {
            var endpoints = connection.GetEndPoints();
            IDatabase database = connection.GetDatabase();
            var transaction = database.CreateTransaction();
            foreach(var endpoint in endpoints)
            {
                var server = connection.GetServer(endpoint);
                var keys = server.Keys(pattern: keyValue + "*");
                Parallel.ForEach(keys, key => transaction.KeyDeleteAsync(key));
            }
            return transaction.Execute();
        }

        private bool PutItem(string key, object value, TimeSpan timespan, bool overwrite)
        {
            bool retValue = false;
            IDatabase database = connection.GetDatabase();
            if (!database.KeyExists(key) || overwrite)
            {             
                database.StringSet(key, value.JsonSerializeObject(), timespan);
                retValue = true;
            }            
            return retValue;
        }

        private TimeSpan GetDefaultCacheExpiry()
        {
            TimeSpan timespan = new TimeSpan(0, CacheUtils.DefaultCacheDuration, 0);
            return timespan;
        }
    }
}
