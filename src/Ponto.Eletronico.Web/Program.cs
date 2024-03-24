using Ardalis.ListStartupServices;
using Ardalis.SharedKernel;
using FastEndpoints;
using FastEndpoints.Swagger;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ponto.Eletronico.Core.Entities;
using Ponto.Eletronico.Core.Interfaces;
using Ponto.Eletronico.Infrastructure.Email;
using Ponto.Eletronico.UseCases.RegistroPonto.RegistrarPonto;
using Ponto.Eletronico.Web.Settings;
using Serilog;
using Serilog.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection;
using Ponto.Eletronico.Infrastructure;
using Ponto.Eletronico.Infrastructure.Identity;
using Ponto.Eletronico.Infrastructure.Messaging;


var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

logger.Information("Starting web host");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));
var microsoftLogger = new SerilogLoggerFactory(logger)
    .CreateLogger<Program>();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});


builder.Services.AddFastEndpoints()
                .SwaggerDocument(o =>
                {
                    o.ShortSchemaNames = true;
                });

ConfigureMediatR();

builder.Services.AddInfrastructureServices(builder.Configuration, microsoftLogger);
builder.Services.AddKeyCloackAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddServiceBus(builder.Configuration);

builder.Services.AddScoped<IEmailSender, MimeKitEmailSender>();

if (builder.Environment.IsDevelopment())
{
    AddShowAllServicesSupport();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseShowAllServicesMiddleware(); // see https://github.com/ardalis/AspNetCoreStartupServices
}
else
{
    app.UseDefaultExceptionHandler(); // from FastEndpoints
    app.UseHsts();
}

app.UseAuthentication()
   .UseAuthorization()
   .UseFastEndpoints()
   .UseSwaggerGen(); // Includes AddFileServer and static files middleware

app.UseHttpsRedirection();

app.Run();


void ConfigureMediatR()
{
    var mediatRAssemblies = new[]
    {
        Assembly.GetAssembly(typeof(RegitroPontos)), // Core
        Assembly.GetAssembly(typeof(CriarRegistroCommand)), // UseCases
        Assembly.GetAssembly(typeof(IdentityService)) // Infraestruture
    };

    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!));
    builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    builder.Services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
}

void AddShowAllServicesSupport()
{
    // add list services for diagnostic purposes - see https://github.com/ardalis/AspNetCoreStartupServices
    builder.Services.Configure<ServiceConfig>(config =>
    {
        config.Services = new List<ServiceDescriptor>(builder.Services);

        // optional - default path to view services is /listallservices - recommended to choose your own path
        config.Path = "/listservices";
    });
}
