using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using CompetitiveBackend.Services;
using CompetitiveBackend.Core.Auth;
using Microsoft.AspNetCore.Http;

namespace CompetitiveBackend.Controllers
{
    public abstract class SessionTokenRequirement: IAuthorizationRequirement
    {
        public abstract bool Check(SessionToken s);
    }
    public class AdminTokenRequirement: SessionTokenRequirement
    {
        public override bool Check(SessionToken s) => s.IsAuthenticated() && s.Role.IsAdmin();
    }
    public class PlayerTokenRequirement: SessionTokenRequirement
    {
        public override bool Check(SessionToken s) => s.IsAuthenticated() && s.Role.IsPlayer();
    }
    public class SessionTokenAuthorizationHandler : AuthorizationHandler<SessionTokenRequirement>
    {
        IAuthService authService;
        public SessionTokenAuthorizationHandler(IAuthService serv)
        {
            authService = serv;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SessionTokenRequirement requirement)
        {
            var httpContext = context.Resource as HttpContext;
            if (httpContext?.TryGetTokenValue(out string token) ?? false)
            {
                SessionToken s = await authService.GetSessionToken(token);
                if (requirement.Check(s)) context.Succeed(requirement);
                else context.Fail();
            }
            else
            {
                context.Fail();
            }
        }
    }
}
