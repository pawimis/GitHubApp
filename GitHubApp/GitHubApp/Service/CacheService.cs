using GitHubApp.Interface;

using MonkeyCache.FileStore;

using System;

namespace GitHubApp.Service
{
    public class CacheService : ICacheService
    {
        public T GetCached<T>(string cacheKey)
        {
            return Barrel.Current.Get<T>(key: cacheKey);
        }

        public void SetCache<T>(string cacheKey, T item)
        {
            Barrel.Current.Add<T>(cacheKey, item, new TimeSpan(72, 0, 0));
        }


    }
}
