using CompetitiveBackend.BackendUsage.UseCases;
using Microsoft.Extensions.Primitives;

namespace CompetitiveBackend.Controllers
{
    public static class AuthUtilite
    {
        public static bool TryGetTokenValue(this HttpRequest request, out string value)
        {
            value = null!;
            if ((request?.Headers.TryGetValue("Bearer", out StringValues q) ?? false) && q[0] != null)
            {
                value = q[0]!;
                return true;
            }

            return false;
        }

        public static async Task<T> Auth<T>(this T useCase, HttpRequest request)
            where T : IAuthableUseCase<T>
        {
            return TryGetTokenValue(request, out string value) ? await useCase.Auth(value) : await useCase.Auth(string.Empty);
        }

        public static async Task<T> Auth<T>(this T useCase, HttpContext request)
            where T : IAuthableUseCase<T>
        {
            return await useCase.Auth(request.Request);
        }
    }
}
