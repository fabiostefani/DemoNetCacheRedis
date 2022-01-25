using DemoNetCacheRedis.Models;

namespace DemoNetCacheRedis.Repositorio;

public interface ICategoriaRepositorio
{
    Task<bool> Adicionar(Categoria categoria);
    Task<Categoria> ObterPorId(Guid id);
    Task<IEnumerable<Categoria>> ObterTodos();
}