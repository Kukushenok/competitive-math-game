// File: IHttpClient.cs
using ClientUsage.Client;
using CompetitiveBackend.BackendUsage.UseCases;

// File: HttpClientExtensions.cs
using CompetitiveBackend.BackendUsage.Objects;

namespace ClientUsage.UseCases
{
    internal class PlayerProfileUseCase : IPlayerProfileUseCase
    {
        private readonly IHttpClient _client;
        public PlayerProfileUseCase(IHttpClient client) => _client = client ?? throw new ArgumentNullException(nameof(client));

        public Task<PlayerProfileDTO> GetProfile(int id)
            => _client.Get<PlayerProfileDTO>($"/api/v1/players/{id}");

        public async Task<LargeDataDTO> GetProfileImage(int id)
        {
            var resp = await _client.Get<LargeDataDTO>($"/api/v1/players/{id}/image");
            return resp;
        }
    }
}