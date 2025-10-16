using ClientUsage.Client;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BackendUsage.Objects;
using ClientUsage.Objects;

namespace ClientUsage.UseCases
{
    internal class SelfUseCase : AuthableUseCaseBase<ISelfUseCase>, ISelfUseCase
    {
        public SelfUseCase(IHttpClient client) : base(client) { }

        public override Task<ISelfUseCase> Auth(string token)
        {
            var authClient = CreateAuthClient(token);
            ISelfUseCase impl = new SelfUseCase(authClient);
            return Task.FromResult(impl);
        }

        public Task UpdateMyImage(LargeDataDTO data)
        {
            using var ms = new MemoryStream(data.Data ?? Array.Empty<byte>());
            return _client.PostMultipartNoContent("/api/v1/players/me/image", ms, "file", "file.bin", "application/octet-stream");
        }

        public Task<PlayerProfileDTO> GetMyProfile()
            => _client.Get<PlayerProfileDTO>("/api/v1/players/me/profile");

        public Task<LargeDataDTO> GetMyImage()
            => _client.Get<LargeDataDTO>("/api/v1/players/me/image");

        public Task UpdateMyPlayerProfile(PlayerProfileDTO p)
            => _client.PutNoContent("/api/v1/players/me/profile", p);
    }
}
