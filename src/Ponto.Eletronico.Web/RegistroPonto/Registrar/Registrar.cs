using FastEndpoints;
using MediatR;
using Ponto.Eletronico.Core.Interfaces;
using Ponto.Eletronico.UseCases.RegistroPonto.RegistrarPonto;

namespace Ponto.Eletronico.Web.RegistroPonto.Registrar;

/// <summary>
/// Criar Novo Registro
/// </summary>
/// <remarks>
/// Criar um novo registro de ponto.
/// </remarks>
public class Registrar(IMediator _mediator, IIdentityService service) : Endpoint<CriarRegistroRequest, CriarRegistroResponse>
{
  public override void Configure()
  {
    Post(CriarRegistroRequest.Route);
    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      //s.Summary = "Criar Novo registro.";
      //s.Description = "Criar Novo registro.";
      s.ExampleRequest = new CriarRegistroRequest() ;
    });
    
  }

  public override async Task HandleAsync(
    CriarRegistroRequest request,
    CancellationToken ct)
  {
    var result = await _mediator.Send(new CriarRegistroCommand(service.GetUserName()!), ct);

    if (result.IsSuccess)
    {
      Response = new CriarRegistroResponse(result.Value);
      return;
    }
  }
}
