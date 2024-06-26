﻿using FastEndpoints;
using MediatR;
using Ponto.Eletronico.Core.Interfaces;
using Ponto.Eletronico.UseCases.RegistroPonto.RegistrarPonto;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Ponto.Eletronico.Web.RegistroPonto.Registrar;

/// <summary>
/// Criar Novo Registro
/// </summary>
/// <remarks>
/// Criar um novo registro de ponto.
/// </remarks>
/// , IIdentityService service
public class Registrar(IMediator _mediator) : Endpoint<CriarRegistroRequest, CriarRegistroResponse>
{
    public override void Configure()
    {
        Post(CriarRegistroRequest.Route);
        Summary(s =>
        {
            // XML Docs are used by default but are overridden by these properties:
            //s.Summary = "Criar Novo registro.";
            //s.Description = "Criar Novo registro.";
            s.ExampleRequest = new CriarRegistroRequest();
        });

    }

    public override async Task HandleAsync(
      CriarRegistroRequest request,
      CancellationToken ct)
    {
        if (User.Identity?.IsAuthenticated == false)
        {
            Debug.Print("Problema");
        }

        var claimValue = User.Claims.FirstOrDefault(claim => claim.Type.Contains(ClaimTypes.Email));
        
        var result = await _mediator.Send(new CriarRegistroCommand(claimValue.Value), ct);

        if (result.IsSuccess)
        {
            Response = new CriarRegistroResponse(result.Value);
            return;
        }
    }
}
