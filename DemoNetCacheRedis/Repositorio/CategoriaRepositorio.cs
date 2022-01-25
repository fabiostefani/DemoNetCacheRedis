using DemoNetCacheRedis.Data;
using DemoNetCacheRedis.Infra;
using DemoNetCacheRedis.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoNetCacheRedis.Repositorio;
public class CategoriaRepositorio : ICategoriaRepositorio
{
    const string KeyValueCache = "Categoria";
    private readonly MemoryCacheContext _ctx;    
    private readonly ICache _cache;

    public CategoriaRepositorio(MemoryCacheContext ctx,                              
                              ICache cache)
    {
        _cache = cache;
        _ctx = ctx;        
    }

    public async Task<bool> Adicionar(Categoria categoria)
    {
        _ctx.Set<Categoria>().Add(categoria);
        var success = await _ctx.SaveChangesAsync() > 0;
        if (success)
        {
            await _cache.InvalidateCache(KeyValueCache);
        }
        return success;
    }

    public async Task<Categoria> ObterPorId(Guid id)
    {
        string keyValueCategoriaId = $"{KeyValueCache}-{id.ToString()}";
        Categoria categoria = await _cache.GetAsync<Categoria>(keyValueCategoriaId);        
        if (categoria == null)
        {
            categoria = await _ctx.Set<Categoria>().FirstOrDefaultAsync(x => x.Id == id);
            
            await _cache.SetAsync(KeyValueCache, categoria, options =>
            {
                options.SetSlidingExpiration(TimeSpan.FromSeconds(10));
                options.SetAbsoluteExpiration(TimeSpan.FromSeconds(30));
            });

            Console.WriteLine("Recuperou do banco");
        }
        else
        {
            Console.WriteLine("Recuperou do Cache");
        }                
        return categoria;
    }

    public async Task<IEnumerable<Categoria>> ObterTodos()
    {
        IEnumerable<Categoria> categorias = await _cache.GetAsync<IEnumerable<Categoria>>(KeyValueCache);
        if (categorias == null || !categorias.Any())
        {
            categorias = await _ctx.Set<Categoria>().ToListAsync();
            await _cache.SetAsync(KeyValueCache, categorias, options =>
            {
                options.SetSlidingExpiration(TimeSpan.FromSeconds(10));
                options.SetAbsoluteExpiration(TimeSpan.FromSeconds(30));
            });
            Console.WriteLine("Recuperou do banco");
        }
        else
        {         
            Console.WriteLine("Recuperou do Cache");
        }
        return categorias;
    }
}