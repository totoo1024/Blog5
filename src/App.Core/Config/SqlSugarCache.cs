using System;
using System.Collections.Generic;
using App.Framwork;
using EasyCaching.Core;
using SqlSugar;
namespace App.Core.Config
{
    /// <summary>
    /// 仅供ORM缓存使用
    /// </summary>
    internal class SqlSugarCache : ICacheService
    {
        private static readonly IEasyCachingProvider cache = Framwork.Storage.GetService<IEasyCachingProvider>();

        public void Add<V>(string key, V value)
        {
            cache.Set(key, value, TimeSpan.FromSeconds(int.MaxValue));
        }

        public void Add<V>(string key, V value, int cacheDurationInSeconds)
        {
            cache.Set(key, value, TimeSpan.FromSeconds(cacheDurationInSeconds));
        }

        public bool ContainsKey<V>(string key)
        {
            return cache.Exists(key);
        }

        public V Get<V>(string key)
        {
            return cache.Get<V>(key).Value;
        }

        public IEnumerable<string> GetAllKey<V>()
        {
            return cache.GetByPrefix<object>("SqlSugarDataCache.").Keys;
        }

        public void Remove<V>(string key)
        {
            cache.Remove(key);
        }

        public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = 2147483647)
        {
            if (cache.Exists(cacheKey))
            {
                return cache.Get<V>(cacheKey).Value;
            }

            V v = create();
            cache.Set(cacheKey, v, TimeSpan.FromSeconds(cacheDurationInSeconds));
            return v;
        }
    }
}