﻿using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Ponto.Eletronico.Core.Interfaces;
using Ponto.Eletronico.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ponto.Eletronico.Infrastructure.Identity;
using Microsoft.Extensions.Configuration;
using Ponto.Eletronico.Infrastructure.Email;

namespace Ponto.Eletronico.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
      this IServiceCollection services,
      IConfiguration config,
      ILogger logger)
    {
        string? connectionString = config.GetConnectionString("DefaultConnection");
        Guard.Against.Null(connectionString);
        services.AddDbContext<AppDbContext>(options =>
         options.UseSqlServer(connectionString));

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
        services.AddScoped<IIdentityService, IdentityService>();

        services.Configure<MailserverConfiguration>(config.GetSection("Mailserver"));

        logger.LogInformation("{Project} services registered", "Infrastructure");

        return services;
    }
}
