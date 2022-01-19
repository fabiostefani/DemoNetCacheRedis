using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace DemoNetCacheRedis.Infra
{
    public class Cache : ICache
    {
        private readonly IDistributedCache _cache;
        public Cache(IDistributedCache cache)
        {
            _cache = cache;

        }
        public async Task<T> Get<T>(string key)
        {
            string cacheJson = await _cache.GetStringAsync(key);            
            if (string.IsNullOrEmpty(cacheJson)) return default(T);
            
            return JsonSerializer.Deserialize<T>(cacheJson);
        }

        public async Task Set(string key, object value, TimeSpan absoluteExpirations)
        {
            DistributedCacheEntryOptions opcoesCache = new DistributedCacheEntryOptions();
            opcoesCache.SetAbsoluteExpiration(TimeSpan.FromSeconds(10));
            opcoesCache.SetAbsoluteExpiration(absoluteExpirations);            
            var cache = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, cache, opcoesCache);
        }
    }
}