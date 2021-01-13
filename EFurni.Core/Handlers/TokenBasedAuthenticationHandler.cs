using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IAuthenticationService = EFurni.Services.IAuthenticationService;

namespace EFurni.Core.Handlers
{
    public class TokenBasedAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IAuthenticationService _authenticationService;

        public TokenBasedAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            IAuthenticationService authenticationService,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _authenticationService = authenticationService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Token header was not found");
            }

            string authToken = Request.Headers["Authorization"];

            var authenticated = await _authenticationService.AuthenticateUser(authToken);

            Claim[] userClaims;

            if (!authenticated)
            {
                userClaims = new[]
                {
                    new Claim(ClaimTypes.Actor, authToken),
                    new Claim(ClaimTypes.Role, "Untrusted")
                };
            }
            else
            {
                userClaims = new[]
                {
                    new Claim(ClaimTypes.Actor, authToken),
                    new Claim(ClaimTypes.Role, "Trusted")
                };
            }

            var authenticationTicket = CreateTicket(userClaims);
            return AuthenticateResult.Success(authenticationTicket);
        }

        private AuthenticationTicket CreateTicket(Claim[] userClaims)
        {
            var identity = new ClaimsIdentity(userClaims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return ticket;
        }
    }
}