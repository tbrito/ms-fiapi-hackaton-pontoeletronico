namespace Ponto.Eletronico.Infrastructure;

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

public static class JwtAuthenticationExtensions
{
    public static AuthenticationBuilder AddJwtAuthentication(
        this AuthenticationBuilder builder,
        Action<JwtBearerOptions> configureOptions) =>
        builder.AddScheme<JwtBearerOptions, PontoAuthenticationHandler>(
            JwtBearerDefaults.AuthenticationScheme, configureOptions);
}
