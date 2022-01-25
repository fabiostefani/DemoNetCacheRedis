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
        
        public async Task<T> GetAsync<T>(string key)
        {
            string cacheJson = await _cache.GetStringAsync(key);            
            if (string.IsNullOrEmpty(cacheJson)) return default(T);            
            return JsonSerializer.Deserialize<T>(cacheJson);
        }

        public async Task SetAsync(string key, object value, Action<CacheOptions> options)
        {
            if (options == null) throw new Exception("Opções de cache não configuradas.");            
            DistributedCacheEntryOptions opcoesCache = SetConfigCache(options);
            var cache = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, cache, opcoesCache);
        }

        private static DistributedCacheEntryOptions SetConfigCache(Action<CacheOptions> options)
        {
            var opcoesCache = new DistributedCacheEntryOptions();                        
            var cfg = new CacheOptions();
            options(cfg);
            if (cfg == null) return opcoesCache;
            if (cfg.HasValueAbsoluteExpiration()) opcoesCache.SetAbsoluteExpiration(cfg.AbsoluteExpiration.Value);
            if (cfg.HasValueSlidingExpiration()) opcoesCache.SetSlidingExpiration(cfg.SlidingExpiration.Value);
            return opcoesCache;
        }

        public async Task InvalidateCache(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }   
}