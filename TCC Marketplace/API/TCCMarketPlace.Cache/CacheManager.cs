using System;
using System.Configuration;


namespace TCCMarketPlace.Cache
{
    /// <summary>
    /// Cache manager
    /// </summary>
    public class CacheManager
    {        
        private static ICache instance = null;
        private static object instanceLock = new object();

        private CacheManager()
        { }

        public static ICache Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (instanceLock)
                    {
                        if(instance == null)
                        {
                            instance = GetCacheInstance();
                        }
                    }
                }
                return instance;
            }
        }
        /// <summary>
        /// Returns new Redis or InMemoryCache instance 
        /// </summary>
        private static ICache GetCacheInstance()
        {
            string cache = ConfigurationManager.AppSettings[CacheConstants.MODE] as string;
            CacheTypeEnum cacheType = CacheTypeEnum.InProcess;
            Enum.TryParse<CacheTypeEnum>(cache, true, out cacheType);
            switch(cacheType)
            {
                case CacheTypeEnum.InProcess: return new InMemoryCache();
                case CacheTypeEnum.Distributed: return new RedisCache(); 
                default: return new InMemoryCache();
            }
        }
    }
}
