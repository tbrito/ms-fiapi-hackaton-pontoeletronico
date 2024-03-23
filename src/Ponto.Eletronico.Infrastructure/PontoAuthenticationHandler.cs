using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Authentication;

namespace Ponto.Eletronico.Infrastructure
{
    public class PontoAuthenticationHandler : AuthenticationHandler<JwtBearerOptions>
    {
        public PontoAuthenticationHandler(
            IOptionsMonitor<JwtBearerOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string authorization = this.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorization))
            {
                return Task.FromResult(AuthenticateResult.Fail("Unable to authenticate. Authorization token does not exist. Use the Bearer Token"));
            }

            string token;

            try
            {
                token = authorization.Substring("Bearer ".Length).Trim();
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("Unable to authenticate. Use the Bearer token"));
            }

            var(principal, jwtSecurityToken) = this.GetPrincipalClaim(token);

            var ticket = new AuthenticationTicket(
                 principal,
                 new AuthenticationProperties(),
                 JwtBearerDefaults.AuthenticationScheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        private (ClaimsPrincipal ClaimsPrincipal, JwtSecurityToken JwtSecurityToken) GetPrincipalClaim(string token)
        {
            var validationParameters = this.Options.TokenValidationParameters.Clone();

            var principal = new ClaimsPrincipal();
            SecurityToken validatedToken = null;

            foreach (var validator in this.Options.SecurityTokenValidators)
            {
                if (!validator.CanReadToken(token))
                {
                    throw new AuthenticationException("Could not validate token. This token is not valid");
                }

                try
                {
                    principal = validator.ValidateToken(token, validationParameters, out validatedToken);
                }
                catch (Exception ex)
                {
                    throw new AuthenticationException(ex.Message, ex);
                }
            }

            return (principal, validatedToken as JwtSecurityToken);
        }
    }
}