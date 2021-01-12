using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EFurni.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IAuthenticationService = EFurni.Services.IAuthenticationService;

namespace EFurni.Core.Handlers
{
    public class TokenBasedAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        
        private const int MinLength = 6;
        private const int MaxLength = 48;

        public TokenBasedAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            IAuthenticationService tokenRepository,
            IConfiguration config,
            UrlEncoder encoder,
            ISystemClock clock,
            ITokenService tokenService) : base(options, logger, encoder, clock)
        {
            _config = config;
            _tokenService = tokenService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (bool.Parse(_config["AuthenticationService:AllowAnonymousApiCalls"]))
            {
                var s =
                    new[]
                    {
                        new Claim(ClaimTypes.Name,"testuser@gmail.com"),
                        new Claim(ClaimTypes.NameIdentifier,"1"),
                        new Claim(ClaimTypes.Role, "Trusted")
                    };

                var i = new ClaimsIdentity(s, Scheme.Name);
                var p = new ClaimsPrincipal(i);
                var t = new AuthenticationTicket(p, Scheme.Name);

                return AuthenticateResult.Success(t);
            }
            
                
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Token header was not found");
            }

            string authToken = Request.Headers["Authorization"];

            if (authToken.Length < MinLength || authToken.Length > MaxLength)
            {
                return AuthenticateResult.Fail("Token too long or too short to process");
            }

            var tokenAccount = await _tokenService.GetTokenAccountAsync(authToken);

            Claim[] userClaims;
            
            if (tokenAccount != null)
            {
                userClaims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, tokenAccount.AccountId.ToString()),
                    new Claim(ClaimTypes.Role, "Trusted")
                };
            }
            else
            {
                userClaims = new[]
                {
                    new Claim(ClaimTypes.Name, authToken),
                    new Claim(ClaimTypes.Role, "Untrusted")
                };
            }
            
            var identity = new ClaimsIdentity(userClaims,Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal,Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}