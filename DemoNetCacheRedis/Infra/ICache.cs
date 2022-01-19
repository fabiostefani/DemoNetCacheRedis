namespace DemoNetCacheRedis.Infra
{
    public interface ICache
    {
        Task<T> Get<T>(string key);
        Task Set(string key, object value, TimeSpan absoluteExpirations);
    }
}