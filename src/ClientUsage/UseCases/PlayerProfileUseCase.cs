// File: IHttpClient.cs
using ClientUsage.Client;

// File: HttpClientExtensions.cs
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;

namespace ClientUsage.UseCases
{
    internal sealed class PlayerProfileUseCase : IPlayerProfileUseCase
    {
        private readonly IHttpClient client;
        public PlayerProfileUseCase(IHttpClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public Task<PlayerProfileDTO> GetProfile(int id)
        {
            return client.Get<PlayerProfileDTO>($"/api/v1/players/{id}");
        }

        public async Task<LargeDataDTO> GetProfileImage(int id)
        {
            LargeDataDTO resp = await client.Get<LargeDataDTO>($"/api/v1/players/{id}/image");
            return resp;
        }
    }
}