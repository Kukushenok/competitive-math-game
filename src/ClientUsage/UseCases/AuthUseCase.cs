// File: IHttpClient.cs
using ClientUsage.Client;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;

// File: HttpClientExtensions.cs

namespace ClientUsage.UseCases
{
    internal class AuthUseCase : IAuthUseCase
    {
        private readonly IHttpClient _client;

        public AuthUseCase(IHttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public Task<AuthSuccessResultDTO> Register(AccountCreationDTO creation)
        {
            if (creation == null) throw new ArgumentNullException(nameof(creation));
            return _client.Post<AuthSuccessResultDTO>("/api/v1/auth/register", creation);
        }

        public Task<AuthSuccessResultDTO> Login(AccountLoginDTO loginRequest)
        {
            if (loginRequest == null) throw new ArgumentNullException(nameof(loginRequest));
            return _client.Post<AuthSuccessResultDTO>("/api/v1/auth/login", loginRequest);
        }
    }
}
