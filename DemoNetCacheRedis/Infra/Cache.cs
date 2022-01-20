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

        //([CanBeNullAttribute] Action<DbContextOptionsBuilder> optionsAction = null, ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TContext : DbContext;
        //class DbContextOptionsBuilder : IDbContextOptionsBuilderInfrastructure

        public async Task<T> GetAsync<T>(string key)
        {
            string cacheJson = await _cache.GetStringAsync(key);            
            if (string.IsNullOrEmpty(cacheJson)) return default(T);
            
            return JsonSerializer.Deserialize<T>(cacheJson);
        }

        public async Task SetAsync(string key, object value, TimeSpan absoluteExpirations)
        {
            DistributedCacheEntryOptions opcoesCache = new DistributedCacheEntryOptions();
            opcoesCache.SetAbsoluteExpiration(absoluteExpirations);            
            var cache = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, cache, opcoesCache);
        }

        public async Task SetTeste1Async(string key, object value, CacheConfig options = null)
        {
            DistributedCacheEntryOptions opcoesCache = new DistributedCacheEntryOptions();
            if (options != null)
            {
                opcoesCache.SetAbsoluteExpiration(options.AbsoluteExpiration.Value);                
                opcoesCache.SetSlidingExpiration(options.SlidingExpiration.Value);                
            }            
            var cache = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, cache, opcoesCache);
        }

        public async Task SetTeste2Async(string key, object value, Action<CacheConfig> options = null)
        {
            DistributedCacheEntryOptions opcoesCache = new DistributedCacheEntryOptions();            

            if (options != null)
            {
                var cfg = new CacheConfig();
                options(cfg);    
                if (cfg != null)
                {
                    opcoesCache.SetAbsoluteExpiration(cfg.AbsoluteExpiration.Value);                
                    if (cfg.HasValueSlidingExpiration()) opcoesCache.SetSlidingExpiration(cfg.SlidingExpiration.Value);                
                }            
            }
            
            var cache = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, cache, opcoesCache);
        }

        // public async Task Set(string key, object value, TimeSpan slidingExpiration)
        // {
        //     DistributedCacheEntryOptions opcoesCache = new DistributedCacheEntryOptions();
        //     opcoesCache.SetSlidingExpiration(slidingExpiration);            
        //     var cache = JsonSerializer.Serialize(value);
        //     await _cache.SetStringAsync(key, cache, opcoesCache);
        // }
    }

    public class CacheConfig
    {
        public TimeSpan? SlidingExpiration { get; private set; }
        public TimeSpan? AbsoluteExpiration { get; private set; }

        public CacheConfig SetSlidingExpiration(TimeSpan slidingExpiration)
        {
            SlidingExpiration = slidingExpiration;
            return this;
        }

        public CacheConfig SetAbsoluteExpiration(TimeSpan absoluteExpiration)
        {
            AbsoluteExpiration = absoluteExpiration;
            return this;
        }

        public bool HasValueSlidingExpiration() => SlidingExpiration != null;
    }

    
}