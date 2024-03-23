using Ardalis.SharedKernel;

namespace Ponto.Eletronico.Core.Entities;

public class RegistroPonto : EntityBase, IAggregateRoot
{
  public RegistroPonto(string userName)
  {
    UserName = userName;
    Registro = DateTime.Now;
  }
  public string UserName { get; set; }
  public DateTime Registro { get; set; }
  public bool MudancaAutorizada { get; set; } = false;
  
  public void AutorizarMudanca()
  {
    MudancaAutorizada = true;
  }
  
}
