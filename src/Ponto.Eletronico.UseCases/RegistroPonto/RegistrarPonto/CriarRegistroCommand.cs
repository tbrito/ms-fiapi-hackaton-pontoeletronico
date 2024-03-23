using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Ponto.Eletronico.UseCases.RegistroPonto.RegistrarPonto;

public record CriarRegistroCommand : ICommand<Result<bool>>
{
  public CriarRegistroCommand(string email)
  {
    Email = email;
  }
  public string Email { get; private set; }
  public Core.Entities.RegistroPonto ToModel()
  {
    return new Core.Entities.RegistroPonto(Email);
  }
}

