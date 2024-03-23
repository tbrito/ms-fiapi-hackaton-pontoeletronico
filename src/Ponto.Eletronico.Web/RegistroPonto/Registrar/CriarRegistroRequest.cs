using System.Text.Json.Serialization;

namespace Ponto.Eletronico.Web.RegistroPonto.Registrar;

public class CriarRegistroRequest
{
    public readonly static string Route = "registro-ponto/registrar";

    public string? UserName { get; set; }
}
