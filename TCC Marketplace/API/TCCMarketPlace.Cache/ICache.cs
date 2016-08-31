using System;

namespace TCCMarketPlace.Cache
{
    public interface ICache
    {
        /// <summary>
        /// Gets caches item by key.
        /// </summary>
        T GetItem<T>(string key) where T:class;        
        /// <summary>
        /// Write to cache with cache duration as default value.
        /// </summary>
        bool PutItem<T>(string key, object value, bool overwrite = true);
        /// <summary>
        /// Write to cache with cache duration as requested by the caller.
        /// </summary>
        bool PutItem<T>(string key, object value, TimeSpan timeSpan, bool overwrite = true);
        /// <summary>
        /// Invalidate cache by key.
        /// </summary>
        bool Invalidate<T>(string key);
        /// <summary>
        /// Invalidate cache by object type.
        /// </summary>
        bool Invalidate<T>();
        /// <summary>
        /// Invalidate cache by cache prefix.
        /// </summary>
        bool Invalidate();
    }
}
