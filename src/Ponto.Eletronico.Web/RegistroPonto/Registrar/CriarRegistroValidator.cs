using Ponto.Eletronico.Infrastructure.Data.Config;
using FastEndpoints;
using FluentValidation;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Ponto.Eletronico.Core.Interfaces;

namespace Ponto.Eletronico.Web.RegistroPonto.Registrar;

/// <summary>
/// See: https://fast-endpoints.com/docs/validation
/// </summary>
public class CriarRegistroValidator : Validator<CriarRegistroRequest>
{
  public CriarRegistroValidator(IIdentityService identityService)
  {
  }
}
