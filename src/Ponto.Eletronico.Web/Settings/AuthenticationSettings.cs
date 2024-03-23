using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;

namespace Ponto.Eletronico.Web.Settings;

public static class AuthenticationSettings
{
  public static IServiceCollection AddAuthenticationSettings(this IServiceCollection services, IConfiguration configuration)
  {
    
    services.AddKeycloakAuthentication(configuration);
    
    services.AddAuthorization()
            .AddKeycloakAuthorization(configuration);
    
    return services;
  }
}
