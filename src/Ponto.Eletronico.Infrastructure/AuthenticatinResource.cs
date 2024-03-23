using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Ponto.Eletronico.Infrastructure;

public static class AuthenticationResource
{
    public static void AddKeyCloackAuthentication(this IServiceCollection services)
    {
        IdentityModelEventSource.ShowPII = true;

        var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            metadataAddress: $"http://grupo23-id.azurewebsites.net/auth/realms/hackaton/.well-known/openid-configuration",
            configRetriever: new OpenIdConnectConfigurationRetriever(),
            docRetriever: new HttpDocumentRetriever { RequireHttps = false });

        var openIdConfig = configManager.GetConfigurationAsync().ConfigureAwait(false).GetAwaiter();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtAuthentication(_ =>
        {
            _.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = "800b5750-e127-428c-9978-661ca58e56b7",

                ValidateIssuer = true,
                ValidIssuers = new[] { "http://grupo23-id.azurewebsites.net/auth/realms/hackaton" },

                ValidateIssuerSigningKey = true,
                IssuerSigningKeys = openIdConfig.GetResult().SigningKeys,

                RequireExpirationTime = true,
                ValidateLifetime = true,
                RequireSignedTokens = true,
            };

            _.RequireHttpsMetadata = false;
        });

    }
}
