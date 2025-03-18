using CompetitiveBackend.Core.Auth;
using Microsoft.Extensions.Primitives;
using System.Diagnostics;
using System.Security.Claims;

namespace CompetitiveBackend.Controllers
{
    public static class AuthUtilite
    {
        private static readonly Role[] REGISTERED_ROLES = new Role[] { new PlayerRole(), new AdminRole(), new GuestRole()};
        public static IEnumerable<string> GetAllRoles()
        {
            foreach (var r in REGISTERED_ROLES) yield return r.ToString();
        }
        public static bool TryGetTokenValue(this HttpRequest request, out string value)
        {
            StringValues q;
            value = null!;
            if ((request?.Headers.TryGetValue("Bearer", out q) ?? false) && q[0] != null)
            {
                value = q[0]!;
                return true;
            }
            return false;
        }
        public static bool TryGetTokenValue(this HttpContext context, out string value)
        {
            value = null!;
            return context?.Request.TryGetTokenValue(out value) ?? false;
        }
        public static SessionToken GetSessionToken(this ClaimsPrincipal user)
        {
            if (!user.Identity?.IsAuthenticated ?? true) return new UnauthenticatedSessionToken();
            int id = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            foreach (Role rl in REGISTERED_ROLES)
            {
                if (user.IsInRole(rl.ToString())) return new AuthenticatedSessionToken(rl, id);
            }
            return new UnauthenticatedSessionToken();
        }
    }
}
