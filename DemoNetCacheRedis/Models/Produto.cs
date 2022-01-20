namespace DemoNetCacheRedis.Models;

public class Produto
{
    public const int TamanhoTextoMinimo = 3;
    public const int TamanhoNome = 200;
    public const int TamanhoDepartamento = 1000;

    public Produto(string nome, string departamento, decimal preco)
    {
        this.Id = Guid.NewGuid();
        this.Nome = nome;
        this.Departamento = departamento;
        this.Preco = preco;
    }

    public Produto ( ) { }
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Departamento { get; set; }
    public decimal Preco { get; set; } 
}