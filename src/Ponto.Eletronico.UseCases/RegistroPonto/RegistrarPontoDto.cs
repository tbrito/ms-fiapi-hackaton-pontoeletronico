using System;

namespace Ponto.Eletronico.UseCases.RegistroPonto;

public record RegistrarPontoDto(Guid idUsuario, DateTime registro) { }
