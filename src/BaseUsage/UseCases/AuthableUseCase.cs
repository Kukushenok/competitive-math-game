using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.Core.Auth;

namespace CompetitiveBackend.BaseUsage.UseCases
{
    public abstract class AuthableUseCase<T> : IDisposable, IAuthableUseCase<T>
        where T : AuthableUseCase<T>
    {
        protected SessionToken User => tokenToUse ?? new UnauthenticatedSessionToken();
        private SessionToken? tokenToUse;
        public async Task<T> Auth(string token)
        {
            SessionToken tkn = await GetSessionToken(token);
            T result = Clone();
            result.tokenToUse = tkn;
            return result;
        }

        protected abstract Task<SessionToken> GetSessionToken(string token);
        protected virtual T Clone()
        {
            return (T)MemberwiseClone();
        }

        public void Dispose()
        {
            tokenToUse = null;
        }
    }
}
