namespace DemoNetCacheRedis.Infra
{
    public class CacheOptions
    {
        public TimeSpan? SlidingExpiration { get; private set; }
        public TimeSpan? AbsoluteExpiration { get; private set; }

        public CacheOptions SetSlidingExpiration(TimeSpan slidingExpiration)
        {
            SlidingExpiration = slidingExpiration;
            return this;
        }

        public CacheOptions SetAbsoluteExpiration(TimeSpan absoluteExpiration)
        {
            AbsoluteExpiration = absoluteExpiration;
            return this;
        }

        public bool HasValueSlidingExpiration() => SlidingExpiration != null;
        public bool HasValueAbsoluteExpiration() => AbsoluteExpiration != null;
    } 
}