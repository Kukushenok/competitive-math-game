using CompetitiveBackend.Repositories;
using CompetitiveBackend.Services.AccountSevice;
using Microsoft.AspNetCore.Authorization;

namespace CompetitiveBackend.LogicComponents
{
    public class SessionTokenAttribute: AuthorizeAttribute, IAuthorizationRequirement
    {
        public bool Check(SessionToken token)
        {
            return token.IsAuthenticated();
        }
    }
    public class SessionTokenPolicy : AuthorizationHandler<SessionTokenAttribute>
    {
        private IAuthService resolver;
        public SessionTokenPolicy(IAuthService resolver)
        {
            this.resolver = resolver;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SessionTokenAttribute requirement)
        {
            SessionToken token = await resolver.GetSessionToken(context.User.Identity.Name);
            if (requirement.Check(token))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
