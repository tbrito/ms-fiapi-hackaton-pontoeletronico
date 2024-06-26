﻿using Ardalis.Result;
using Ardalis.SharedKernel;
using Ponto.Eletronico.Core.Entities;
using Ponto.Eletronico.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Ponto.Eletronico.UseCases.RegistroPonto.RegistrarPonto;

public class CriarRegistroHandler(IRepository<Core.Entities.RegitroPontos> _repository, IServiceBusProducer serviceBusProducer)
  : ICommandHandler<CriarRegistroCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CriarRegistroCommand request, CancellationToken cancellationToken)
    {
        var registroPonto = request.ToModel();

        var createdItem = await _repository.AddAsync(registroPonto, cancellationToken);

        ///para publicar na pipeline
        await serviceBusProducer.SendAsync<RegitroPontos>(registroPonto, "grupo23-pontobatido");

        return createdItem is Core.Entities.RegitroPontos;
    }
}
