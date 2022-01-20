namespace DemoNetCacheRedis.Infra
{
    public interface ICache
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync(string key, object value, TimeSpan absoluteExpirations);
        Task SetTeste1Async(string key, object value, CacheConfig options = null);
        Task SetTeste2Async(string key, object value, Action<CacheConfig> options);
    }
}