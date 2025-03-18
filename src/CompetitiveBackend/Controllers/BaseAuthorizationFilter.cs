using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;

namespace CompetitiveBackend.Controllers
{
    public abstract class SessionTokenRequirement : IAuthorizationRequirement
    {
        public abstract bool Check(SessionToken s);
    }
    public class AdminTokenRequirement : SessionTokenRequirement
    {
        public override bool Check(SessionToken s) => s.IsAuthenticated() && s.Role.IsAdmin();
    }
    public class PlayerTokenRequirement : SessionTokenRequirement
    {
        public override bool Check(SessionToken s) => s.IsAuthenticated() && s.Role.IsPlayer();
    }
    public class AuthOptions : AuthenticationSchemeOptions { }
    public class SessionTokenAuthenticationHandler : AuthenticationHandler<AuthOptions>
    {
        IAuthService authService;
        public SessionTokenAuthenticationHandler(IAuthService service, IOptionsMonitor<AuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            authService = service;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request?.TryGetTokenValue(out string token) ?? false)
            {
                SessionToken s = await authService.GetSessionToken(token);
                if (s.TryGetAccountIdentifier(out int id))
                {
                    var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name, token),
                            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                            new Claim(ClaimTypes.Role, s.Role.ToString())
                        };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new GenericPrincipal(identity, AuthUtilite.GetAllRoles().ToArray());
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
            }
            return AuthenticateResult.NoResult();

        }
    }
    public class SessionTokenAuthorizationHandler : AuthorizationHandler<SessionTokenRequirement>
    {
        IAuthService authService;
        public SessionTokenAuthorizationHandler(IAuthService serv)
        {
            authService = serv;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SessionTokenRequirement requirement)
        {
            var s = context.User.GetSessionToken();
            if (requirement.Check(s)) context.Succeed(requirement);
            else context.Fail();
            return Task.CompletedTask;
        }
    }
}
