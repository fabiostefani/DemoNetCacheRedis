using DemoNetCacheRedis.Models;
using FluentValidation;
using FluentValidation.Results;

namespace DemoNetCacheRedis.Controllers.InputModels;

public class CadastrarCategoriaImputModel
{
    public string Descricao { get; set; }
    public ValidationResult EstaValido()
    {
        return new CadastrarCategoriaImputModelValidation().Validate(this);
    }
}

public class CadastrarCategoriaImputModelValidation : AbstractValidator<CadastrarCategoriaImputModel>
{
    public CadastrarCategoriaImputModelValidation()
    {
        RuleFor(c => c.Descricao)
        .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
        .Length(Categoria.TamanhoMinimo, Categoria.TamanhoMaximoDescricao).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
        
    }
}