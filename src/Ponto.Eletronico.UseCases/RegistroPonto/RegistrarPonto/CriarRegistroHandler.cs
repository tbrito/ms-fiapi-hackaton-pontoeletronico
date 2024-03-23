using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Ponto.Eletronico.UseCases.RegistroPonto.RegistrarPonto;

public class CriarRegistroHandler(IRepository<Core.Entities.RegistroPonto> _repository)
  : ICommandHandler<CriarRegistroCommand, Result<bool>>
{
  public async Task<Result<bool>> Handle(CriarRegistroCommand request,
    CancellationToken cancellationToken)
  {
    var registroPonto = request.ToModel();
    
    var createdItem = await _repository.AddAsync(registroPonto, cancellationToken);

    return createdItem.IsAssignableFrom<Core.Entities.RegistroPonto>();
  }
}
