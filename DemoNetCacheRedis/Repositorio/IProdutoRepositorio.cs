using DemoNetCacheRedis.Models;

namespace DemoNetCacheRedis.Repositorio;

public interface IProdutoRepositorio
{
    Task<bool> Adicionar(Produto produto);
    Task<Produto> ObterPorId(Guid id);
    Task<IEnumerable<Produto>> ObterTodos();
}