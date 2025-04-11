using CompetitiveBackend.Core.Auth;

namespace CompetitiveBackend.BackendUsage
{
    public abstract class AuthableUseCase<T> : IDisposable, IAuthableUseCase<T> where T : AuthableUseCase<T>
    {
        protected SessionToken User { get => tokenToUse ?? new UnauthenticatedSessionToken(); }
        private SessionToken? tokenToUse;
        public async Task<T> Auth(string token)
        {
            var tkn = await GetSessionToken(token);
            T result = Clone();
            result.tokenToUse = tkn;
            return result;
        }
        protected abstract Task<SessionToken> GetSessionToken(string token);
        protected virtual T Clone() => (T)MemberwiseClone();

        public void Dispose()
        {
            tokenToUse = null;
        }
    }
}
