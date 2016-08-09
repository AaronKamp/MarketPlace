using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCMarketPlace.Cache
{
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
