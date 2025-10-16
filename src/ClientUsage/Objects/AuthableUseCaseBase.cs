// File: IHttpClient.cs
using ClientUsage.Client;
using CompetitiveBackend.BackendUsage.UseCases;

namespace ClientUsage.Objects
{
    public abstract class AuthableUseCaseBase<TImpl> : IAuthableUseCase<TImpl>
        where TImpl : IAuthableUseCase<TImpl>
    {
        protected readonly IHttpClient _client;
        protected AuthableUseCaseBase(IHttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public abstract Task<TImpl> Auth(string token);

        public virtual void Dispose()
        {
            if (_client is IDisposable d) d.Dispose();
        }

        protected IHttpClient CreateAuthClient(string token) => new AuthenticatedHttpClient(_client, token);
    }
}
