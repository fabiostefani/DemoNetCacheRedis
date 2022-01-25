using DemoNetCacheRedis.Controllers.InputModels;
using DemoNetCacheRedis.Models;
using DemoNetCacheRedis.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace DemoNetCacheRedis.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriaController : ControllerBase
{
    private readonly ILogger<Categoria> _logger;
    private readonly ICategoriaRepositorio _categoriaRepositorio;
    

    public CategoriaController(ILogger<Categoria> logger,
                             ICategoriaRepositorio categoriaRepositorio)
    {
        _logger = logger;
        _categoriaRepositorio = categoriaRepositorio;
    }

    [HttpPost]
    public async Task<ActionResult> Post(CadastrarCategoriaImputModel imputModel)
    {
        var validacao = imputModel.EstaValido();
        if (!validacao.IsValid)
        {
            return BadRequest(validacao.Errors.Select(x => x.ErrorMessage));
        }
        var categoria = new Categoria(imputModel.Descricao);
        var sucesso = await _categoriaRepositorio.Adicionar(categoria);
        return sucesso ? Created(nameof(Get), categoria.Id) : BadRequest(imputModel);
    }

    [HttpGet()]
    [Route("{id:guid}")]
    public async Task<ActionResult> Get(Guid id)
    {
        return Ok(await _categoriaRepositorio.ObterPorId(id));
    }

    [HttpGet()]
    public async Task<ActionResult> Get()
    {
        return Ok(await _categoriaRepositorio.ObterTodos());
    }
}