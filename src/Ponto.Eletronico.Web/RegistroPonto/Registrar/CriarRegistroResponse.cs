namespace Ponto.Eletronico.Web.RegistroPonto.Registrar;

public class CriarRegistroResponse(bool success)
{
  public bool Success { get; set; } = success;
  public string Message { get; set; } = "Marcação realizada com sucesso";
}

