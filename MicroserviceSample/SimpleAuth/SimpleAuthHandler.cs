using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MicroserviceSample.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MicroserviceSample.SimpleAuth
{
    public class SimpleAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public SimpleAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IOptions<SimpleAuthOptions> authOptions)
            : base(options, logger, encoder, clock)
        {
            AuthOptions = authOptions.Value;
        }

        private SimpleAuthOptions AuthOptions { get; }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            bool isAuth = Context.Request.Query[AuthOptions.QueryKey] == AuthOptions.Secret;
            if (!isAuth)
            {
                return Task.FromResult(AuthenticateResult.Fail("Key is not correct"));
            }

            var identity = new ClaimsIdentity(SimpleAuthDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, AuthOptions.Username));

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, SimpleAuthDefaults.AuthenticationScheme);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
