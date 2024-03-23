using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Ponto.Eletronico.Core.Interfaces;

namespace Ponto.Eletronico.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetUserName()
      => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
}
