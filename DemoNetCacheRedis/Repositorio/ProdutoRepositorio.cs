using System.Text.Json;
using DemoNetCacheRedis.Data;
using DemoNetCacheRedis.Infra;
using DemoNetCacheRedis.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace DemoNetCacheRedis.Repositorio;
public class ProdutoRepositorio : IProdutoRepositorio
{
    const string KeyValueCache = "Produto";
    private readonly MemoryCacheContext _ctx;
    private readonly IDistributedCache _distributedCache;
    
    public ProdutoRepositorio(MemoryCacheContext ctx,
                              IDistributedCache distributedCache)
    {
        _ctx = ctx;
        _distributedCache = distributedCache;
    }

    public async Task<bool> Adicionar(Produto produto)
    {
        _ctx.Set<Produto>().Add(produto);
        return await _ctx.SaveChangesAsync() > 0;        
    }

    public async Task<Produto> ObterPorId(Guid id)
    {
        string keyValueProdutoId = $"{KeyValueCache}-{id.ToString()}";
        string produtoJson = await _distributedCache.GetStringAsync(keyValueProdutoId);
        Produto produto;
        if (string.IsNullOrEmpty(produtoJson))
        {
            produto = await _ctx.Set<Produto>().FirstOrDefaultAsync(x => x.Id == id);
            DistributedCacheEntryOptions opcoesCache = new DistributedCacheEntryOptions();
            opcoesCache.SetSlidingExpiration(TimeSpan.FromSeconds(10));
            opcoesCache.SetAbsoluteExpiration(TimeSpan.FromSeconds(30));            
            produtoJson = JsonSerializer.Serialize(produto);
            await _distributedCache.SetStringAsync(keyValueProdutoId, produtoJson, opcoesCache);
            Console.WriteLine("Recuperou do banco");
        }
        else
        {
            produto = JsonSerializer.Deserialize<Produto>(produtoJson);
            Console.WriteLine("Recuperou do Cache");
        }                
        return produto;
    }

    public async Task<IEnumerable<Produto>> ObterTodos()
    {
        string produtosJson = await _distributedCache.GetStringAsync(KeyValueCache);
        IEnumerable<Produto> produtos;
        if (string.IsNullOrEmpty(produtosJson))        
        {
            produtos = await _ctx.Set<Produto>().ToListAsync();

            DistributedCacheEntryOptions opcoesCache = new DistributedCacheEntryOptions();
            opcoesCache.SetSlidingExpiration(TimeSpan.FromSeconds(10));
            opcoesCache.SetAbsoluteExpiration(TimeSpan.FromSeconds(30));            
            produtosJson = JsonSerializer.Serialize(produtos);
            await _distributedCache.SetStringAsync(KeyValueCache, produtosJson, opcoesCache);
            Console.WriteLine("Recuperou do banco");
        }
        else
        {      
            produtos = JsonSerializer.Deserialize<IEnumerable<Produto>>(produtosJson);   
            Console.WriteLine("Recuperou do Cache");
        }
        return produtos;
    }
}