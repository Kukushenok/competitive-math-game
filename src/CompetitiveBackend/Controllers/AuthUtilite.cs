using CompetitiveBackend.BackendUsage;
using CompetitiveBackend.Core.Auth;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;

namespace CompetitiveBackend.Controllers
{
    public static class AuthUtilite
    {
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
        public static async Task<T> Auth<T>(this T useCase, HttpRequest request) where T: IAuthableUseCase<T>
        {
            if(TryGetTokenValue(request, out string value))
            {
                return await useCase.Auth(value);
            }
            return await useCase.Auth(string.Empty);
        }
        public static async Task<T> Auth<T>(this T useCase, HttpContext request) where T : IAuthableUseCase<T>
        {
            return await useCase.Auth(request.Request);
        }
    }
}
