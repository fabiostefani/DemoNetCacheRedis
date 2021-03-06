using Microsoft.EntityFrameworkCore;

namespace DemoNetCacheRedis.Data
{
    public class MemoryCacheContext : DbContext
    {
        public MemoryCacheContext(DbContextOptions<MemoryCacheContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MemoryCacheContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}