using Ardalis.SharedKernel;
using System;

namespace Ponto.Eletronico.Core.Entities;

public class RegitroPontos : EntityBase, IAggregateRoot
{
    public RegitroPontos(string email)
    {
        Email = email;
        Registro = DateTime.Now.ToUniversalTime();
    }

    public int Id { get; set; }

    public string Email { get; set; }
    
    public DateTime Registro { get; set; }
    
    public bool MudancaAutorizada { get; set; } = true;

    public void AutorizarMudanca()
    {
        MudancaAutorizada = true;
    }

}
