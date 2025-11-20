// File: IHttpClient.cs
using ClientUsage.Client;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;

// File: HttpClientExtensions.cs
namespace ClientUsage.UseCases
{
    internal sealed class AuthUseCase : IAuthUseCase
    {
        private readonly IHttpClient client;

        public AuthUseCase(IHttpClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public Task<AuthSuccessResultDTO> Register(AccountCreationDTO creation)
        {
            return creation == null
                ? throw new ArgumentNullException(nameof(creation))
                : client.Post<AuthSuccessResultDTO>("/api/v1/auth/register", creation);
        }

        public Task<AuthSuccessResultDTO> Login(AccountLoginDTO loginRequest)
        {
            return loginRequest == null
                ? throw new ArgumentNullException(nameof(loginRequest))
                : client.Post<AuthSuccessResultDTO>("/api/v1/auth/login", loginRequest);
        }
    }
}
