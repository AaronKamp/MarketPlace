using System;

namespace TCCMarketPlace.Cache
{
    public interface ICache
    {
        T GetItem<T>(string key) where T:class;        

        bool PutItem<T>(string key, object value, bool overwrite = true);

        bool PutItem<T>(string key, object value, TimeSpan timeSpan, bool overwrite = true);

        bool Invalidate<T>(string key);

        bool Invalidate<T>();

        bool Invalidate();
    }
}
