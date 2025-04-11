using CompetitiveBackend.BackendUsage;
using CompetitiveBackend.BackendUsage.Exceptions;
using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Services;

namespace CompetitiveBackend.BaseUsage
{
    public class BaseAuthableUseCase<T> : AuthableUseCase<T> where T : BaseAuthableUseCase<T>
    {
        private IAuthService authService;
        public BaseAuthableUseCase(IAuthService authService)
        {
            this.authService = authService;
        }
        protected override async Task<SessionToken> GetSessionToken(string token)
        {
            return await authService.GetSessionToken(token);
        }
        protected void AuthCheck(out int id)
        {
            id = 0;
            if(!User.TryGetAccountIdentifier(out id)) throw new UnauthenticatedException();
        }
        protected void PlayerAuthCheck(out int id)
        {
            AuthCheck(out id);
            if (!User.Role.IsPlayer()) throw new IsNotPlayerException();
        }
        protected void AdminAuthCheck(out int id)
        {
            AuthCheck(out id);
            if (!User.Role.IsAdmin()) throw new OperationNotPermittedException();
        }
    }
}
