using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luo.Core.Common
{
    public static class CacheHelper
    {
        public static IDistributedCache _Cache;
        private static double _CacheExpirationTimeSeconds;
        public static void AddDistributedCacheSteup(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();//分布式内存缓存
            _Cache = services.BuildServiceProvider().GetService<IDistributedCache>();
            _CacheExpirationTimeSeconds = Appsettings.GetValue("CacheExpirationSeconds").ObjToMoney();
            if (_CacheExpirationTimeSeconds <= 0) 
            {
                _CacheExpirationTimeSeconds = 1000 * 60 * 5;
            }
        }
       
        public static void SetCache<T>(string key, T obj) where T : new()
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(_CacheExpirationTimeSeconds)
            };
            _Cache.Set(key, obj.ObjToBytesArray(), options);
        }
        public static void SetCache(string key, object obj)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(_CacheExpirationTimeSeconds)
            };
            _Cache.Set(key, obj.ObjToBytesArray(), options);
        }

        public static T GetCache<T>(string key)
        {
            var byteArray = _Cache.Get(key);
            return byteArray.ByteArrayToObject<T>();
        }
    }
}
