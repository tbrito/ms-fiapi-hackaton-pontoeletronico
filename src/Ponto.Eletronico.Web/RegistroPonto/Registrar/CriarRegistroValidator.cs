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
    /// <summary>
    /// Initializes a new instance of the <see cref="CriarRegistroValidator"/> class.
    /// IIdentityService identityService
    /// </summary>
    public CriarRegistroValidator()
  {
  }
}
