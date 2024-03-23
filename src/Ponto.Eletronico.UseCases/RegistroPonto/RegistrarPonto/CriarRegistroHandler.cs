using Ardalis.Result;
using Ardalis.SharedKernel;
using System.Threading;
using System.Threading.Tasks;

namespace Ponto.Eletronico.UseCases.RegistroPonto.RegistrarPonto;

public class CriarRegistroHandler(IRepository<Core.Entities.RegitroPontos> _repository)
  : ICommandHandler<CriarRegistroCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CriarRegistroCommand request,
      CancellationToken cancellationToken)
    {
        var registroPonto = request.ToModel();

        var createdItem = await _repository.AddAsync(registroPonto, cancellationToken);

        return createdItem is Core.Entities.RegitroPontos;
    }
}
