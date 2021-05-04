namespace GitHubApp.Interface
{
    public interface ICacheService
    {
        T GetCached<T>(string cacheKey);
        void SetCache<T>(string cacheKey, T item);
    }
}
