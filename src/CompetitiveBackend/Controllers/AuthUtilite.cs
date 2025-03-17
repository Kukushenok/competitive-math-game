using Microsoft.Extensions.Primitives;

namespace CompetitiveBackend.Controllers
{
    public static class AuthUtilite
    {
        public static bool TryGetTokenValue(this HttpContext context, out string value)
        {
            StringValues q;
            value = null!;
            if ((context?.Request.Headers.TryGetValue("Bearer", out q) ?? false) && q[0] != null)
            {
                value = q[0]!;
                return true;
            }
            return false;
        }
    }
}
