namespace DemoNetCacheRedis.Infra
{
    public interface ICache
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync(string key, object value, Action<CacheOptions> options);        
    }
}